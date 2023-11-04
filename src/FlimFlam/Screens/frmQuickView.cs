using System.Windows.Forms;

namespace Plisky.FlimFlam { 

    /// <summary>
    /// Summary description for frmQuickView.
    /// </summary>
    internal class frmQuickView : System.Windows.Forms.Form {
        private System.Windows.Forms.Button btnClose;
        private CheckBox chkRespectFilter;
        private CheckBox chkStayOnTop;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbxQuickView;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rdoAppendEnd;
        private System.Windows.Forms.RadioButton rdoInsertTop;
        private System.Windows.Forms.TextBox textBox1;

        internal frmQuickView() {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkRespectFilter = new System.Windows.Forms.CheckBox();
            this.chkStayOnTop = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.rdoInsertTop = new System.Windows.Forms.RadioButton();
            this.rdoAppendEnd = new System.Windows.Forms.RadioButton();
            this.lbxQuickView = new System.Windows.Forms.ListBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            //
            // panel1
            //
            this.panel1.Controls.Add(this.chkRespectFilter);
            this.panel1.Controls.Add(this.chkStayOnTop);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.rdoInsertTop);
            this.panel1.Controls.Add(this.rdoAppendEnd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(912, 32);
            this.panel1.TabIndex = 0;
            //
            // chkRespectFilter
            //
            this.chkRespectFilter.AutoSize = true;
            this.chkRespectFilter.Location = new System.Drawing.Point(197, 9);
            this.chkRespectFilter.Name = "chkRespectFilter";
            this.chkRespectFilter.Size = new System.Drawing.Size(83, 17);
            this.chkRespectFilter.TabIndex = 7;
            this.chkRespectFilter.Text = "Flt. Respect";
            this.chkRespectFilter.UseVisualStyleBackColor = true;
            //
            // chkStayOnTop
            //
            this.chkStayOnTop.AutoSize = true;
            this.chkStayOnTop.Location = new System.Drawing.Point(143, 9);
            this.chkStayOnTop.Name = "chkStayOnTop";
            this.chkStayOnTop.Size = new System.Drawing.Size(48, 17);
            this.chkStayOnTop.TabIndex = 6;
            this.chkStayOnTop.Text = "AOT";
            this.chkStayOnTop.UseVisualStyleBackColor = true;
            //
            // label1
            //
            this.label1.Location = new System.Drawing.Point(467, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Additional Filter:";
            //
            // textBox1
            //
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Location = new System.Drawing.Point(555, 8);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(205, 20);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "textBox1";
            //
            // btnClose
            //
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(8, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(128, 24);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            //
            // rdoInsertTop
            //
            this.rdoInsertTop.Location = new System.Drawing.Point(848, 8);
            this.rdoInsertTop.Name = "rdoInsertTop";
            this.rdoInsertTop.Size = new System.Drawing.Size(56, 16);
            this.rdoInsertTop.TabIndex = 1;
            this.rdoInsertTop.Text = "Insert";
            //
            // rdoAppendEnd
            //
            this.rdoAppendEnd.Checked = true;
            this.rdoAppendEnd.Location = new System.Drawing.Point(776, 8);
            this.rdoAppendEnd.Name = "rdoAppendEnd";
            this.rdoAppendEnd.Size = new System.Drawing.Size(64, 16);
            this.rdoAppendEnd.TabIndex = 0;
            this.rdoAppendEnd.TabStop = true;
            this.rdoAppendEnd.Text = "Append";
            //
            // lbxQuickView
            //
            this.lbxQuickView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbxQuickView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbxQuickView.Location = new System.Drawing.Point(0, 32);
            this.lbxQuickView.Name = "lbxQuickView";
            this.lbxQuickView.Size = new System.Drawing.Size(912, 119);
            this.lbxQuickView.TabIndex = 1;
            //
            // frmQuickView
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(912, 157);
            this.Controls.Add(this.lbxQuickView);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.Name = "frmQuickView";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmQuickView";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion Windows Form Designer generated code
    }
}