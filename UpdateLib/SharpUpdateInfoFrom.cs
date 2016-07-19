using System;
using System.Windows.Forms;

namespace UpdateLib
{
    /// <summary>
    /// Form to show details about the update
    /// </summary>
    internal partial class SharpUpdateInfoFrom : Form
    {
        private const string rtfHeader = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang1031{\\fonttbl{\\f0\\fnil\\fcharset0 Segoe UI;}}\r\n\\viewkind4\\uc1\\pard\\f0\\fs17 ";
        private const string rtfTrailer = "\\par\r\n}\r\n";
        /// <summary>
        /// Creates a new SharpUpdateInfoForm
        /// </summary>
        internal SharpUpdateInfoFrom(ISharpUpdatable applicationInfo, SharpUpdateXml updateInfo)
        {
            InitializeComponent();

            // Sets the icon if it's not null
            if (applicationInfo.ApplicationIcon != null)
                this.Icon = applicationInfo.ApplicationIcon;

            // Fill in the UI
            this.Text = applicationInfo.ApplicationName + " - Update Info";
            this.lblVersions.Text = $"Current Version: {applicationInfo.ApplicationAssembly.GetName().Version.ToString()}\n Update Version: {updateInfo.Version.ToString()}";
            this.txtDescription.Rtf = rtfHeader + updateInfo.Description + rtfTrailer;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtDescription_KeyDown(object sender, KeyEventArgs e)
        {
            // Only allow Cntrl - C to copy text
            if (!(e.Control && e.KeyCode == Keys.C))
                e.SuppressKeyPress = true;
        }
    }
}
