namespace Plisky.FlimFlam { 
    partial class frmTimingsAdditionalScreen {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTimingsAdditionalScreen));
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkUseRangefilterOnTimings = new System.Windows.Forms.CheckBox();
            this.txtExcludeElapsedLessThan = new System.Windows.Forms.TextBox();
            this.txtExcludeElapsedGreaterThan = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(154, 137);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(73, 137);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "C&ancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // chkUseRangefilterOnTimings
            // 
            this.chkUseRangefilterOnTimings.AutoSize = true;
            this.chkUseRangefilterOnTimings.Location = new System.Drawing.Point(5, 12);
            this.chkUseRangefilterOnTimings.Name = "chkUseRangefilterOnTimings";
            this.chkUseRangefilterOnTimings.Size = new System.Drawing.Size(249, 17);
            this.chkUseRangefilterOnTimings.TabIndex = 2;
            this.chkUseRangefilterOnTimings.Text = "Use an elapsed time (ms) range filter on timings:";
            this.chkUseRangefilterOnTimings.UseVisualStyleBackColor = true;
            this.chkUseRangefilterOnTimings.CheckedChanged += new System.EventHandler(this.chkUseRangefilterOnTimings_CheckedChanged);
            // 
            // txtExcludeElapsedLessThan
            // 
            this.txtExcludeElapsedLessThan.Enabled = false;
            this.txtExcludeElapsedLessThan.Location = new System.Drawing.Point(96, 64);
            this.txtExcludeElapsedLessThan.Name = "txtExcludeElapsedLessThan";
            this.txtExcludeElapsedLessThan.Size = new System.Drawing.Size(133, 20);
            this.txtExcludeElapsedLessThan.TabIndex = 3;
            this.txtExcludeElapsedLessThan.TextChanged += new System.EventHandler(this.txtExcludeElapsedLessThan_TextChanged);
            // 
            // txtExcludeElapsedGreaterThan
            // 
            this.txtExcludeElapsedGreaterThan.Enabled = false;
            this.txtExcludeElapsedGreaterThan.Location = new System.Drawing.Point(96, 103);
            this.txtExcludeElapsedGreaterThan.Name = "txtExcludeElapsedGreaterThan";
            this.txtExcludeElapsedGreaterThan.Size = new System.Drawing.Size(133, 20);
            this.txtExcludeElapsedGreaterThan.TabIndex = 4;
            this.txtExcludeElapsedGreaterThan.TextChanged += new System.EventHandler(this.txtExcludeElapsedGreaterThan_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(227, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Exclude entries whos elapsed time is less than:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(242, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Exclude entries whos elapsed time is greater than:";
            // 
            // frmTimingsAdditionalScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 172);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtExcludeElapsedGreaterThan);
            this.Controls.Add(this.txtExcludeElapsedLessThan);
            this.Controls.Add(this.chkUseRangefilterOnTimings);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmTimingsAdditionalScreen";
            this.Text = "frmTimingsAdditionalScreen";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkUseRangefilterOnTimings;
        private System.Windows.Forms.TextBox txtExcludeElapsedLessThan;
        private System.Windows.Forms.TextBox txtExcludeElapsedGreaterThan;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}