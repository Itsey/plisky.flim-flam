namespace MexInternals.Mex.Screens {
    partial class frmSaveOptions {
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
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.btnDoSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkIncludeODS = new System.Windows.Forms.CheckBox();
            this.chkKeepArrivalOrder = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkUseFilter = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(12, 91);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(273, 124);
            this.checkedListBox1.TabIndex = 0;
            // 
            // btnDoSave
            // 
            this.btnDoSave.Location = new System.Drawing.Point(372, 192);
            this.btnDoSave.Name = "btnDoSave";
            this.btnDoSave.Size = new System.Drawing.Size(75, 23);
            this.btnDoSave.TabIndex = 1;
            this.btnDoSave.Text = "Save";
            this.btnDoSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(291, 192);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // chkIncludeODS
            // 
            this.chkIncludeODS.AutoSize = true;
            this.chkIncludeODS.Location = new System.Drawing.Point(291, 91);
            this.chkIncludeODS.Name = "chkIncludeODS";
            this.chkIncludeODS.Size = new System.Drawing.Size(156, 17);
            this.chkIncludeODS.TabIndex = 3;
            this.chkIncludeODS.Text = "Keep Output Debug Strings";
            this.chkIncludeODS.UseVisualStyleBackColor = true;
            // 
            // chkKeepArrivalOrder
            // 
            this.chkKeepArrivalOrder.AutoSize = true;
            this.chkKeepArrivalOrder.Location = new System.Drawing.Point(291, 115);
            this.chkKeepArrivalOrder.Name = "chkKeepArrivalOrder";
            this.chkKeepArrivalOrder.Size = new System.Drawing.Size(112, 17);
            this.chkKeepArrivalOrder.TabIndex = 4;
            this.chkKeepArrivalOrder.Text = "Maintain Squence";
            this.chkKeepArrivalOrder.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Processes To Export";
            // 
            // chkUseFilter
            // 
            this.chkUseFilter.AutoSize = true;
            this.chkUseFilter.Location = new System.Drawing.Point(291, 138);
            this.chkUseFilter.Name = "chkUseFilter";
            this.chkUseFilter.Size = new System.Drawing.Size(116, 17);
            this.chkUseFilter.TabIndex = 6;
            this.chkUseFilter.Text = "Use Filter On Data.";
            this.chkUseFilter.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 20);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(272, 20);
            this.textBox1.TabIndex = 7;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(292, 20);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(116, 20);
            this.textBox2.TabIndex = 8;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(414, 20);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(33, 23);
            this.btnBrowse.TabIndex = 9;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Location";
            // 
            // frmSaveOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(451, 229);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.chkUseFilter);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkKeepArrivalOrder);
            this.Controls.Add(this.chkIncludeODS);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDoSave);
            this.Controls.Add(this.checkedListBox1);
            this.Name = "frmSaveOptions";
            this.Text = "frmSaveOptions";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button btnDoSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkIncludeODS;
        private System.Windows.Forms.CheckBox chkKeepArrivalOrder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkUseFilter;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label label2;
    }
}