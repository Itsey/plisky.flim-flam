using System;
using System.Drawing;
using System.Windows.Forms;

namespace Plisky.FlimFlam { 

    public partial class frmTimingsAdditionalScreen : Form {
        private double excludeLessThanThisValue = double.MinValue;

        private double excludeMoreThanThisValue = double.MaxValue;

        public frmTimingsAdditionalScreen() {
            InitializeComponent();
        }

        internal AdditionalData GetAdditionalDataFromScreen() {
            AdditionalData result = new AdditionalData();
            result.UseExclusionFilters = chkUseRangefilterOnTimings.Checked;
            result.ExcludeTimingsLessThan = this.excludeLessThanThisValue;
            result.ExcludeTimingsGreaterThan = this.excludeMoreThanThisValue;
            return result;
        }

        internal void SetScreenFromAddtionalData(AdditionalData incomming) {
            if (incomming == null) { return; }

            chkUseRangefilterOnTimings.Checked = incomming.UseExclusionFilters;
            if (chkUseRangefilterOnTimings.Checked) {
                if (incomming.ExcludeTimingsLessThan != double.MinValue) {
                    txtExcludeElapsedLessThan.Text = incomming.ExcludeTimingsLessThan.ToString();
                }
                if (incomming.ExcludeTimingsGreaterThan != double.MaxValue) {
                    txtExcludeElapsedGreaterThan.Text = incomming.ExcludeTimingsGreaterThan.ToString();
                }
            } else {
                txtExcludeElapsedGreaterThan.Text = txtExcludeElapsedLessThan.Text = string.Empty;
            }
        }

        private static double GetExclusionValue(TextBox sender) {
            double result;

            if (!double.TryParse(sender.Text, out result)) {
                sender.BackColor = Color.IndianRed;
            } else {
                sender.BackColor = Color.FromKnownColor(KnownColor.Window);
            }
            return result;
        }

        private void chkUseRangefilterOnTimings_CheckedChanged(object sender, EventArgs e) {
            txtExcludeElapsedLessThan.Enabled = txtExcludeElapsedGreaterThan.Enabled = chkUseRangefilterOnTimings.Checked;
        }

        private void txtExcludeElapsedGreaterThan_TextChanged(object sender, EventArgs e) {
            excludeMoreThanThisValue = GetExclusionValue(txtExcludeElapsedGreaterThan);
        }

        private void txtExcludeElapsedLessThan_TextChanged(object sender, EventArgs e) {
            excludeLessThanThisValue = GetExclusionValue(txtExcludeElapsedLessThan);
        }
    }
}