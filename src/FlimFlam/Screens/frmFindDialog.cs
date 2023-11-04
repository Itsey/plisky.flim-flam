namespace Plisky.FlimFlam { 

    /// <summary>
    /// Summary description for frmFindDialog.
    /// </summary>
    internal class frmFindDialog : System.Windows.Forms.Form {
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.CheckBox chkCaseSensitive;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        private System.Windows.Forms.TextBox txtMatchText;

        internal frmFindDialog() {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        internal enum FindMatchLocationType { CurrentPhysicalView, CurrentLogicalView, CurrentViewNoFilter };

        internal enum FindMatchUsageType { TextMatchCaseSensitive, TextMatchNoCase, RegexMatch, Unknown };

        internal string GetFindMatchText() {
            return txtMatchText.Text;
        }

        /// <summary>
        /// Returns an active find structure from the settings specified in the dialog.
        /// </summary>
        /// <returns></returns>
        internal ActiveFindStructure GetFindStructure() {
            return new ActiveFindStructure(txtMatchText.Text, !chkCaseSensitive.Checked);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        internal FindMatchLocationType GetLocationType() {
            return FindMatchLocationType.CurrentPhysicalView;
        }

        internal FindMatchUsageType GetUsageType() {
            if (chkCaseSensitive.Checked) {
                return FindMatchUsageType.TextMatchCaseSensitive;
            } else {
                return FindMatchUsageType.TextMatchNoCase;
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFindDialog));
            btnOk = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            txtMatchText = new System.Windows.Forms.TextBox();
            chkCaseSensitive = new System.Windows.Forms.CheckBox();
            SuspendLayout();
            //
            // btnOk
            //
            btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnOk.Location = new System.Drawing.Point(452, 68);
            btnOk.Name = "btnOk";
            btnOk.Size = new System.Drawing.Size(90, 28);
            btnOk.TabIndex = 0;
            btnOk.Text = "F&ind";
            //
            // btnCancel
            //
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(356, 68);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(90, 28);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "C&ancel";
            //
            // txtMatchText
            //
            txtMatchText.Location = new System.Drawing.Point(4, 15);
            txtMatchText.Name = "txtMatchText";
            txtMatchText.Size = new System.Drawing.Size(547, 23);
            txtMatchText.TabIndex = 2;
            //
            // chkCaseSensitive
            //
            chkCaseSensitive.Location = new System.Drawing.Point(6, 47);
            chkCaseSensitive.Name = "chkCaseSensitive";
            chkCaseSensitive.Size = new System.Drawing.Size(172, 19);
            chkCaseSensitive.TabIndex = 6;
            chkCaseSensitive.Text = "Case Sensitive?";
            //
            // frmFindDialog
            //
            AcceptButton = btnOk;
            AutoScaleBaseSize = new System.Drawing.Size(6, 16);
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(566, 115);
            Controls.Add(chkCaseSensitive);
            Controls.Add(txtMatchText);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Name = "frmFindDialog";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Find";
            Activated += frmFindDialog_Activated;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion Windows Form Designer generated code

        private void frmFindDialog_Activated(object sender, System.EventArgs e) {
            txtMatchText.Focus();
        }
    }
}