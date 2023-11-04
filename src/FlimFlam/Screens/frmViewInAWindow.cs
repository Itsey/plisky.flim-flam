using System.Windows.Forms;

namespace Plisky.FlimFlam { 

    internal partial class frmViewInAWindow : Form {

        internal frmViewInAWindow() {
            InitializeComponent();
            lvwExtractedViewList.Items.Clear();
        }

        internal void AddEntry(int imageidx, long gidx, string machine, string processId, string threadId, string locationData, string entry) {
            ListViewItem lvi = new ListViewItem();
            lvi.ImageIndex = imageidx;
            lvi.Tag = gidx;
            lvi.Text = gidx.ToString();
            lvi.SubItems.Add(machine + "\\" + processId);
            lvi.SubItems.Add(threadId);
            lvi.SubItems.Add(locationData);
            lvi.SubItems.Add(entry);
            lvwExtractedViewList.Items.Add(lvi);
        }

        private void btnClose_Click(object sender, System.EventArgs e) {
            Close();
        }
    }
}