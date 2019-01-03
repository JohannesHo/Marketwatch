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
    public partial class FormBuyItem : Form {
        public PictureBox[] stickerBoxes { get; private set; } = new PictureBox[4];
        public FormBuyItem() {
            InitializeComponent();
            stickerBoxes[0] = stickerPictureBox1;
            stickerBoxes[1] = stickerPictureBox2;
            stickerBoxes[2] = stickerPictureBox3;
            stickerBoxes[3] = stickerPictureBox4;
        }
    }
}
