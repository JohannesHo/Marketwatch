using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Marketwatch {
    public partial class FormAbout : Form {
        public FormAbout() {
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, EventArgs e) {
            this.Dispose();
        }

        private void FormAbout_Load(object sender, EventArgs e) {
            versionLabel.Text = "Version: " + Application.ProductVersion;
        }
    }
}
