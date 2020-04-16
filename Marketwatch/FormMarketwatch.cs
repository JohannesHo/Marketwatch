using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SteamKit2;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;
using UpdateLib;

namespace Marketwatch {
    public partial class FormMarketwatch : Form, ISharpUpdatable {
        public event EventHandler<ItemEventArgs> newItem;

        public ConcurrentDictionary<UInt64, UInt64> lookupDict = new ConcurrentDictionary<UInt64, UInt64>();
        public ConcurrentDictionary<UInt64, Item> items = new ConcurrentDictionary<UInt64, Item>();
        public ConcurrentDictionary<UInt64, Item> itemsReady = new ConcurrentDictionary<UInt64, Item>();

        public SteamWorker steamWorker;

        public SharpUpdater updater;

        public string ApplicationName {
            get {
                return "Marketwatch";
            }
        }

        public string ApplicationID {
            get {
                return "Marketwatch";
            }
        }

        public Assembly ApplicationAssembly {
            get {
                return Assembly.GetExecutingAssembly();
            }
        }

        public Icon ApplicationIcon {
            get {
                return this.Icon;
            }
        }

        public Uri UpdateXmlLocation {
            get {
                return new Uri("http://marketwatch.mooo.com/update.xml");
            }
        }

        public Form Context {
            get {
                return this;
            }
        }

        public FormMarketwatch() {
            InitializeComponent();

            DataGridViewTextBoxColumn itemColumnOld = (DataGridViewTextBoxColumn)dataGridView.Columns[itemNameColumn.Index];
            DataGridViewRichTextBoxColumn itemColumnNew = new DataGridViewRichTextBoxColumn();

            itemColumnNew.Name = itemColumnOld.Name;
            itemColumnNew.HeaderText = itemColumnOld.HeaderText;
            itemColumnNew.ReadOnly = itemColumnOld.ReadOnly;
            itemColumnNew.AutoSizeMode = itemColumnOld.AutoSizeMode;

            dataGridView.Columns.RemoveAt(itemNameColumn.Index);
            dataGridView.Columns.Insert(itemNameColumn.Index, itemColumnNew);

            DataGridViewRow template = this.dataGridView.RowTemplate;
            template.Height = 32;
            template.CreateCells(dataGridView);
            template.Cells[inspectButtonColumn.Index].Value = Resources.strings.INSPECT_ITEM;
            template.Cells[buyButtonColumn.Index].Value = Resources.strings.BUY_ITEM;

            menuStrip.Renderer = new ToolStripRenderer();

            updater = new SharpUpdater(this);
        }

        private void searchButton_Click(object sender, EventArgs e) {
            //For all Items
            String Pistols = "CZ75%20Auto|Desert%20Eagle|Dual%20Berettas|Five-SeveN|Glock-18|P2000|P250|R8%20Revolver|Tec-9|USP-S";
            String Rifles = "AK-47|AUG|AWP|FAMAS|G3SG1|Galil%20AR|M4A1-S|M4A4|SCAR-20|SG%20553|SSG%2008";
            String SMGs = "MAC-10|MP5-SD|MP7|MP9|PP-Bizon|P90|UMP-45";
            String Heavy = "MAG-7|Nova|Sawed-Off|XM1014|M249|Negev";
            String Weapons = Pistols + "|" + Rifles + "|" + SMGs + "|" + Heavy;
            String Knifes = "Bayonet|Bowie%20Knife|Butterfly%20Knife|Falchion%20Knife|Flip%20Knife|Gut%20Knife|Huntsman%20Knife|Karambit|M9%20Bayonet|Shadow%20Daggers|Navaja%20Knife|Stiletto%20Knife|Ursus%20Knife|Talon%20Knife|Classic%20Knife|Nomad%20Knife|Survival%20Knife|Paracord%20Knife|Skeleton%20Knife";
            String Gloves = "Bloodhound%20Gloves|Driver%20Gloves|Hand%20Wraps|Hydra%20Gloves|Moto%20Gloves|Specialist%20Gloves|Sport%20Gloves";
            String Conditions = "Factory%20New|Minimal%20Wear|Field-Tested|Well-Worn|Battle-Scarred";

            Match communityLink = Regex.Match(searchBox.Text, @"^https?://steamcommunity.com/market/listings/730/(?:%E2%98%85%20)?(?:(?<Modifier>StatTrak%E2%84%A2|Souvenir)%20)?(?:(?<Weapon>" + Weapons + ")|(?<Knife>" + Knifes + ")|(?<Glove>" + Gloves + "))(?(%20)(?:%20(?:%7C|\\|)%20(?<Skin>[A-z,0-9,%,-]+)%20(?:%28|\\()(?<Condition>" + Conditions + ")(?:%29|\\))))$");
            if (communityLink.Success) {
                clear();
                searchBox.Enabled = false;

                HtmlAgilityPack.HtmlWeb hw = new HtmlAgilityPack.HtmlWeb();
                HtmlAgilityPack.HtmlDocument doc = hw.Load(searchBox.Text + "?count=1");
                UInt32 searchResults_total = UInt32.Parse(doc.DocumentNode.SelectSingleNode("//span[@id='searchResults_total']").InnerText.Replace(",", ""));

                labelSeparator.Visible = true;
                labelCurrent.Text = String.Format("{0,4}", 0);
                labelTotal.Text = String.Format("{0,4}", searchResults_total);

                List<string> prepareLinks = new List<string>();
                for (UInt32 start = 0; (start < (searchResults_total / 100) + 1) && start < Properties.Settings.Default.maxRequest; start++)
                    prepareLinks.Add(searchBox.Text + "/render/?query=&start=" + start * 100 + "&count=100&currency=" + Properties.Settings.Default.currency);


                List<Task> prepareTasks = new List<Task>();
                ECurrencyCode currencyCode = (ECurrencyCode)Enum.Parse(typeof(ECurrencyCode), Properties.Settings.Default.currency.ToString());
                foreach (String link in prepareLinks)
                    prepareTasks.Add(Task.Factory.StartNew(() => prepareLists(link, currencyCode)));


                Task.WaitAll(prepareTasks.ToArray());

                beginSubmittingRows();
            } else
                MessageBox.Show(Resources.strings.FIX_LINK_MSGBOX_TEXT + "http://steamcommunity.com/market/listings/730/AK-47%20%7C%20Redline%20%28Minimal%20Wear%29", Resources.strings.FIX_LINK_MSGBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        }

        private void prepareLists(string link, ECurrencyCode currencyCode) {
            string jsonString;
            using (WebClient wc = new WebClient()) {
                try {
                    jsonString = wc.DownloadString(link);
                } catch (WebException ex) {
                    Console.Error.WriteLine("Error getting data from steam. Error: " + ex.Message);
                    return;
                }

            }
            Console.WriteLine(link);
            JObject root = JObject.Parse(jsonString);
            string results_html = root.GetValue("results_html").ToString();
            JToken listinginfo = root.GetValue("listinginfo");
            JToken assets = root.SelectToken(@"assets.730.2");

            foreach (JToken info in listinginfo) {
                Item item = new Item();
                item.param_s = 0;
                item.param_a = UInt64.Parse(info.First.SelectToken(@"asset").Value<String>("id"));
                item.param_d = UInt64.Parse(Regex.Match(info.First.SelectToken(@"asset.market_actions[0]").Value<String>("link"), "\\d+$").Value);
                item.param_m = UInt64.Parse(info.First.Value<String>("listingid"));

                item.convertedPrice = info.First.Value<String>("converted_price");
                item.convertedFee = info.First.Value<String>("converted_fee");

                if (item.convertedPrice != null && item.convertedFee != null) {
                    item.convertedTotal = String.Format("{0}", UInt64.Parse(item.convertedPrice) + UInt64.Parse(item.convertedFee));
                    item.price = String.Format(Currency.GetCultureInfoByCurrencySymbol(currencyCode.ToString()), "{0:C}", UInt64.Parse(item.convertedTotal) / 100.0);

                } else
                    item.price = Resources.strings.SOLD;


                if (!lookupDict.TryAdd(item.param_a, item.param_m))
                    Console.Error.WriteLine("Duplicated item detected {0}", item);
                else if (!items.TryAdd(item.param_m, item))
                    Console.Error.WriteLine("Could not add {0}", item);
            }

            foreach (JToken asset in assets) {
                UInt64 listingId = UInt64.Parse(Regex.Match(asset.First.SelectToken(@"market_actions[0]").Value<String>("link"), "(?!.*\\dM)(\\d+)(?=A)").Value);

                Item item;
                if (!items.TryRemove(listingId, out item)) {
                    Console.Error.WriteLine("Could not remove item with listingId {0}", listingId);
                    continue;
                }

                //set name if there is a custom one use it else the std item name
                JToken nameTag;
                if ((nameTag = asset.First.SelectToken(@"fraudwarnings[0]")) != null) {
                    item.name = Regex.Match(nameTag.ToString().Replace("''", "\""), "\".*\"$").Value;
                    item.hasNameTag = true;
                } else
                    item.name = asset.First.Value<String>("name");

                //check for stickers
                JToken stickers;
                if ((stickers = asset.First.SelectToken(@"descriptions").Last) != null) {
                    string html_Sticker = stickers.Value<String>("value");

                    if (!String.IsNullOrWhiteSpace(html_Sticker)) {
                        item.name = item.name + " ( * )";

                        MatchCollection stickerURLs = Regex.Matches(html_Sticker, "(?<=src=\")[^ \"]*(?=\")");
                        foreach (Match stickerURL in stickerURLs)
                            item.stickerImages.Add(getImageFromURL(stickerURL.Value));

                        item.stickerNames = Regex.Match(html_Sticker, "(?<=Sticker: ).*(?=</center>)").Value.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                    }
                }



                if (!items.TryAdd(item.param_m, item))
                    Console.Error.WriteLine("Could not readd {0}", item);
            }

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(results_html);

            foreach (HtmlAgilityPack.HtmlNode accImageURL in doc.DocumentNode.SelectNodes("//span[@class='market_listing_owner_avatar']")) {

                string listingIdString = accImageURL.SelectSingleNode("../..//span[@class='market_listing_item_name']").GetAttributeValue("id", "");
                UInt64 listingId = UInt64.Parse(Regex.Match(listingIdString, "(\\d+)(?!.*\\d)").Value);

                Item item;
                if (!items.TryRemove(listingId, out item)) {
                    Console.Error.WriteLine("Could not remove item with listingId {0}", listingId);
                    continue;
                }

                item.ownerImage = getImageFromURL(accImageURL.SelectSingleNode(".//img").GetAttributeValue("src", ""));

                if (!items.TryAdd(item.param_m, item))
                    Console.Error.WriteLine("Could not readd {0}", item);
            }

            foreach (HtmlAgilityPack.HtmlNode previewImageURL in doc.DocumentNode.SelectNodes("//div[@class='market_listing_item_img_container']/img")) {
                string listingIdString = previewImageURL.GetAttributeValue("id", "");
                UInt64 listingId = UInt64.Parse(Regex.Match(listingIdString, "\\d+").Value);

                Item item;
                if (!items.TryRemove(listingId, out item)) {
                    Console.Error.WriteLine("Could not remove item with listingId {0}", listingId);
                    continue;
                }

                string previeImageURLString = previewImageURL.GetAttributeValue("src", "");
                item.previewImage = getImageFromURL(previeImageURLString.Replace("/62fx62f", "/32fx32f"));
                item.previewHighResolutionImageURL = previeImageURLString.Replace("/62fx62f", "/220fx220f");

                if (!items.TryAdd(item.param_m, item))
                    Console.Error.WriteLine("Could not add {0} to ready", item);
            }
        }

        private void beginSubmittingRows() {
            foreach (KeyValuePair<UInt64, Item> pair in items)
                newItem?.Invoke(this, new ItemEventArgs(pair.Value));
        }

        private async void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            if (((DataGridView)sender).Columns[e.ColumnIndex].GetType() == typeof(DataGridViewButtonColumn)) {
                Object link = ((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex + 2].Value;
                if (link == null)
                    return;
                if (e.ColumnIndex + 2 == inspectLinkColumn.Index) {
                    if (Marketwatch.isDebug)
                        Clipboard.SetText(link.ToString());
                    Process.Start(link.ToString());
                } else if (e.ColumnIndex + 2 == buyLinkColumn.Index) {
                    UInt64 listingId = UInt64.Parse(Regex.Match(link.ToString(), "(\\d{2,})(?=',)").Value);
                    Item item;
                    if (!items.TryGetValue(listingId, out item) || item.price.Equals(Resources.strings.SOLD))
                        return;

                    NameValueCollection data = new NameValueCollection();
                    data.Add("sessionid", steamWorker.steamWeb.SessionId);
                    data.Add("currency", Properties.Settings.Default.currency.ToString());
                    data.Add("subtotal", item.convertedPrice);
                    data.Add("fee", item.convertedFee);
                    data.Add("total", item.convertedTotal);
                    data.Add("quantity", "1");

                    using (FormBuyItem dialog = new FormBuyItem()) {
                        string currencySymbol = Enum.GetName(typeof(ECurrencyCode), Properties.Settings.Default.currency);
                        dialog.previewPictureBox.Load(item.previewHighResolutionImageURL);
                        dialog.sellerPictureBox.Image = await item.ownerImage;
                        dialog.labelItemPrice.Text = String.Format(Currency.GetCultureInfoByCurrencySymbol(currencySymbol), "{0:C}", UInt64.Parse(item.convertedPrice) / 100.0);
                        dialog.labelFees.Text = String.Format(Currency.GetCultureInfoByCurrencySymbol(currencySymbol), "{0:C}", UInt64.Parse(item.convertedFee) / 100.0);
                        dialog.labelTotal.Text = item.price;
                        dialog.labelFloatValue.Text = item.floatValue.ToString();

                        string displayName;
                        //remove " (*) as its not part of the name and indicates stickers
                        if (item.name.Contains("( * )"))
                            displayName = item.name.Remove(item.name.LastIndexOf('"') + 1);
                        else
                            displayName = item.name;

                        dialog.labelDisplayName.Text = displayName;

                        for (int i = 0; i < item.stickerImages.Count; i++) {
                            dialog.toolTip.SetToolTip(dialog.stickerBoxes[i], item.stickerNames[i]);
                            dialog.stickerBoxes[i].Image = await item.stickerImages[i];
                        }


                        NumberFormatInfo numberFormatInfo = Currency.GetCultureInfoByCurrencySymbol(currencySymbol).NumberFormat;

                        Decimal price = Decimal.Parse(item.price, NumberStyles.Currency, numberFormatInfo);
                        Decimal balance = Decimal.Parse(this.walletAmount.Text, NumberStyles.Currency, numberFormatInfo);

                        if (price > balance) {
                            if (!Marketwatch.isDebug)
                                dialog.purchaseButton.Enabled = false;
                            dialog.labelInsufficientFunds.Visible = true;
                            dialog.pictureBoxWarning.Visible = true;
                        }


                        DialogResult result = dialog.ShowDialog();

                        if (result == DialogResult.OK)
                            if (Marketwatch.isDebug)
                                Clipboard.SetText("BuyMarketListing('listing', '" + item.param_m + "', 730, '2', '" + item.param_a + "')");
                            else {
                                string response = steamWorker.steamWeb.Fetch(url: "https://steamcommunity.com/market/buylisting/" + item.param_m, method: "POST", data: data, referer: searchBox.Text);
                                try {
                                     JToken jsonResponse = JToken.Parse(response);
                                    int responseCode = jsonResponse.SelectToken(@"wallet_info.success").Value<int>();
                                    if (responseCode == 1)
                                        MessageBox.Show("Successfully purchased skin.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                } catch (Exception ex) {
                                    Console.WriteLine(response);
                                }
                            }
                    }
                }
                 
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e) {
            dataGridView.ClearSelection();
        }

        private void clearButton_Click(object sender, EventArgs e) {
            clear();
        }

        private void clear() {
            dataGridView.Rows.Clear();
            lookupDict.Clear();
            items.Clear();
            steamWorker.itemQueue.Clear();
            labelSeparator.Visible = false;
            labelCurrent.Text = "";
            labelTotal.Text = "";
            searchBox.Enabled = true;
        }

        private void searchButton_EnabledChanged(object sender, EventArgs e) {
            clearButton.Enabled = !clearButton.Enabled;
        }

        private async Task<Image> getImageFromURL(String url) {
            using (WebClient webClient = new WebClient()) {
                try {
                    byte[] data = await webClient.DownloadDataTaskAsync(new Uri(url));

                    using (MemoryStream mem = new MemoryStream(data)) {
                        return Image.FromStream(mem);
                    }
                } catch {
                    //Something went wrong with getting the image use a error one insteed
                    return Properties.Resources.messagebox_warning;
                }

            }
        }

        private void settingsButton_Click(object sender, EventArgs e) {
            new FormSettings().Show();
        }

        private void dataGridView_SortCompare(object sender, DataGridViewSortCompareEventArgs e) {
            if (e.Column == priceColumn) {
                if (e.CellValue1.ToString().Equals(Resources.strings.SOLD))
                    e.SortResult = 1;
                else if (e.CellValue2.ToString().Equals(Resources.strings.SOLD))
                    e.SortResult = -1;
                else {
                    NumberFormatInfo numberFormatInfo = Currency.GetCultureInfoByCurrencySymbol(Enum.GetName(typeof(ECurrencyCode), Properties.Settings.Default.currency)).NumberFormat;
                    Decimal cell1 = Decimal.Parse(e.CellValue1.ToString(), NumberStyles.Currency, numberFormatInfo);
                    Decimal cell2 = Decimal.Parse(e.CellValue2.ToString(), NumberStyles.Currency, numberFormatInfo);

                    e.SortResult = Decimal.Compare(cell1, cell2);
                    e.Handled = true;
                }
                return;
            }
        }

        private void MarketwatchForm_FormClosed(object sender, FormClosedEventArgs e) {
            Environment.Exit(0);
        }

        private void donateButton_Click(object sender, EventArgs e) {
            if (steamWorker != null && steamWorker.isSteamWebReady) {
                FormWeb webForm = new FormWeb(steamWorker.steamWeb);
                webForm.webBrowser.Navigate("https://steamcommunity.com/tradeoffer/new/?partner=44396159&token=PAn2liAp", "_self", null, "User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36");
            } else
                MessageBox.Show(Resources.strings.LOGIN_MSGBOX_TEXT, Resources.strings.LOGIN_MSGBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1); //TODO: localize
        }

        private void donateButton_MouseEnter(object sender, EventArgs e) {
            donateButton.BackgroundImage = Properties.Resources.steamDonateLowResMirror2;
        }

        private void donateButton_MouseLeave(object sender, EventArgs e) {
            donateButton.BackgroundImage = Properties.Resources.steamDonateLowResMirror1;
        }

        private void beendenToolStripMenuItem_Click(object sender, EventArgs e) {
            Environment.Exit(0);
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e) {
            new FormAbout().Show();
        }

        private void optionenToolStripMenuItem_Click(object sender, EventArgs e) {
            new FormSettings().Show();
        }

        private void FormMarketwatch_Load(object sender, EventArgs e) {
            String user = "", pass = "";

            if (Properties.Settings.Default.rememberLogin != true) {
                using (FormLogin dialog = new FormLogin()) {
                    DialogResult result = dialog.ShowDialog();

                    if (result == DialogResult.OK) {
                        user = dialog.user;
                        pass = dialog.pass;
                    }
                }
            } else {
                Byte[] entropy = Convert.FromBase64String(Properties.Settings.Default.entropy);
                Byte[] passEncrypted = Convert.FromBase64String(Properties.Settings.Default.passwordEncrypted);

                user = Properties.Settings.Default.username;

                pass = Encoding.UTF8.GetString(ProtectedData.Unprotect(passEncrypted, entropy, DataProtectionScope.CurrentUser));
            }

            steamWorker = new SteamWorker(user, pass, this);
            this.newItem = steamWorker.ItemEventHandler;
            Thread worker = new Thread(new ThreadStart(steamWorker.managingSteamEvents));
            worker.Name = "SteamWorker";
            worker.Start();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e) {
            loginToolStripMenuItem.Enabled = true;
            logoutToolStripMenuItem.Enabled = false;

            steamWorker.stopExecution();
            searchButton.Enabled = false;
            clearButton.Enabled = false;
            walletAmount.Text = "";
            accLabel.Text = "";

            Properties.Settings.Default.rememberLogin = false;
            Properties.Settings.Default.username = "";
            Properties.Settings.Default.passwordEncrypted = "";
            Properties.Settings.Default.Save();
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e) {
            String user = "", pass = "";

            using (FormLogin dialog = new FormLogin()) {
                DialogResult result = dialog.ShowDialog();

                if (result == DialogResult.OK) {
                    user = dialog.user;
                    pass = dialog.pass;
                }
            }

            steamWorker = new SteamWorker(user, pass, this);
            this.newItem = steamWorker.ItemEventHandler;
            Thread worker = new Thread(new ThreadStart(steamWorker.managingSteamEvents));
            worker.Name = "SteamWorker";
            worker.Start();

        }

        private void officialGroupToolStripMenuItem_Click(object sender, EventArgs e) {
            Process.Start("http://steamcommunity.com/groups/marketwatch-official#");
        }

        private void releaseNotesToolStripMenuItem_Click(object sender, EventArgs e) {
            new FormPatchnotes(this).Show();
        }

        private void checkForUpdateToolStripMenuItem_Click(object sender, EventArgs e) {
            updater.DoUpdate();
        }

        private void FormMarketwatch_Shown(object sender, EventArgs e) {
            if (Properties.Settings.Default.autoUpdate)
                updater.DoUpdate();
        }
    }


    public class ItemEventArgs : EventArgs {
        public Item item;
        public ItemEventArgs(Item item) {
            this.item = item;
        }
    }

    public class ToolStripRenderer : ToolStripSystemRenderer {
        public ToolStripRenderer() {
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e) {
            if (!(e.ToolStrip.GetType() == typeof(MenuStrip)))
                base.OnRenderToolStripBorder(e);
        }
    }

}