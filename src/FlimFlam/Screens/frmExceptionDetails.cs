using System;
using System.Windows.Forms;

namespace Plisky.FlimFlam { 

    internal partial class frmExceptionDetails : Form {
        private long baseIdx;

        internal frmExceptionDetails() {
            InitializeComponent();
        }

        // Default 0

        internal void InitialiseOnceDatasIn(long idxUsed) {
            if (lbxExceptionHeirachy.Items.Count > 0) {
                lbxExceptionHeirachy.SelectedIndex = 0;
            }
            baseIdx = idxUsed;
        }

        private void BtnShowAllTraceUpToException_Click(object sender, EventArgs e) {
            ShowFormsBackTraceView(false);
        }

        private void BtnShowBackTrace_Click(object sender, EventArgs e) {
            ShowFormsBackTraceView(true);
        }

        private void LbxExceptionHeirachy_SelectedIndexChanged(object sender, EventArgs e) {
            // When the selection is changed the details view displays the details about the selected exception.
            LooksLikeAnException llae = (LooksLikeAnException)lbxExceptionHeirachy.SelectedItem;
            txtSelectedExceptionDetails.Text = llae.GetDescriptionFully();
        }

        private void ShowFormsBackTraceView(bool sameThread) {
            MexCore.TheCore.ViewManager.GetBackTraceLeadingToMessage(lbxExceptionBackTrace, baseIdx, sameThread);
        }
    }
}