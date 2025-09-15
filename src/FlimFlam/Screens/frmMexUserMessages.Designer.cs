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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMexUserMessages));
            btnCloseMessages = new System.Windows.Forms.Button();
            lbxUserMessages = new System.Windows.Forms.ListBox();
            txtMostRecentMessage = new System.Windows.Forms.TextBox();
            panel1 = new System.Windows.Forms.Panel();
            label2 = new System.Windows.Forms.Label();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            lblMexVersionInformation = new System.Windows.Forms.Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // btnCloseMessages
            // 
            btnCloseMessages.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCloseMessages.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCloseMessages.Location = new System.Drawing.Point(441, 345);
            btnCloseMessages.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnCloseMessages.Name = "btnCloseMessages";
            btnCloseMessages.Size = new System.Drawing.Size(88, 27);
            btnCloseMessages.TabIndex = 0;
            btnCloseMessages.Text = "Close";
            btnCloseMessages.UseVisualStyleBackColor = true;
            // 
            // lbxUserMessages
            // 
            lbxUserMessages.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lbxUserMessages.FormattingEnabled = true;
            lbxUserMessages.ItemHeight = 15;
            lbxUserMessages.Location = new System.Drawing.Point(2, 235);
            lbxUserMessages.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            lbxUserMessages.Name = "lbxUserMessages";
            lbxUserMessages.Size = new System.Drawing.Size(526, 94);
            lbxUserMessages.TabIndex = 1;
            // 
            // txtMostRecentMessage
            // 
            txtMostRecentMessage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtMostRecentMessage.Location = new System.Drawing.Point(2, 120);
            txtMostRecentMessage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtMostRecentMessage.Multiline = true;
            txtMostRecentMessage.Name = "txtMostRecentMessage";
            txtMostRecentMessage.Size = new System.Drawing.Size(526, 108);
            txtMostRecentMessage.TabIndex = 2;
            // 
            // panel1
            // 
            panel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            panel1.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(lblMexVersionInformation);
            panel1.Location = new System.Drawing.Point(2, 3);
            panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(526, 110);
            panel1.TabIndex = 6;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label2.Location = new System.Drawing.Point(84, 77);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(320, 16);
            label2.TabIndex = 9;
            label2.Text = "Mex viewer, displays trace output from//Bilge.";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (System.Drawing.Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.InitialImage = (System.Drawing.Image)resources.GetObject("pictureBox1.InitialImage");
            pictureBox1.Location = new System.Drawing.Point(12, 10);
            pictureBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(40, 20);
            pictureBox1.TabIndex = 8;
            pictureBox1.TabStop = false;
            // 
            // lblMexVersionInformation
            // 
            lblMexVersionInformation.AutoSize = true;
            lblMexVersionInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblMexVersionInformation.Location = new System.Drawing.Point(84, 10);
            lblMexVersionInformation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblMexVersionInformation.Name = "lblMexVersionInformation";
            lblMexVersionInformation.Size = new System.Drawing.Size(144, 16);
            lblMexVersionInformation.TabIndex = 6;
            lblMexVersionInformation.Text = "Mex Version X.X.X.X";
            // 
            // frmMexUserMessages
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = btnCloseMessages;
            ClientSize = new System.Drawing.Size(532, 385);
            Controls.Add(panel1);
            Controls.Add(txtMostRecentMessage);
            Controls.Add(lbxUserMessages);
            Controls.Add(btnCloseMessages);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MinimumSize = new System.Drawing.Size(539, 411);
            Name = "frmMexUserMessages";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "About Mex";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCloseMessages;
        private System.Windows.Forms.ListBox lbxUserMessages;
        private System.Windows.Forms.TextBox txtMostRecentMessage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblMexVersionInformation;
        private System.Windows.Forms.Label label2;
    }
}