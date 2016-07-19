using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Marketwatch
{
    public partial class FormLogin : Form
    {
        public string user, pass;
        public FormLogin()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (checkBoxRemember.Checked)
            {
                byte[] entropy = new byte[32];
                using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(entropy);
                }

                Properties.Settings.Default.rememberLogin = true;
                Properties.Settings.Default.username = accBox.Text;
                Properties.Settings.Default.entropy = Convert.ToBase64String(entropy);
                Properties.Settings.Default.passwordEncrypted = Convert.ToBase64String(ProtectedData.Protect(Encoding.UTF8.GetBytes(passBox.Text), entropy, DataProtectionScope.CurrentUser));
                Properties.Settings.Default.Save();
            }

            user = accBox.Text;
            pass = passBox.Text;
        }
    }
}
