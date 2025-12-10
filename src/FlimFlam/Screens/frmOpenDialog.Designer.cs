namespace Plisky.FlimFlam { 
  partial class frmOpenDialog {
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOpenDialog));
            btnOk = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            txtFilename = new System.Windows.Forms.TextBox();
            groupBox1 = new System.Windows.Forms.GroupBox();
            rdoDebugViewTex = new System.Windows.Forms.RadioButton();
            rdoDefaultLogfile = new System.Windows.Forms.RadioButton();
            rdoImportADPlus = new System.Windows.Forms.RadioButton();
            rdoImportTypeTxtFileWriter = new System.Windows.Forms.RadioButton();
            lbxMRUList = new System.Windows.Forms.ListBox();
            btnBrowseForFile = new System.Windows.Forms.Button();
            chkAsynchImport = new System.Windows.Forms.CheckBox();
            txtLabelIdent = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // btnOk
            // 
            btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnOk.Location = new System.Drawing.Point(682, 277);
            btnOk.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnOk.Name = "btnOk";
            btnOk.Size = new System.Drawing.Size(88, 27);
            btnOk.TabIndex = 0;
            btnOk.Text = "OK";
            btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(588, 277);
            btnCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(88, 27);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtFilename
            // 
            txtFilename.Location = new System.Drawing.Point(4, 1);
            txtFilename.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtFilename.Name = "txtFilename";
            txtFilename.Size = new System.Drawing.Size(713, 23);
            txtFilename.TabIndex = 2;
            txtFilename.TextChanged += TxtFilename_TextChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(rdoDebugViewTex);
            groupBox1.Controls.Add(rdoDefaultLogfile);
            groupBox1.Controls.Add(rdoImportADPlus);
            groupBox1.Controls.Add(rdoImportTypeTxtFileWriter);
            groupBox1.Location = new System.Drawing.Point(4, 31);
            groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Size = new System.Drawing.Size(387, 119);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Import file type:";
            // 
            // rdoDebugViewTex
            // 
            rdoDebugViewTex.AutoSize = true;
            rdoDebugViewTex.Location = new System.Drawing.Point(208, 22);
            rdoDebugViewTex.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            rdoDebugViewTex.Name = "rdoDebugViewTex";
            rdoDebugViewTex.Size = new System.Drawing.Size(159, 19);
            rdoDebugViewTex.TabIndex = 3;
            rdoDebugViewTex.Text = "Tex Log From DebugView";
            rdoDebugViewTex.UseVisualStyleBackColor = true;
            // 
            // rdoDefaultLogfile
            // 
            rdoDefaultLogfile.AutoSize = true;
            rdoDefaultLogfile.Location = new System.Drawing.Point(7, 75);
            rdoDefaultLogfile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            rdoDefaultLogfile.Name = "rdoDefaultLogfile";
            rdoDefaultLogfile.Size = new System.Drawing.Size(95, 19);
            rdoDefaultLogfile.TabIndex = 2;
            rdoDefaultLogfile.Text = "Just A Logfile";
            rdoDefaultLogfile.UseVisualStyleBackColor = true;
            // 
            // rdoImportADPlus
            // 
            rdoImportADPlus.AutoSize = true;
            rdoImportADPlus.Location = new System.Drawing.Point(7, 48);
            rdoImportADPlus.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            rdoImportADPlus.Name = "rdoImportADPlus";
            rdoImportADPlus.Size = new System.Drawing.Size(148, 19);
            rdoImportADPlus.TabIndex = 1;
            rdoImportADPlus.Text = "Log Where CRLF Is Lost";
            rdoImportADPlus.UseVisualStyleBackColor = true;
            // 
            // rdoImportTypeTxtFileWriter
            // 
            rdoImportTypeTxtFileWriter.AutoSize = true;
            rdoImportTypeTxtFileWriter.Checked = true;
            rdoImportTypeTxtFileWriter.Location = new System.Drawing.Point(7, 22);
            rdoImportTypeTxtFileWriter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            rdoImportTypeTxtFileWriter.Name = "rdoImportTypeTxtFileWriter";
            rdoImportTypeTxtFileWriter.Size = new System.Drawing.Size(124, 19);
            rdoImportTypeTxtFileWriter.TabIndex = 0;
            rdoImportTypeTxtFileWriter.TabStop = true;
            rdoImportTypeTxtFileWriter.Text = "Log File from Bilge";
            rdoImportTypeTxtFileWriter.UseVisualStyleBackColor = true;
            // 
            // lbxMRUList
            // 
            lbxMRUList.FormattingEnabled = true;
            lbxMRUList.ItemHeight = 15;
            lbxMRUList.Location = new System.Drawing.Point(4, 157);
            lbxMRUList.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            lbxMRUList.Name = "lbxMRUList";
            lbxMRUList.Size = new System.Drawing.Size(766, 109);
            lbxMRUList.TabIndex = 4;
            lbxMRUList.SelectedIndexChanged += LbxMRUList_SelectedIndexChanged;
            // 
            // btnBrowseForFile
            // 
            btnBrowseForFile.Location = new System.Drawing.Point(727, 1);
            btnBrowseForFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnBrowseForFile.Name = "btnBrowseForFile";
            btnBrowseForFile.Size = new System.Drawing.Size(43, 23);
            btnBrowseForFile.TabIndex = 5;
            btnBrowseForFile.Text = "...";
            btnBrowseForFile.UseVisualStyleBackColor = true;
            btnBrowseForFile.Click += BtnBrowseForFile_Click;
            // 
            // chkAsynchImport
            // 
            chkAsynchImport.AutoSize = true;
            chkAsynchImport.Checked = true;
            chkAsynchImport.CheckState = System.Windows.Forms.CheckState.Checked;
            chkAsynchImport.Location = new System.Drawing.Point(14, 285);
            chkAsynchImport.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            chkAsynchImport.Name = "chkAsynchImport";
            chkAsynchImport.Size = new System.Drawing.Size(150, 19);
            chkAsynchImport.TabIndex = 6;
            chkAsynchImport.Text = "Do background import.";
            chkAsynchImport.UseVisualStyleBackColor = true;
            // 
            // txtLabelIdent
            // 
            txtLabelIdent.BackColor = System.Drawing.SystemColors.Info;
            txtLabelIdent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtLabelIdent.Location = new System.Drawing.Point(551, 31);
            txtLabelIdent.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtLabelIdent.Name = "txtLabelIdent";
            txtLabelIdent.Size = new System.Drawing.Size(219, 23);
            txtLabelIdent.TabIndex = 7;
            txtLabelIdent.TextChanged += TxtLabelIdent_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(438, 33);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(102, 15);
            label1.TabIndex = 8;
            label1.Text = "Label This Import:";
            // 
            // frmOpenDialog
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(784, 318);
            Controls.Add(label1);
            Controls.Add(txtLabelIdent);
            Controls.Add(chkAsynchImport);
            Controls.Add(btnBrowseForFile);
            Controls.Add(lbxMRUList);
            Controls.Add(groupBox1);
            Controls.Add(txtFilename);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "frmOpenDialog";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Open Trace From Existing Source";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.TextBox txtFilename;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.RadioButton rdoDefaultLogfile;
    private System.Windows.Forms.RadioButton rdoImportADPlus;
    private System.Windows.Forms.RadioButton rdoImportTypeTxtFileWriter;
    private System.Windows.Forms.ListBox lbxMRUList;
    private System.Windows.Forms.Button btnBrowseForFile;
    private System.Windows.Forms.CheckBox chkAsynchImport;
    private System.Windows.Forms.TextBox txtLabelIdent;
    private System.Windows.Forms.Label label1;
      private System.Windows.Forms.RadioButton rdoDebugViewTex;
  }
}