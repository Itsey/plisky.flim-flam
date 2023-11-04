namespace Plisky.FlimFlam { 
    partial class frmMexUserMessages {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMexUserMessages));
            this.btnCloseMessages = new System.Windows.Forms.Button();
            this.lbxUserMessages = new System.Windows.Forms.ListBox();
            this.txtMostRecentMessage = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblMexVersionInformation = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCloseMessages
            // 
            this.btnCloseMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCloseMessages.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCloseMessages.Location = new System.Drawing.Point(378, 299);
            this.btnCloseMessages.Name = "btnCloseMessages";
            this.btnCloseMessages.Size = new System.Drawing.Size(75, 23);
            this.btnCloseMessages.TabIndex = 0;
            this.btnCloseMessages.Text = "Close";
            this.btnCloseMessages.UseVisualStyleBackColor = true;
            // 
            // lbxUserMessages
            // 
            this.lbxUserMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbxUserMessages.FormattingEnabled = true;
            this.lbxUserMessages.Location = new System.Drawing.Point(2, 204);
            this.lbxUserMessages.Name = "lbxUserMessages";
            this.lbxUserMessages.Size = new System.Drawing.Size(451, 82);
            this.lbxUserMessages.TabIndex = 1;
            // 
            // txtMostRecentMessage
            // 
            this.txtMostRecentMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMostRecentMessage.Location = new System.Drawing.Point(2, 104);
            this.txtMostRecentMessage.Multiline = true;
            this.txtMostRecentMessage.Name = "txtMostRecentMessage";
            this.txtMostRecentMessage.Size = new System.Drawing.Size(451, 94);
            this.txtMostRecentMessage.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lblMexVersionInformation);
            this.panel1.Location = new System.Drawing.Point(2, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(451, 95);
            this.panel1.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Minya Nouvelle", 9.749999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(72, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(289, 17);
            this.label2.TabIndex = 9;
            this.label2.Text = "Mex viewer, displays trace output from//Bilge.";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(10, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(34, 17);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Minya Nouvelle", 9.749999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(72, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(190, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "Compatible With Tex 1.X,2.X";
            // 
            // lblMexVersionInformation
            // 
            this.lblMexVersionInformation.AutoSize = true;
            this.lblMexVersionInformation.Font = new System.Drawing.Font("Minya Nouvelle", 9.749999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMexVersionInformation.Location = new System.Drawing.Point(72, 9);
            this.lblMexVersionInformation.Name = "lblMexVersionInformation";
            this.lblMexVersionInformation.Size = new System.Drawing.Size(144, 17);
            this.lblMexVersionInformation.TabIndex = 6;
            this.lblMexVersionInformation.Text = "Mex Version X.X.X.X";
            // 
            // frmMexUserMessages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCloseMessages;
            this.ClientSize = new System.Drawing.Size(456, 334);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtMostRecentMessage);
            this.Controls.Add(this.lbxUserMessages);
            this.Controls.Add(this.btnCloseMessages);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(464, 361);
            this.Name = "frmMexUserMessages";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About Mex";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCloseMessages;
        private System.Windows.Forms.ListBox lbxUserMessages;
        private System.Windows.Forms.TextBox txtMostRecentMessage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblMexVersionInformation;
        private System.Windows.Forms.Label label2;
    }
}