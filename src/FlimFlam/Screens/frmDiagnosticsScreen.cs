//using Plisky.Plumbing.Legacy;
using System.Windows.Forms;

namespace Plisky.FlimFlam {

    /// <summary>
    /// Summary description for frmDiagnosticsScreen.
    /// </summary>
    internal class frmDiagnosticsScreen : System.Windows.Forms.Form {
        private System.Windows.Forms.Button btnRefreshDiagnostics;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        private System.Windows.Forms.Label label1;
        private TextBox txtAppData;
        private TextBox txtDeepInspectIndex;

        internal frmDiagnosticsScreen() {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnRefreshDiagnostics = new System.Windows.Forms.Button();
            this.txtAppData = new System.Windows.Forms.TextBox();
            this.txtDeepInspectIndex = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            //
            // label1
            //
            this.label1.Location = new System.Drawing.Point(16, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(680, 56);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            //
            // btnRefreshDiagnostics
            //
            this.btnRefreshDiagnostics.Location = new System.Drawing.Point(532, 394);
            this.btnRefreshDiagnostics.Name = "btnRefreshDiagnostics";
            this.btnRefreshDiagnostics.Size = new System.Drawing.Size(280, 23);
            this.btnRefreshDiagnostics.TabIndex = 1;
            this.btnRefreshDiagnostics.Text = "btnRefreshDiagnostics";
            this.btnRefreshDiagnostics.Click += new System.EventHandler(this.RefreshDiagnostics_Click);
            //
            // txtAppData
            //
            this.txtAppData.Location = new System.Drawing.Point(12, 156);
            this.txtAppData.Multiline = true;
            this.txtAppData.Name = "txtAppData";
            this.txtAppData.Size = new System.Drawing.Size(800, 213);
            this.txtAppData.TabIndex = 2;
            //
            // txtDeepInspectIndex
            //
            this.txtDeepInspectIndex.Location = new System.Drawing.Point(702, 16);
            this.txtDeepInspectIndex.Name = "txtDeepInspectIndex";
            this.txtDeepInspectIndex.Size = new System.Drawing.Size(100, 20);
            this.txtDeepInspectIndex.TabIndex = 3;
            this.txtDeepInspectIndex.Text = "-1";
            //
            // frmDiagnosticsScreen
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(824, 429);
            this.Controls.Add(this.txtDeepInspectIndex);
            this.Controls.Add(this.txtAppData);
            this.Controls.Add(this.btnRefreshDiagnostics);
            this.Controls.Add(this.label1);
            this.Name = "frmDiagnosticsScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmDiagnosticsScreen";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion Windows Form Designer generated code

        private void RefreshDiagnostics_Click(object sender, System.EventArgs e) {
            //Bilge.Log("About to request diagnostics.");
            //Bilge.TimeStart("DianosticsRefresh");
            long deepInspect = -1;

            if (txtDeepInspectIndex.Text != "-1") {
                deepInspect = long.Parse(txtDeepInspectIndex.Text);
            }
            MexCore.TheCore.ViewManager.RefreshView_Diagnostics(label1, txtAppData, deepInspect);
            //Bilge.TimeStop("DiangosticsRefresh");
        }
    }
}