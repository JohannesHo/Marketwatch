using SteamKit2;
using SteamKit2.GC;
using SteamKit2.GC.CSGO.Internal;
using SteamKit2.Internal;
using SteamKit2.Discovery;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Marketwatch {
    public class SteamWorker {
        private SteamClient steamClient;
        private CallbackManager manager;

        private SteamUser steamUser;
        private SteamFriends steamFriends;

        private SteamGameCoordinator steamGameCoordinator;

        private bool isRunning;

        private string user, pass;
        private string authCode, twoFactorAuth;
        private string myUserNonce;

        private int count = 0;

        private EventWaitHandle itemWaitHandle;
        private EventWaitHandle welcomeWaitHandle;

        private FormMarketwatch form;

        public Queue<Item> itemQueue { get; private set; }
        public SteamWeb steamWeb { get; private set; }

        public bool isSteamWebReady { get; private set; } = false;

        public SteamWorker(string account, string password, FormMarketwatch form) {
            user = account;
            pass = password;
            this.form = form;

            itemQueue = new Queue<Item>();
            itemWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);

            welcomeWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);

            isRunning = true;

            Thread worker = new Thread(new ThreadStart(handleUIRequests));
            worker.Name = "SteamWorker - Itemqueue";
            worker.Start();
        }

        public void managingSteamEvents() {
            var cellid = 0u;

            // if we've previously connected and saved our cellid, load it.
            if (File.Exists("cellid.txt")) {
                if (!uint.TryParse(File.ReadAllText("cellid.txt"), out cellid)) {
                    Console.WriteLine("Error parsing cellid from cellid.txt. Continuing with cellid 0.");
                    cellid = 0;
                } else {
                    Console.WriteLine($"Using persisted cell ID {cellid}");
                }
            }

            var configuration = SteamConfiguration.Create(b => b.WithCellID(cellid).WithServerListProvider(new FileStorageServerListProvider("servers_list.bin")));

            steamClient = new SteamClient(configuration);
            steamWeb = new SteamWeb();

            manager = new CallbackManager(steamClient);

            steamUser = steamClient.GetHandler<SteamUser>();
            steamFriends = steamClient.GetHandler<SteamFriends>();
            steamGameCoordinator = steamClient.GetHandler<SteamGameCoordinator>();

            manager.Subscribe<SteamClient.ConnectedCallback>(OnConnected);
            manager.Subscribe<SteamClient.DisconnectedCallback>(OnDisconnected);

            manager.Subscribe<SteamUser.LoggedOnCallback>(OnLoggedOn);
            manager.Subscribe<SteamUser.LoggedOffCallback>(OnLoggedOff);
            manager.Subscribe<SteamUser.UpdateMachineAuthCallback>(OnMachineAuth);
            manager.Subscribe<SteamUser.WalletInfoCallback>(OnWalletInfo);
            manager.Subscribe<SteamUser.LoginKeyCallback>(OnLoginKey);

            manager.Subscribe<SteamGameCoordinator.MessageCallback>(OnGCMessage);

            Console.WriteLine("Connecting to Steam...");

            SteamDirectory.LoadAsync(configuration).Wait();

            steamClient.Connect();

            while (isRunning) {
                manager.RunWaitCallbacks(TimeSpan.FromSeconds(1));
            }

            //remove Steamworker from Form
            form.steamWorker = null;
        }


        private void OnConnected(SteamClient.ConnectedCallback callback) {
            if (String.IsNullOrEmpty(user) || String.IsNullOrEmpty(pass)) {
                Console.WriteLine("Unable to sign in to Steam: bad user / password ");

                isRunning = false;
                return;
            }


            Console.WriteLine("Connected to Steam! Logging into the account '{0}'...", user);

            byte[] sentryHash = null;
            string sentryFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Marketwatch\\MarketwatchSentry_" + user + ".bin");

            if (File.Exists(sentryFilePath)) {
                // if we have a saved sentry file, read and sha-1 hash it
                byte[] sentryFile = File.ReadAllBytes(sentryFilePath);
                sentryHash = CryptoHelper.SHAHash(sentryFile);
            }


            steamUser.LogOn(new SteamUser.LogOnDetails {
                Username = user,
                Password = pass,

                // in this sample, we pass in an additional authcode
                // this value will be null (which is the default) for our first logon attempt
                AuthCode = authCode,

                // if the account is using 2-factor auth, we'll provide the two factor code instead
                // this will also be null on our first logon attempt
                TwoFactorCode = twoFactorAuth,

                // our subsequent logons use the hash of the sentry file as proof of ownership of the file
                // this will also be null for our first (no authcode) and second (authcode only) logon attempts
                SentryFileHash = sentryHash,
            });
        }

        private void OnDisconnected(SteamClient.DisconnectedCallback callback) {
            // after recieving an AccountLogonDenied, we'll be disconnected from steam
            // so after we read an authcode from the user, we need to reconnect to begin the logon flow again

            Console.WriteLine("Disconnected from Steam, reconnecting in 5...");

            Thread.Sleep(TimeSpan.FromSeconds(5));

            steamClient.Connect();
        }

        private void OnLoggedOn(SteamUser.LoggedOnCallback callback) {
            bool isSteamGuard = callback.Result == EResult.AccountLogonDenied;
            bool is2FA = callback.Result == EResult.AccountLoginDeniedNeedTwoFactor;

            if (isSteamGuard || is2FA) {
                Console.WriteLine("This account is SteamGuard protected!");

                if (is2FA)
                    handleSteamGuard(Resources.strings.AUTH_MOBILE, ref twoFactorAuth);
                else
                    handleSteamGuard(Resources.strings.AUTH_EMAIL_PRE + callback.EmailDomain + Resources.strings.AUTH_EMAIL_POST, ref authCode);

                return;
            }

            if (callback.Result != EResult.OK) {
                string msg = new StringBuilder(Resources.strings.LOGIN_UNABLE).Append(callback.Result).Append(" / ").Append(callback.ExtendedResult).ToString();
                MessageBox.Show(msg, Resources.strings.LOGIN_FAILED, MessageBoxButtons.OK, MessageBoxIcon.Error);

                isRunning = false;
                return;
            }

            Console.WriteLine("Successfully logged on!");

            form.Invoke(new MethodInvoker(delegate {
                ToolStripMenuItem item = (ToolStripMenuItem)((MenuStrip)form.Controls["menuStrip"]).Items["accToolStripMenuItem"];
                item.DropDownItems["loginToolStripMenuItem"].Enabled = false;
                item.DropDownItems["logoutToolStripMenuItem"].Enabled = true;

                Label accLabel = (Label)((FlowLayoutPanel)form.Controls["accFlowLayoutPanel"]).Controls["accLabel"];
                accLabel.Text = user;
            }));

            myUserNonce = callback.WebAPIUserNonce;

            // at this point, we'd be able to perform actions on Steam
            var requestWallet = new ClientMsgProtobuf<CMsgClientWalletInfoUpdate>(EMsg.AMGetWalletDetails);
            steamClient.Send(requestWallet);

            var playGame = new ClientMsgProtobuf<CMsgClientGamesPlayed>(EMsg.ClientGamesPlayed);
            playGame.Body.games_played.Add(new CMsgClientGamesPlayed.GamePlayed { game_id = new GameID(730), });
            steamClient.Send(playGame);

            Console.WriteLine("Waiting for CSGO to startup...");

            Thread worker = new Thread(new ThreadStart(handleClientStart));
            worker.Name = "SteamWorker - Start Client";
            worker.Start();
        }

        private void OnLoggedOff(SteamUser.LoggedOffCallback callback) {
            Console.WriteLine("Logged off of Steam: {0}", callback.Result);
        }

        private void OnMachineAuth(SteamUser.UpdateMachineAuthCallback callback) {
            Console.WriteLine("Updating sentryfile...");

            // write out our sentry file
            // ideally we'd want to write to the filename specified in the callback
            // but then this sample would require more code to find the correct sentry file to read during logon
            // for the sake of simplicity, we'll just use "sentry.bin"

            int fileSize;
            byte[] sentryHash;

            FileInfo sentryFileInfo = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Marketwatch\\MarketwatchSentry_" + user + ".bin"));
            if (!sentryFileInfo.Directory.Exists)
                sentryFileInfo.Directory.Create();

            using (var fs = File.Open(sentryFileInfo.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite)) {
                fs.Seek(callback.Offset, SeekOrigin.Begin);
                fs.Write(callback.Data, 0, callback.BytesToWrite);
                fileSize = (int)fs.Length;

                fs.Seek(0, SeekOrigin.Begin);
                using (var sha = new SHA1CryptoServiceProvider()) {
                    sentryHash = sha.ComputeHash(fs);
                }
            }

            // inform the steam servers that we're accepting this sentry file
            steamUser.SendMachineAuthResponse(new SteamUser.MachineAuthDetails {
                JobID = callback.JobID,

                FileName = callback.FileName,

                BytesWritten = callback.BytesToWrite,
                FileSize = fileSize,
                Offset = callback.Offset,

                Result = EResult.OK,
                LastError = 0,

                OneTimePassword = callback.OneTimePassword,

                SentryFileHash = sentryHash,
            });

            Console.WriteLine("Done!");
        }

        private void OnWalletInfo(SteamUser.WalletInfoCallback callback) {
            form.Invoke(new MethodInvoker(delegate {
                form.Controls["walletAmount"].Text = String.Format(Currency.GetCultureInfoByCurrencySymbol(callback.Currency.ToString()), "{0,10:C}", (callback.Balance / 100.0));
                Properties.Settings.Default.currency = int.Parse(callback.Currency.ToString("d"));
            }));
        }

        private void OnLoginKey(SteamUser.LoginKeyCallback callback) {
            isSteamWebReady = steamWeb.Authenticate(callback.UniqueID, steamClient, myUserNonce);
        }

        private async void OnGCMessage(SteamGameCoordinator.MessageCallback callback) {
            if (callback.EMsg == (uint)EGCBaseClientMsg.k_EMsgGCClientWelcome) {
                Console.WriteLine("Got Welcome");

                welcomeWaitHandle.Set();

                form.Invoke(new MethodInvoker(delegate {
                    form.Controls["searchButton"].Enabled = true;
                }));
            }

            if (callback.EMsg == (uint)ECsgoGCMsg.k_EMsgGCCStrike15_v2_Client2GCEconPreviewDataBlockResponse) {
                var response = new ClientGCMsgProtobuf<CMsgGCCStrike15_v2_Client2GCEconPreviewDataBlockResponse>((uint)ECsgoGCMsg.k_EMsgGCCStrike15_v2_Client2GCEconPreviewDataBlockResponse);
                response.Deserialize(callback.Message.GetData());

                uint paintIndex = response.Body.iteminfo.paintindex; //index
                double wearFloat = Convert.ToDouble(BitConverter.ToSingle(BitConverter.GetBytes(response.Body.iteminfo.paintwear), 0)); //float

                UInt64 listingId;
                //If item already removed skip progressing.
                if (!form.lookupDict.TryRemove(BitConverter.ToUInt64(BitConverter.GetBytes(response.Body.iteminfo.itemid), 0), out listingId))
                    return;

                //only print item float if its going to be added
                Console.WriteLine(String.Format("{0,4} {1,20}", ++count, wearFloat));

                //Else progress item
                Item currentItem;
                form.items.TryGetValue(listingId, out currentItem);

                currentItem.floatValue = wearFloat;
                currentItem.paintIndex = paintIndex;

                DataGridView dgv = (DataGridView)form.Controls["dataGridView"];

                DataGridViewRow row = await currentItem.itemToRow(dgv);

                form.Invoke(new MethodInvoker(delegate {
                    dgv.Rows.Add(row);
                    form.Controls["labelCurrent"].Text = String.Format("{0,4}", dgv.RowCount);
                }));

                itemWaitHandle.Set();

                return;
            }
        }

        public void ItemEventHandler(object sender, ItemEventArgs args) {
            itemQueue.Enqueue(args.item);
        }

        private void handleUIRequests() {
            while (isRunning) {
                while (itemQueue.Count > 0 && isRunning) {
                    Item item = itemQueue.Dequeue();
                    Console.WriteLine("Dequeued item: " + item);
                    int retryCount = 0;
                    do {
                        item.requestItemPreviewData(steamGameCoordinator); Thread.Sleep(Properties.Settings.Default.timeBetweenFloatRequests);
                    }
                    while (!itemWaitHandle.WaitOne(Properties.Settings.Default.timeBetweenFloatRequests * 6) && retryCount++ < 5 && isRunning);

                    if (retryCount > 5)
                        Console.WriteLine("Request trys depleted.");

                    itemWaitHandle.Reset();
                }
                Thread.Sleep(250);
            }
        }

        private void handleClientStart() {
            var clientHello = new ClientGCMsgProtobuf<CMsgClientHello>((uint)EGCBaseClientMsg.k_EMsgGCClientHello);
            do steamGameCoordinator.Send(clientHello, 730);
            while (!welcomeWaitHandle.WaitOne(500));
        }

        private void handleSteamGuard(string msg, ref string methode) {
            string value = null;
            DialogResult result = InputBox(Resources.strings.AUTH, msg, ref value);
            if (result == DialogResult.OK)
                methode = value;
            else if (result == DialogResult.Cancel) {
                Console.WriteLine("SteamGuard authentification canceled aborting login attempt.");
                isRunning = false;
            }
        }

        public void stopExecution() {
            Console.WriteLine("Stopping execution of SteamWorker.");
            isRunning = false;
        }

        private static DialogResult InputBox(string title, string promptText, ref string value) {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = Resources.strings.OK;
            buttonCancel.Text = Resources.strings.CANCEL;
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 46, 200, 20);
            buttonOk.SetBounds(228, 44, 75, 23);
            buttonCancel.SetBounds(309, 44, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 80);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }
    }
}