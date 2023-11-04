namespace Plisky.FlimFlam { 

    /// <summary>
    /// Summary description for frmMainExtendedDetails.
    /// </summary>
    internal class frmMainExtendedDetails : System.Windows.Forms.Form {
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        private System.Windows.Forms.TextBox txtFullDetails;
        private System.Windows.Forms.TextBox txtIndex;
        private System.Windows.Forms.TextBox txtPid;

        internal frmMainExtendedDetails() {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        internal void PopulateFromIndex(long theIdx) {
            NonTracedApplicationEntry ee = MexCore.TheCore.DataManager.FindNTAEntryByIndex(theIdx);
            txtFullDetails.Text = ee.DebugEntry;
            txtIndex.Text = ee.AssignedIndex.ToString();
            txtPid.Text = ee.Pid.ToString();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMainExtendedDetails));
            this.txtFullDetails = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtPid = new System.Windows.Forms.TextBox();
            this.txtIndex = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            //
            // txtFullDetails
            //
            this.txtFullDetails.Location = new System.Drawing.Point(8, 8);
            this.txtFullDetails.Multiline = true;
            this.txtFullDetails.Name = "txtFullDetails";
            this.txtFullDetails.ReadOnly = true;
            this.txtFullDetails.Size = new System.Drawing.Size(816, 232);
            this.txtFullDetails.TabIndex = 0;
            this.txtFullDetails.Text = "textBox1";
            //
            // btnOk
            //
            this.btnOk.Location = new System.Drawing.Point(752, 296);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            //
            // btnCancel
            //
            this.btnCancel.Location = new System.Drawing.Point(672, 296);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "C&ancel";
            //
            // txtPid
            //
            this.txtPid.Location = new System.Drawing.Point(8, 248);
            this.txtPid.Name = "txtPid";
            this.txtPid.ReadOnly = true;
            this.txtPid.Size = new System.Drawing.Size(176, 20);
            this.txtPid.TabIndex = 3;
            this.txtPid.Text = "textBox2";
            //
            // txtIndex
            //
            this.txtIndex.Location = new System.Drawing.Point(240, 248);
            this.txtIndex.Name = "txtIndex";
            this.txtIndex.ReadOnly = true;
            this.txtIndex.Size = new System.Drawing.Size(192, 20);
            this.txtIndex.TabIndex = 4;
            this.txtIndex.Text = "textBox3";
            //
            // frmMainExtendedDetails
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(832, 325);
            this.Controls.Add(this.txtIndex);
            this.Controls.Add(this.txtPid);
            this.Controls.Add(this.txtFullDetails);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMainExtendedDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmMainExtendedDetails";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion Windows Form Designer generated code
    }
}