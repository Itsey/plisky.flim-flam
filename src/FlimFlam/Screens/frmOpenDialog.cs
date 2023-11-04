using System;
using System.IO;
using System.Windows.Forms;
using Plisky.Diagnostics.FlimFlam;

namespace Plisky.FlimFlam { 

    internal partial class frmOpenDialog : Form {
        private bool m_modifiedLabel;

        internal frmOpenDialog() {
            InitializeComponent();
        }

        internal string Filename {
            get { return txtFilename.Text; }
        }

        internal bool GetAsynchOK() {
            return chkAsynchImport.Checked;
        }

        internal FileImportMethod GetFileImportMethod() {
            if (rdoImportADPlus.Checked) { return FileImportMethod.ADPlusLog; }
            if (rdoImportTypeTxtFileWriter.Checked) { return FileImportMethod.TextWriterWithTexSupport; }
            if (rdoDebugViewTex.Checked) { return FileImportMethod.DebugViewTexLog; }
            return FileImportMethod.GenericLog;
        }

        internal string GetLabelIdent() {
            return txtLabelIdent.Text;
        }

        internal void InitialiseDialog(string lastFileLoaded, string[] mruList) {
            if (!string.IsNullOrEmpty(lastFileLoaded)) {
                txtFilename.Text = lastFileLoaded;
            }
            if ((mruList != null) && (mruList.Length > 0)) {
                lbxMRUList.Items.AddRange(mruList);
            }
        }

        private void btnBrowseForFile_Click(object sender, EventArgs e) {
            using (OpenFileDialog ofd = new OpenFileDialog()) {
                ofd.InitialDirectory = Environment.CurrentDirectory;
                ofd.Filter = "TexLogs (txt)|*.txt|Logs (log)|*.Log|All Files (*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK) {
                    txtFilename.Text = ofd.FileName;
                }
            }
        }

        private void lbxMRUList_SelectedIndexChanged(object sender, EventArgs e) {
            if (lbxMRUList.SelectedItem == null) { return; }
            txtFilename.Text = lbxMRUList.SelectedItem.ToString();
        }

        private void txtFilename_TextChanged(object sender, EventArgs e) {
            btnOk.Enabled = File.Exists(txtFilename.Text);
            if (!m_modifiedLabel) {
                txtLabelIdent.Text = Path.GetFileNameWithoutExtension(txtFilename.Text);
            }
        }

        // Default false
        private void txtLabelIdent_TextChanged(object sender, EventArgs e) {
            m_modifiedLabel = true;
        }
    }
}