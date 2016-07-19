using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Windows.Forms;

namespace UpdateLib
{
    public class SharpUpdater
    {
        private ISharpUpdatable applicationInfo;
        private BackgroundWorker bgWorker;

        public SharpUpdater(ISharpUpdatable applicationInfo)
        {
            this.applicationInfo = applicationInfo;

            this.bgWorker = new BackgroundWorker();
            this.bgWorker.DoWork += BgWorker_DoWork;
            this.bgWorker.RunWorkerCompleted += BgWorker_RunWorkerCompleted;
        }

        public void DoUpdate()
        {
            if (!this.bgWorker.IsBusy)
                this.bgWorker.RunWorkerAsync(this.applicationInfo);
        }

        private void BgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            ISharpUpdatable application = (ISharpUpdatable)e.Argument;

            if (!SharpUpdateXml.ExistsOnServer(application.UpdateXmlLocation))
                e.Cancel = true;
            else
                e.Result = SharpUpdateXml.Parse(application.UpdateXmlLocation, application.ApplicationID);
        }

        private void BgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                SharpUpdateXml update = (SharpUpdateXml)e.Result;

                if (update != null && update.IsNewerThan(this.applicationInfo.ApplicationAssembly.GetName().Version))
                {
                    if (new SharpUpdateAcceptForm(this.applicationInfo, update).ShowDialog(this.applicationInfo.Context) == DialogResult.Yes)
                        this.DownloadUpdate(update);
                }
            }
        }

        private void DownloadUpdate(SharpUpdateXml update)
        {
            SharpUpdateDownloadForm form = new SharpUpdateDownloadForm(update.Uri, update.MD5, this.applicationInfo.ApplicationIcon);
            DialogResult result = form.ShowDialog(this.applicationInfo.Context);

            if (result == DialogResult.OK)
            {
                string currentPath = this.applicationInfo.ApplicationAssembly.Location;
                string newPath = Path.Combine(Path.GetDirectoryName(currentPath), update.FileName);

                UpdateApplication(form.TempFilePath, currentPath, newPath, update.LaunchArgs);
                
                Application.Exit();
            } 
            else if (result == DialogResult.Abort)
            {
                MessageBox.Show("The update download was cancelled. \nThis program has not been modified.", "Update Download Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("There was a problem downloading the update. \nPlease try again later.", "Update Download Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UpdateApplication(string tempFilePath, string currentPath, string newPath, string launchArgs)
        {
            ProcessStartInfo info = new ProcessStartInfo();

            if (Path.GetExtension(newPath) == ".zip")
            {
                String tempExtractPath = Path.Combine(Path.GetDirectoryName(tempFilePath), Path.GetFileNameWithoutExtension(tempFilePath));
                ZipFile.ExtractToDirectory(tempFilePath, tempExtractPath);

                info.Arguments = $"/C Choice /C Y /N /D Y /T 4 & XCopy /E /H /Y /C /Q \"{tempExtractPath}\" \"{Path.GetDirectoryName(currentPath)}\" & " +
                                 $"Del /F /S {tempExtractPath} & Del /F /S {tempFilePath} & " +
                                 $"Start \"\" /D \"{Path.GetDirectoryName(currentPath)}\" \"{Path.GetFileName(currentPath)}\" {launchArgs}";
            }
            else
                info.Arguments = $"/C Choice /C Y /N /D Y /T 4 & Del /F /Q \"{currentPath}\" & " +
                                 $"Choice /C Y /N /D Y /T 2 & Move /Y \"{tempFilePath}\" \"{newPath}\" & " +
                                 $"Start \"\" /D \"{Path.GetDirectoryName(newPath)}\" \"{Path.GetFileName(newPath)}\" {launchArgs}";

            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.CreateNoWindow = true;
            info.FileName = "cmd.exe";
            Process.Start(info);
        }
    }
}
