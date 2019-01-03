using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace UpdateLib {
    /// <summary>
    /// Form that downloads the update
    /// </summary>
    internal partial class SharpUpdateDownloadForm : Form {
        /// <summary>
        /// The web client to download the update
        /// </summary>
        private WebClient webClient;

        /// <summary>
        /// The thread to hash the file on
        /// </summary>
        private BackgroundWorker bgWorker;

        /// <summary>
        /// A temp file name to download to
        /// </summary>
        private string tempFile;

        /// <summary>
        /// The MD5 hash of the file to download
        /// </summary>
        private string md5;

        /// <summary>
        /// Gets the temp file path for the downloaded file
        /// </summary>
        internal string TempFilePath {
            get { return this.tempFile; }
        }

        /// <summary>
        /// Creates a new SharpUpdateDownloadForm
        /// </summary>
        internal SharpUpdateDownloadForm(Uri location, string md5, Icon programIcon) {
            InitializeComponent();

            if (programIcon != null)
                this.Icon = programIcon;

            // Set the temp file name and create new 0-byte file
            tempFile = Path.GetTempFileName();

            this.md5 = md5;

            webClient = new WebClient();
            webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
            webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;

            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += BgWorker_DoWork;
            bgWorker.RunWorkerCompleted += BgWorker_RunWorkerCompleted;

            try { webClient.DownloadFileAsync(location, this.tempFile); } catch { this.DialogResult = DialogResult.No; this.Close(); }
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            this.progressBar.Value = e.ProgressPercentage;
            this.lblProgress.Text = $"Downloaded {FormatBytes(e.BytesReceived, 2, true)} of {FormatBytes(e.TotalBytesToReceive, 2, true)}";
        }

        private string FormatBytes(long bytes, int decimalPlaces, bool showByteType) {
            double newBytes = bytes;
            string formatString = "{0";
            string byteType = "B";

            if (newBytes >= 0x00000400 && newBytes < 0x00100000) {
                newBytes /= 0x00000400;
                byteType = "KB";
            } else if (newBytes >= 0x00100000 && newBytes < 0x40000000) {
                newBytes /= 0x00100000;
                byteType = "MB";
            } else {
                newBytes /= 0x40000000;
                byteType = "GB";
            }

            if (decimalPlaces > 0)
                formatString += ":0.";

            for (int i = 0; i < decimalPlaces; i++)
                formatString += "0";

            formatString += "}";

            if (showByteType)
                formatString += byteType;

            return string.Format(formatString, newBytes);
        }

        private void WebClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e) {
            if (e.Error != null) {
                this.DialogResult = DialogResult.No;
                this.Close();
            } else if (e.Cancelled) {
                this.DialogResult = DialogResult.Abort;
                this.Close();
            } else {
                lblProgress.Text = "Verifying Download...";
                progressBar.Style = ProgressBarStyle.Marquee;

                bgWorker.RunWorkerAsync(new string[] { this.tempFile, this.md5 });
            }
        }

        private void BgWorker_DoWork(object sender, DoWorkEventArgs e) {
            string file = ((string[])e.Argument)[0];
            string updateMd5 = ((string[])e.Argument)[1];

            if (Hasher.HashFile(file, HashType.MD5) != updateMd5)
                e.Result = DialogResult.No;
            else
                e.Result = DialogResult.OK;
        }

        private void BgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            this.DialogResult = (DialogResult)e.Result;
            this.Close();
        }

        private void SharpUpdateDownloadForm_FormClosed(object sender, FormClosedEventArgs e) {
            if (webClient.IsBusy) {
                webClient.CancelAsync();
                this.DialogResult = DialogResult.Abort;
            }

            if (bgWorker.IsBusy) {
                bgWorker.CancelAsync();
                this.DialogResult = DialogResult.Abort;
            }
        }
    }
}
