using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace OldFlimflam.Screens {

    public partial class frmImageData : Form {

        public frmImageData() {
            InitializeComponent();
        }

        internal void Populate(string item1, byte[] item2) {
            using (var ms = new MemoryStream(item2)) {
                pbxDisplay.Image = Bitmap.FromStream(ms);
                lblText.Text = item1;
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            pbxDisplay.SizeMode = PictureBoxSizeMode.StretchImage;
        }
    }
}