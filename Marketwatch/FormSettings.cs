using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Marketwatch
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
            currencySelector.DataSource = Enum.GetValues(typeof(SteamKit2.ECurrencyCode));
            currencySelector.SelectedIndex = Properties.Settings.Default.currency;

            if (Properties.Settings.Default.language != "")
                languageSelector.SelectedItem = Properties.Settings.Default.language;
            else
            {
                switch (CultureInfo.CurrentUICulture.EnglishName)
                {
                    //case "Chinese (Simplified, China)":
                    //case "Chinese (Traditional, China)":
                    //case "Portuguese (Brazil)":
                    //    languageSelector.SelectedItem = CultureInfo.CurrentUICulture.EnglishName;
                    //    break;
                    default:
                        languageSelector.SelectedItem = Regex.Replace(CultureInfo.CurrentUICulture.EnglishName, @"\(.+\)", "").Trim();
                        break;
                }
            }

            maxRequestTrackBar.Value = Properties.Settings.Default.maxRequest;
            settingTextBox1.Text = "" + Properties.Settings.Default.maxRequest;

            updateCheckBox.Checked = Properties.Settings.Default.autoUpdate;
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            //Save Values
            Properties.Settings.Default.currency = currencySelector.SelectedIndex;
            Properties.Settings.Default.language = languageSelector.SelectedItem.ToString();
            Properties.Settings.Default.maxRequest = maxRequestTrackBar.Value;
            Properties.Settings.Default.autoUpdate = updateCheckBox.Checked;

            Properties.Settings.Default.Save();
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void maxRequestTrackBar_Scroll(object sender, EventArgs e)
        {
            settingTextBox1.Text = "" + maxRequestTrackBar.Value;
        }

        private void languageSelector_SelectionChangeCommitted(object sender, EventArgs e)
        {
            
        }
    }
}
