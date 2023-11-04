namespace Plisky.FlimFlam { 
  partial class frmCreateDuplicate {
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCreateDuplicate));
        this.label1 = new System.Windows.Forms.Label();
        this.lblIdentifyAppRange = new System.Windows.Forms.Label();
        this.txtNewDupeName = new System.Windows.Forms.TextBox();
        this.btnYesOK = new System.Windows.Forms.Button();
        this.btnNoCancel = new System.Windows.Forms.Button();
        this.label2 = new System.Windows.Forms.Label();
        this.SuspendLayout();
        // 
        // label1
        // 
        this.label1.Location = new System.Drawing.Point(2, 9);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(526, 36);
        this.label1.TabIndex = 0;
        this.label1.Text = "You have selected a range in an application to be duplicated into a new applicati" +
            "on to view as a snapshot of this application.  Do you want to proceed?";
        // 
        // lblIdentifyAppRange
        // 
        this.lblIdentifyAppRange.Location = new System.Drawing.Point(2, 45);
        this.lblIdentifyAppRange.Name = "lblIdentifyAppRange";
        this.lblIdentifyAppRange.Size = new System.Drawing.Size(514, 38);
        this.lblIdentifyAppRange.TabIndex = 1;
        this.lblIdentifyAppRange.Text = "XXX";
        // 
        // txtNewDupeName
        // 
        this.txtNewDupeName.Location = new System.Drawing.Point(5, 122);
        this.txtNewDupeName.Name = "txtNewDupeName";
        this.txtNewDupeName.Size = new System.Drawing.Size(511, 20);
        this.txtNewDupeName.TabIndex = 2;
        // 
        // btnYesOK
        // 
        this.btnYesOK.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.btnYesOK.Location = new System.Drawing.Point(425, 148);
        this.btnYesOK.Name = "btnYesOK";
        this.btnYesOK.Size = new System.Drawing.Size(91, 23);
        this.btnYesOK.TabIndex = 3;
        this.btnYesOK.Text = "Yes, Proceed";
        this.btnYesOK.UseVisualStyleBackColor = true;
        // 
        // btnNoCancel
        // 
        this.btnNoCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.btnNoCancel.Location = new System.Drawing.Point(344, 148);
        this.btnNoCancel.Name = "btnNoCancel";
        this.btnNoCancel.Size = new System.Drawing.Size(75, 23);
        this.btnNoCancel.TabIndex = 4;
        this.btnNoCancel.Text = "No, Cancel";
        this.btnNoCancel.UseVisualStyleBackColor = true;
        // 
        // label2
        // 
        this.label2.AutoSize = true;
        this.label2.Location = new System.Drawing.Point(5, 103);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(231, 13);
        this.label2.TabIndex = 5;
        this.label2.Text = "Specify an identification name for this duplicate:";
        // 
        // frmCreateDuplicate
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(518, 189);
        this.Controls.Add(this.label2);
        this.Controls.Add(this.btnNoCancel);
        this.Controls.Add(this.btnYesOK);
        this.Controls.Add(this.txtNewDupeName);
        this.Controls.Add(this.lblIdentifyAppRange);
        this.Controls.Add(this.label1);
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.Name = "frmCreateDuplicate";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Duplicate Range?";
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label lblIdentifyAppRange;
    private System.Windows.Forms.TextBox txtNewDupeName;
    private System.Windows.Forms.Button btnYesOK;
    private System.Windows.Forms.Button btnNoCancel;
    private System.Windows.Forms.Label label2;
  }
}