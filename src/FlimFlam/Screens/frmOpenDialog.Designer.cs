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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOpenDialog));
        this.btnOk = new System.Windows.Forms.Button();
        this.btnCancel = new System.Windows.Forms.Button();
        this.txtFilename = new System.Windows.Forms.TextBox();
        this.groupBox1 = new System.Windows.Forms.GroupBox();
        this.rdoDebugViewTex = new System.Windows.Forms.RadioButton();
        this.rdoDefaultLogfile = new System.Windows.Forms.RadioButton();
        this.rdoImportADPlus = new System.Windows.Forms.RadioButton();
        this.rdoImportTypeTxtFileWriter = new System.Windows.Forms.RadioButton();
        this.lbxMRUList = new System.Windows.Forms.ListBox();
        this.btnBrowseForFile = new System.Windows.Forms.Button();
        this.chkAsynchImport = new System.Windows.Forms.CheckBox();
        this.txtLabelIdent = new System.Windows.Forms.TextBox();
        this.label1 = new System.Windows.Forms.Label();
        this.groupBox1.SuspendLayout();
        this.SuspendLayout();
        // 
        // btnOk
        // 
        this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.btnOk.Location = new System.Drawing.Point(585, 240);
        this.btnOk.Name = "btnOk";
        this.btnOk.Size = new System.Drawing.Size(75, 23);
        this.btnOk.TabIndex = 0;
        this.btnOk.Text = "OK";
        this.btnOk.UseVisualStyleBackColor = true;
        // 
        // btnCancel
        // 
        this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.btnCancel.Location = new System.Drawing.Point(504, 240);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(75, 23);
        this.btnCancel.TabIndex = 1;
        this.btnCancel.Text = "Cancel";
        this.btnCancel.UseVisualStyleBackColor = true;
        // 
        // txtFilename
        // 
        this.txtFilename.Location = new System.Drawing.Point(3, 1);
        this.txtFilename.Name = "txtFilename";
        this.txtFilename.Size = new System.Drawing.Size(612, 20);
        this.txtFilename.TabIndex = 2;
        this.txtFilename.TextChanged += new System.EventHandler(this.txtFilename_TextChanged);
        // 
        // groupBox1
        // 
        this.groupBox1.Controls.Add(this.rdoDebugViewTex);
        this.groupBox1.Controls.Add(this.rdoDefaultLogfile);
        this.groupBox1.Controls.Add(this.rdoImportADPlus);
        this.groupBox1.Controls.Add(this.rdoImportTypeTxtFileWriter);
        this.groupBox1.Location = new System.Drawing.Point(3, 27);
        this.groupBox1.Name = "groupBox1";
        this.groupBox1.Size = new System.Drawing.Size(332, 103);
        this.groupBox1.TabIndex = 3;
        this.groupBox1.TabStop = false;
        this.groupBox1.Text = "Import file type:";
        // 
        // rdoDebugViewTex
        // 
        this.rdoDebugViewTex.AutoSize = true;
        this.rdoDebugViewTex.Location = new System.Drawing.Point(178, 19);
        this.rdoDebugViewTex.Name = "rdoDebugViewTex";
        this.rdoDebugViewTex.Size = new System.Drawing.Size(148, 17);
        this.rdoDebugViewTex.TabIndex = 3;
        this.rdoDebugViewTex.Text = "Tex Log From DebugView";
        this.rdoDebugViewTex.UseVisualStyleBackColor = true;
        // 
        // rdoDefaultLogfile
        // 
        this.rdoDefaultLogfile.AutoSize = true;
        this.rdoDefaultLogfile.Location = new System.Drawing.Point(6, 65);
        this.rdoDefaultLogfile.Name = "rdoDefaultLogfile";
        this.rdoDefaultLogfile.Size = new System.Drawing.Size(88, 17);
        this.rdoDefaultLogfile.TabIndex = 2;
        this.rdoDefaultLogfile.Text = "Just A Logfile";
        this.rdoDefaultLogfile.UseVisualStyleBackColor = true;
        // 
        // rdoImportADPlus
        // 
        this.rdoImportADPlus.AutoSize = true;
        this.rdoImportADPlus.Location = new System.Drawing.Point(6, 42);
        this.rdoImportADPlus.Name = "rdoImportADPlus";
        this.rdoImportADPlus.Size = new System.Drawing.Size(142, 17);
        this.rdoImportADPlus.TabIndex = 1;
        this.rdoImportADPlus.Text = "Log Where CRLF Is Lost";
        this.rdoImportADPlus.UseVisualStyleBackColor = true;
        // 
        // rdoImportTypeTxtFileWriter
        // 
        this.rdoImportTypeTxtFileWriter.AutoSize = true;
        this.rdoImportTypeTxtFileWriter.Checked = true;
        this.rdoImportTypeTxtFileWriter.Location = new System.Drawing.Point(6, 19);
        this.rdoImportTypeTxtFileWriter.Name = "rdoImportTypeTxtFileWriter";
        this.rdoImportTypeTxtFileWriter.Size = new System.Drawing.Size(134, 17);
        this.rdoImportTypeTxtFileWriter.TabIndex = 0;
        this.rdoImportTypeTxtFileWriter.TabStop = true;
        this.rdoImportTypeTxtFileWriter.Text = "TextFileWriter from Tex";
        this.rdoImportTypeTxtFileWriter.UseVisualStyleBackColor = true;
        // 
        // lbxMRUList
        // 
        this.lbxMRUList.FormattingEnabled = true;
        this.lbxMRUList.Location = new System.Drawing.Point(3, 136);
        this.lbxMRUList.Name = "lbxMRUList";
        this.lbxMRUList.Size = new System.Drawing.Size(657, 95);
        this.lbxMRUList.TabIndex = 4;
        this.lbxMRUList.SelectedIndexChanged += new System.EventHandler(this.lbxMRUList_SelectedIndexChanged);
        // 
        // btnBrowseForFile
        // 
        this.btnBrowseForFile.Location = new System.Drawing.Point(623, 1);
        this.btnBrowseForFile.Name = "btnBrowseForFile";
        this.btnBrowseForFile.Size = new System.Drawing.Size(37, 20);
        this.btnBrowseForFile.TabIndex = 5;
        this.btnBrowseForFile.Text = "...";
        this.btnBrowseForFile.UseVisualStyleBackColor = true;
        this.btnBrowseForFile.Click += new System.EventHandler(this.btnBrowseForFile_Click);
        // 
        // chkAsynchImport
        // 
        this.chkAsynchImport.AutoSize = true;
        this.chkAsynchImport.Checked = true;
        this.chkAsynchImport.CheckState = System.Windows.Forms.CheckState.Checked;
        this.chkAsynchImport.Location = new System.Drawing.Point(12, 247);
        this.chkAsynchImport.Name = "chkAsynchImport";
        this.chkAsynchImport.Size = new System.Drawing.Size(134, 17);
        this.chkAsynchImport.TabIndex = 6;
        this.chkAsynchImport.Text = "Do background import.";
        this.chkAsynchImport.UseVisualStyleBackColor = true;
        // 
        // txtLabelIdent
        // 
        this.txtLabelIdent.BackColor = System.Drawing.SystemColors.Info;
        this.txtLabelIdent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.txtLabelIdent.Location = new System.Drawing.Point(472, 27);
        this.txtLabelIdent.Name = "txtLabelIdent";
        this.txtLabelIdent.Size = new System.Drawing.Size(188, 20);
        this.txtLabelIdent.TabIndex = 7;
        this.txtLabelIdent.TextChanged += new System.EventHandler(this.txtLabelIdent_TextChanged);
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(375, 29);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(91, 13);
        this.label1.TabIndex = 8;
        this.label1.Text = "Label This Import:";
        // 
        // frmOpenDialog
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(672, 276);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.txtLabelIdent);
        this.Controls.Add(this.chkAsynchImport);
        this.Controls.Add(this.btnBrowseForFile);
        this.Controls.Add(this.lbxMRUList);
        this.Controls.Add(this.groupBox1);
        this.Controls.Add(this.txtFilename);
        this.Controls.Add(this.btnCancel);
        this.Controls.Add(this.btnOk);
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.Name = "frmOpenDialog";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Open Trace From Existing Source";
        this.groupBox1.ResumeLayout(false);
        this.groupBox1.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();

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