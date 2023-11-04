namespace Plisky.FlimFlam { 
  partial class frmViewInAWindow {
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmViewInAWindow));
        this.panel1 = new System.Windows.Forms.Panel();
        this.panel2 = new System.Windows.Forms.Panel();
        this.lvwExtractedViewList = new System.Windows.Forms.ListView();
        this.clmnGlobalIndex = new System.Windows.Forms.ColumnHeader();
        this.clmnMachinePid = new System.Windows.Forms.ColumnHeader();
        this.clmnThreadID = new System.Windows.Forms.ColumnHeader();
        this.clmnLocationDetails = new System.Windows.Forms.ColumnHeader();
        this.clmnDebugData = new System.Windows.Forms.ColumnHeader();
        this.pnlControls = new System.Windows.Forms.Panel();
        this.btnCopy = new System.Windows.Forms.Button();
        this.btnClose = new System.Windows.Forms.Button();
        this.panel2.SuspendLayout();
        this.pnlControls.SuspendLayout();
        this.SuspendLayout();
        // 
        // panel1
        // 
        this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
        this.panel1.Location = new System.Drawing.Point(0, 0);
        this.panel1.Name = "panel1";
        this.panel1.Size = new System.Drawing.Size(701, 24);
        this.panel1.TabIndex = 0;
        // 
        // panel2
        // 
        this.panel2.Controls.Add(this.lvwExtractedViewList);
        this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panel2.Location = new System.Drawing.Point(0, 24);
        this.panel2.Name = "panel2";
        this.panel2.Size = new System.Drawing.Size(701, 455);
        this.panel2.TabIndex = 1;
        // 
        // lvwExtractedViewList
        // 
        this.lvwExtractedViewList.AllowColumnReorder = true;
        this.lvwExtractedViewList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.lvwExtractedViewList.CausesValidation = false;
        this.lvwExtractedViewList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmnGlobalIndex,
            this.clmnMachinePid,
            this.clmnThreadID,
            this.clmnLocationDetails,
            this.clmnDebugData});
        this.lvwExtractedViewList.Dock = System.Windows.Forms.DockStyle.Fill;
        this.lvwExtractedViewList.FullRowSelect = true;
        this.lvwExtractedViewList.GridLines = true;
        this.lvwExtractedViewList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
        this.lvwExtractedViewList.Location = new System.Drawing.Point(0, 0);
        this.lvwExtractedViewList.Name = "lvwExtractedViewList";
        this.lvwExtractedViewList.Size = new System.Drawing.Size(701, 455);
        this.lvwExtractedViewList.TabIndex = 2;
        this.lvwExtractedViewList.UseCompatibleStateImageBehavior = false;
        this.lvwExtractedViewList.View = System.Windows.Forms.View.Details;
        // 
        // clmnGlobalIndex
        // 
        this.clmnGlobalIndex.Text = "Global Index";
        this.clmnGlobalIndex.Width = 79;
        // 
        // clmnMachinePid
        // 
        this.clmnMachinePid.Text = "Machine\\Pid";
        this.clmnMachinePid.Width = 99;
        // 
        // clmnThreadID
        // 
        this.clmnThreadID.Text = "Thread";
        this.clmnThreadID.Width = 83;
        // 
        // clmnLocationDetails
        // 
        this.clmnLocationDetails.Text = "LocationData";
        this.clmnLocationDetails.Width = 173;
        // 
        // clmnDebugData
        // 
        this.clmnDebugData.Text = "Debug Text";
        this.clmnDebugData.Width = 254;
        // 
        // pnlControls
        // 
        this.pnlControls.BackColor = System.Drawing.Color.Azure;
        this.pnlControls.Controls.Add(this.btnCopy);
        this.pnlControls.Controls.Add(this.btnClose);
        this.pnlControls.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.pnlControls.Location = new System.Drawing.Point(0, 479);
        this.pnlControls.Name = "pnlControls";
        this.pnlControls.Size = new System.Drawing.Size(701, 33);
        this.pnlControls.TabIndex = 2;
        // 
        // btnCopy
        // 
        this.btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnCopy.BackColor = System.Drawing.Color.PowderBlue;
        this.btnCopy.Enabled = false;
        this.btnCopy.Location = new System.Drawing.Point(495, 6);
        this.btnCopy.Name = "btnCopy";
        this.btnCopy.Size = new System.Drawing.Size(107, 23);
        this.btnCopy.TabIndex = 1;
        this.btnCopy.Text = "Copy To Clipboard";
        this.btnCopy.UseVisualStyleBackColor = false;
        this.btnCopy.Visible = false;
        // 
        // btnClose
        // 
        this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnClose.BackColor = System.Drawing.Color.PowderBlue;
        this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.btnClose.Location = new System.Drawing.Point(608, 6);
        this.btnClose.Name = "btnClose";
        this.btnClose.Size = new System.Drawing.Size(90, 23);
        this.btnClose.TabIndex = 0;
        this.btnClose.Text = "Close";
        this.btnClose.UseVisualStyleBackColor = false;
        this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
        // 
        // frmViewInAWindow
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.btnClose;
        this.ClientSize = new System.Drawing.Size(701, 512);
        this.Controls.Add(this.panel2);
        this.Controls.Add(this.panel1);
        this.Controls.Add(this.pnlControls);
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.Name = "frmViewInAWindow";
        this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
        this.Text = "Promoted View";
        this.panel2.ResumeLayout(false);
        this.pnlControls.ResumeLayout(false);
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Panel pnlControls;
    
    
    private System.Windows.Forms.ListView lvwExtractedViewList;
    private System.Windows.Forms.ColumnHeader clmnGlobalIndex;
    private System.Windows.Forms.ColumnHeader clmnMachinePid;
    private System.Windows.Forms.ColumnHeader clmnThreadID;
    private System.Windows.Forms.ColumnHeader clmnLocationDetails;
    private System.Windows.Forms.ColumnHeader clmnDebugData;
    private System.Windows.Forms.Button btnCopy;
    private System.Windows.Forms.Button btnClose;
  }
}