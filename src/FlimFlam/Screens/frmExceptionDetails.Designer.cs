namespace Plisky.FlimFlam { 
  partial class frmExceptionDetails {
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
        this.lbxExceptionBackTrace = new System.Windows.Forms.ListBox();
        this.btnShowBackTraceForThread = new System.Windows.Forms.Button();
        this.txtExceptionLocationFilename = new System.Windows.Forms.TextBox();
        this.txtExceptionLocationLineNo = new System.Windows.Forms.TextBox();
        this.txtExceptionSummary = new System.Windows.Forms.TextBox();
        this.label1 = new System.Windows.Forms.Label();
        this.label3 = new System.Windows.Forms.Label();
        this.label4 = new System.Windows.Forms.Label();
        this.label5 = new System.Windows.Forms.Label();
        this.btnShowAllTraceUpToException = new System.Windows.Forms.Button();
        this.btnCloseForm = new System.Windows.Forms.Button();
        this.label2 = new System.Windows.Forms.Label();
        this.label6 = new System.Windows.Forms.Label();
        this.splitContainer1 = new System.Windows.Forms.SplitContainer();
        this.lbxExceptionHeirachy = new System.Windows.Forms.ListBox();
        this.txtSelectedExceptionDetails = new System.Windows.Forms.TextBox();
        this.splitContainer1.Panel1.SuspendLayout();
        this.splitContainer1.Panel2.SuspendLayout();
        this.splitContainer1.SuspendLayout();
        this.SuspendLayout();
        // 
        // lbxExceptionBackTrace
        // 
        this.lbxExceptionBackTrace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.lbxExceptionBackTrace.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.lbxExceptionBackTrace.Font = new System.Drawing.Font("ProFontWindows", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lbxExceptionBackTrace.FormattingEnabled = true;
        this.lbxExceptionBackTrace.ItemHeight = 12;
        this.lbxExceptionBackTrace.Location = new System.Drawing.Point(12, 428);
        this.lbxExceptionBackTrace.Name = "lbxExceptionBackTrace";
        this.lbxExceptionBackTrace.Size = new System.Drawing.Size(684, 122);
        this.lbxExceptionBackTrace.TabIndex = 1;
        // 
        // btnShowBackTraceForThread
        // 
        this.btnShowBackTraceForThread.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnShowBackTraceForThread.BackColor = System.Drawing.Color.RosyBrown;
        this.btnShowBackTraceForThread.Location = new System.Drawing.Point(409, 395);
        this.btnShowBackTraceForThread.Name = "btnShowBackTraceForThread";
        this.btnShowBackTraceForThread.Size = new System.Drawing.Size(169, 23);
        this.btnShowBackTraceForThread.TabIndex = 3;
        this.btnShowBackTraceForThread.Text = "Show BackTrace (SameThread):";
        this.btnShowBackTraceForThread.UseVisualStyleBackColor = false;
        this.btnShowBackTraceForThread.Click += new System.EventHandler(this.btnShowBackTrace_Click);
        // 
        // txtExceptionLocationFilename
        // 
        this.txtExceptionLocationFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.txtExceptionLocationFilename.BackColor = System.Drawing.Color.Lavender;
        this.txtExceptionLocationFilename.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.txtExceptionLocationFilename.Font = new System.Drawing.Font("ProFontWindows", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.txtExceptionLocationFilename.Location = new System.Drawing.Point(73, 136);
        this.txtExceptionLocationFilename.Name = "txtExceptionLocationFilename";
        this.txtExceptionLocationFilename.Size = new System.Drawing.Size(529, 19);
        this.txtExceptionLocationFilename.TabIndex = 6;
        // 
        // txtExceptionLocationLineNo
        // 
        this.txtExceptionLocationLineNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.txtExceptionLocationLineNo.BackColor = System.Drawing.Color.Lavender;
        this.txtExceptionLocationLineNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.txtExceptionLocationLineNo.Font = new System.Drawing.Font("ProFontWindows", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.txtExceptionLocationLineNo.Location = new System.Drawing.Point(622, 136);
        this.txtExceptionLocationLineNo.Name = "txtExceptionLocationLineNo";
        this.txtExceptionLocationLineNo.Size = new System.Drawing.Size(63, 19);
        this.txtExceptionLocationLineNo.TabIndex = 7;
        // 
        // txtExceptionSummary
        // 
        this.txtExceptionSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.txtExceptionSummary.BackColor = System.Drawing.Color.Lavender;
        this.txtExceptionSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.txtExceptionSummary.Font = new System.Drawing.Font("ProFontWindows", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.txtExceptionSummary.Location = new System.Drawing.Point(12, 25);
        this.txtExceptionSummary.Multiline = true;
        this.txtExceptionSummary.Name = "txtExceptionSummary";
        this.txtExceptionSummary.Size = new System.Drawing.Size(674, 108);
        this.txtExceptionSummary.TabIndex = 8;
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label1.Location = new System.Drawing.Point(0, 5);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(465, 13);
        this.label1.TabIndex = 9;
        this.label1.Text = "An exception was encountered.  The message associated with the exception was:";
        // 
        // label3
        // 
        this.label3.AutoSize = true;
        this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label3.Location = new System.Drawing.Point(9, 136);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(58, 13);
        this.label3.TabIndex = 11;
        this.label3.Text = "Location:";
        // 
        // label4
        // 
        this.label4.AutoSize = true;
        this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label4.Location = new System.Drawing.Point(9, 167);
        this.label4.Name = "label4";
        this.label4.Size = new System.Drawing.Size(60, 13);
        this.label4.TabIndex = 12;
        this.label4.Text = "Heirachy:";
        // 
        // label5
        // 
        this.label5.AutoSize = true;
        this.label5.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label5.Location = new System.Drawing.Point(9, 256);
        this.label5.Name = "label5";
        this.label5.Size = new System.Drawing.Size(49, 13);
        this.label5.TabIndex = 13;
        this.label5.Text = "Details.";
        // 
        // btnShowAllTraceUpToException
        // 
        this.btnShowAllTraceUpToException.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnShowAllTraceUpToException.BackColor = System.Drawing.Color.RosyBrown;
        this.btnShowAllTraceUpToException.Location = new System.Drawing.Point(225, 395);
        this.btnShowAllTraceUpToException.Name = "btnShowAllTraceUpToException";
        this.btnShowAllTraceUpToException.Size = new System.Drawing.Size(166, 23);
        this.btnShowAllTraceUpToException.TabIndex = 14;
        this.btnShowAllTraceUpToException.Text = "Show BackTrace (All Threads):";
        this.btnShowAllTraceUpToException.UseVisualStyleBackColor = false;
        this.btnShowAllTraceUpToException.Click += new System.EventHandler(this.btnShowAllTraceUpToException_Click);
        // 
        // btnCloseForm
        // 
        this.btnCloseForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnCloseForm.BackColor = System.Drawing.Color.RosyBrown;
        this.btnCloseForm.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.btnCloseForm.Location = new System.Drawing.Point(584, 395);
        this.btnCloseForm.Name = "btnCloseForm";
        this.btnCloseForm.Size = new System.Drawing.Size(112, 23);
        this.btnCloseForm.TabIndex = 16;
        this.btnCloseForm.Text = "Cl&ose";
        this.btnCloseForm.UseVisualStyleBackColor = false;
        // 
        // label2
        // 
        this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.label2.AutoSize = true;
        this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label2.Location = new System.Drawing.Point(603, 140);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(17, 13);
        this.label2.TabIndex = 17;
        this.label2.Text = "@";
        // 
        // label6
        // 
        this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.label6.AutoSize = true;
        this.label6.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label6.Location = new System.Drawing.Point(9, 400);
        this.label6.Name = "label6";
        this.label6.Size = new System.Drawing.Size(194, 13);
        this.label6.TabIndex = 18;
        this.label6.Text = "Last traces before the exception:";
        // 
        // splitContainer1
        // 
        this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.splitContainer1.Location = new System.Drawing.Point(75, 167);
        this.splitContainer1.Name = "splitContainer1";
        this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
        // 
        // splitContainer1.Panel1
        // 
        this.splitContainer1.Panel1.Controls.Add(this.lbxExceptionHeirachy);
        // 
        // splitContainer1.Panel2
        // 
        this.splitContainer1.Panel2.Controls.Add(this.txtSelectedExceptionDetails);
        this.splitContainer1.Size = new System.Drawing.Size(610, 214);
        this.splitContainer1.SplitterDistance = 92;
        this.splitContainer1.TabIndex = 19;
        // 
        // lbxExceptionHeirachy
        // 
        this.lbxExceptionHeirachy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.lbxExceptionHeirachy.Dock = System.Windows.Forms.DockStyle.Fill;
        this.lbxExceptionHeirachy.Font = new System.Drawing.Font("ProFontWindows", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lbxExceptionHeirachy.FormattingEnabled = true;
        this.lbxExceptionHeirachy.ItemHeight = 12;
        this.lbxExceptionHeirachy.Location = new System.Drawing.Point(0, 0);
        this.lbxExceptionHeirachy.MinimumSize = new System.Drawing.Size(2, 86);
        this.lbxExceptionHeirachy.Name = "lbxExceptionHeirachy";
        this.lbxExceptionHeirachy.Size = new System.Drawing.Size(610, 86);
        this.lbxExceptionHeirachy.TabIndex = 5;
        this.lbxExceptionHeirachy.SelectedIndexChanged += new System.EventHandler(this.lbxExceptionHeirachy_SelectedIndexChanged);
        // 
        // txtSelectedExceptionDetails
        // 
        this.txtSelectedExceptionDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.txtSelectedExceptionDetails.Dock = System.Windows.Forms.DockStyle.Fill;
        this.txtSelectedExceptionDetails.Font = new System.Drawing.Font("ProFontWindows", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.txtSelectedExceptionDetails.Location = new System.Drawing.Point(0, 0);
        this.txtSelectedExceptionDetails.MinimumSize = new System.Drawing.Size(2, 106);
        this.txtSelectedExceptionDetails.Multiline = true;
        this.txtSelectedExceptionDetails.Name = "txtSelectedExceptionDetails";
        this.txtSelectedExceptionDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.txtSelectedExceptionDetails.Size = new System.Drawing.Size(610, 118);
        this.txtSelectedExceptionDetails.TabIndex = 6;
        // 
        // frmExceptionDetails
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.Color.MistyRose;
        this.ClientSize = new System.Drawing.Size(698, 562);
        this.Controls.Add(this.splitContainer1);
        this.Controls.Add(this.label6);
        this.Controls.Add(this.label2);
        this.Controls.Add(this.btnCloseForm);
        this.Controls.Add(this.btnShowAllTraceUpToException);
        this.Controls.Add(this.label5);
        this.Controls.Add(this.label4);
        this.Controls.Add(this.label3);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.txtExceptionSummary);
        this.Controls.Add(this.txtExceptionLocationLineNo);
        this.Controls.Add(this.txtExceptionLocationFilename);
        this.Controls.Add(this.btnShowBackTraceForThread);
        this.Controls.Add(this.lbxExceptionBackTrace);
        this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
        this.MinimumSize = new System.Drawing.Size(706, 586);
        this.Name = "frmExceptionDetails";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "frmExceptionDetails";
        this.splitContainer1.Panel1.ResumeLayout(false);
        this.splitContainer1.Panel2.ResumeLayout(false);
        this.splitContainer1.Panel2.PerformLayout();
        this.splitContainer1.ResumeLayout(false);
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnShowBackTraceForThread;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
      internal System.Windows.Forms.ListBox lbxExceptionBackTrace;
    internal System.Windows.Forms.TextBox txtExceptionLocationFilename;
    internal System.Windows.Forms.TextBox txtExceptionLocationLineNo;
    internal System.Windows.Forms.TextBox txtExceptionSummary;
    private System.Windows.Forms.Button btnShowAllTraceUpToException;
    private System.Windows.Forms.Button btnCloseForm;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label6;
      private System.Windows.Forms.SplitContainer splitContainer1;
      internal System.Windows.Forms.ListBox lbxExceptionHeirachy;
      internal System.Windows.Forms.TextBox txtSelectedExceptionDetails;
  }
}