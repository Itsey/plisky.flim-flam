namespace OldFlimflam.Screens {
    partial class frmConnectToEndpoint {
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
            btnSave = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            tabActivePolls = new System.Windows.Forms.TabControl();
            tpListActivePolls = new System.Windows.Forms.TabPage();
            tpHttpPoll = new System.Windows.Forms.TabPage();
            btnAdd = new System.Windows.Forms.Button();
            chkPollForUpdates = new System.Windows.Forms.CheckBox();
            btnExecute = new System.Windows.Forms.Button();
            txtBody = new System.Windows.Forms.TextBox();
            txtUriBase = new System.Windows.Forms.TextBox();
            txtFullUri = new System.Windows.Forms.TextBox();
            btnQcGetTrace = new System.Windows.Forms.Button();
            btnQcSetTraceOn = new System.Windows.Forms.Button();
            lbxResults = new System.Windows.Forms.ListBox();
            label1 = new System.Windows.Forms.Label();
            txtPort = new System.Windows.Forms.TextBox();
            txtUri = new System.Windows.Forms.TextBox();
            tpGooglePs = new System.Windows.Forms.TabPage();
            lblAction = new System.Windows.Forms.Label();
            btnTest = new System.Windows.Forms.Button();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            txtSubscription = new System.Windows.Forms.TextBox();
            txtGoogleProjectId = new System.Windows.Forms.TextBox();
            tabActivePolls.SuspendLayout();
            tpHttpPoll.SuspendLayout();
            tpGooglePs.SuspendLayout();
            SuspendLayout();
            // 
            // btnSave
            // 
            btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnSave.Location = new System.Drawing.Point(1076, 365);
            btnSave.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(88, 27);
            btnSave.TabIndex = 1;
            btnSave.Text = "OK";
            btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(1076, 302);
            btnCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(88, 27);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(14, 31);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(42, 15);
            label2.TabIndex = 7;
            label2.Text = "http://";
            // 
            // tabActivePolls
            // 
            tabActivePolls.Controls.Add(tpListActivePolls);
            tabActivePolls.Controls.Add(tpHttpPoll);
            tabActivePolls.Controls.Add(tpGooglePs);
            tabActivePolls.Location = new System.Drawing.Point(14, 74);
            tabActivePolls.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabActivePolls.Name = "tabActivePolls";
            tabActivePolls.SelectedIndex = 0;
            tabActivePolls.Size = new System.Drawing.Size(1023, 318);
            tabActivePolls.TabIndex = 9;
            // 
            // tpListActivePolls
            // 
            tpListActivePolls.Location = new System.Drawing.Point(4, 24);
            tpListActivePolls.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tpListActivePolls.Name = "tpListActivePolls";
            tpListActivePolls.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tpListActivePolls.Size = new System.Drawing.Size(1015, 290);
            tpListActivePolls.TabIndex = 0;
            tpListActivePolls.Text = "Active Polls";
            tpListActivePolls.UseVisualStyleBackColor = true;
            // 
            // tpHttpPoll
            // 
            tpHttpPoll.Controls.Add(btnAdd);
            tpHttpPoll.Controls.Add(chkPollForUpdates);
            tpHttpPoll.Controls.Add(btnExecute);
            tpHttpPoll.Controls.Add(txtBody);
            tpHttpPoll.Controls.Add(txtUriBase);
            tpHttpPoll.Controls.Add(txtFullUri);
            tpHttpPoll.Controls.Add(btnQcGetTrace);
            tpHttpPoll.Controls.Add(btnQcSetTraceOn);
            tpHttpPoll.Controls.Add(lbxResults);
            tpHttpPoll.Controls.Add(label1);
            tpHttpPoll.Controls.Add(txtPort);
            tpHttpPoll.Controls.Add(txtUri);
            tpHttpPoll.Location = new System.Drawing.Point(4, 24);
            tpHttpPoll.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tpHttpPoll.Name = "tpHttpPoll";
            tpHttpPoll.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tpHttpPoll.Size = new System.Drawing.Size(1015, 290);
            tpHttpPoll.TabIndex = 1;
            tpHttpPoll.Text = "Http";
            tpHttpPoll.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            btnAdd.Location = new System.Drawing.Point(922, 247);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new System.Drawing.Size(75, 23);
            btnAdd.TabIndex = 17;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += BtnAdd_Click;
            // 
            // chkPollForUpdates
            // 
            chkPollForUpdates.AutoSize = true;
            chkPollForUpdates.Location = new System.Drawing.Point(694, 28);
            chkPollForUpdates.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            chkPollForUpdates.Name = "chkPollForUpdates";
            chkPollForUpdates.Size = new System.Drawing.Size(46, 19);
            chkPollForUpdates.TabIndex = 16;
            chkPollForUpdates.Text = "Poll";
            chkPollForUpdates.UseVisualStyleBackColor = true;
            // 
            // btnExecute
            // 
            btnExecute.Location = new System.Drawing.Point(694, 103);
            btnExecute.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnExecute.Name = "btnExecute";
            btnExecute.Size = new System.Drawing.Size(88, 27);
            btnExecute.TabIndex = 15;
            btnExecute.Text = "Execute";
            btnExecute.UseVisualStyleBackColor = true;
            // 
            // txtBody
            // 
            txtBody.Location = new System.Drawing.Point(71, 119);
            txtBody.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtBody.Name = "txtBody";
            txtBody.Size = new System.Drawing.Size(615, 23);
            txtBody.TabIndex = 12;
            // 
            // txtUriBase
            // 
            txtUriBase.Location = new System.Drawing.Point(75, 89);
            txtUriBase.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtUriBase.Name = "txtUriBase";
            txtUriBase.Size = new System.Drawing.Size(240, 23);
            txtUriBase.TabIndex = 13;
            // 
            // txtFullUri
            // 
            txtFullUri.Location = new System.Drawing.Point(322, 89);
            txtFullUri.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtFullUri.Name = "txtFullUri";
            txtFullUri.Size = new System.Drawing.Size(364, 23);
            txtFullUri.TabIndex = 14;
            // 
            // btnQcGetTrace
            // 
            btnQcGetTrace.Location = new System.Drawing.Point(167, 53);
            btnQcGetTrace.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnQcGetTrace.Name = "btnQcGetTrace";
            btnQcGetTrace.Size = new System.Drawing.Size(88, 27);
            btnQcGetTrace.TabIndex = 10;
            btnQcGetTrace.Text = "Query";
            btnQcGetTrace.UseVisualStyleBackColor = true;
            // 
            // btnQcSetTraceOn
            // 
            btnQcSetTraceOn.Location = new System.Drawing.Point(71, 53);
            btnQcSetTraceOn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnQcSetTraceOn.Name = "btnQcSetTraceOn";
            btnQcSetTraceOn.Size = new System.Drawing.Size(88, 27);
            btnQcSetTraceOn.TabIndex = 11;
            btnQcSetTraceOn.Text = "On";
            btnQcSetTraceOn.UseVisualStyleBackColor = true;
            // 
            // lbxResults
            // 
            lbxResults.FormattingEnabled = true;
            lbxResults.ItemHeight = 15;
            lbxResults.Location = new System.Drawing.Point(71, 146);
            lbxResults.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            lbxResults.Name = "lbxResults";
            lbxResults.Size = new System.Drawing.Size(615, 124);
            lbxResults.TabIndex = 9;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(330, 24);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(10, 15);
            label1.TabIndex = 8;
            label1.Text = ":";
            // 
            // txtPort
            // 
            txtPort.Location = new System.Drawing.Point(351, 24);
            txtPort.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtPort.Name = "txtPort";
            txtPort.Size = new System.Drawing.Size(70, 23);
            txtPort.TabIndex = 2;
            // 
            // txtUri
            // 
            txtUri.Location = new System.Drawing.Point(71, 24);
            txtUri.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtUri.Name = "txtUri";
            txtUri.Size = new System.Drawing.Size(251, 23);
            txtUri.TabIndex = 1;
            // 
            // tpGooglePs
            // 
            tpGooglePs.Controls.Add(lblAction);
            tpGooglePs.Controls.Add(btnTest);
            tpGooglePs.Controls.Add(label4);
            tpGooglePs.Controls.Add(label3);
            tpGooglePs.Controls.Add(txtSubscription);
            tpGooglePs.Controls.Add(txtGoogleProjectId);
            tpGooglePs.Location = new System.Drawing.Point(4, 24);
            tpGooglePs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tpGooglePs.Name = "tpGooglePs";
            tpGooglePs.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tpGooglePs.Size = new System.Drawing.Size(1015, 290);
            tpGooglePs.TabIndex = 2;
            tpGooglePs.Text = "Pub Sub";
            tpGooglePs.UseVisualStyleBackColor = true;
            // 
            // lblAction
            // 
            lblAction.AutoSize = true;
            lblAction.Location = new System.Drawing.Point(22, 252);
            lblAction.Name = "lblAction";
            lblAction.Size = new System.Drawing.Size(38, 15);
            lblAction.TabIndex = 6;
            lblAction.Text = "label5";
            // 
            // btnTest
            // 
            btnTest.Location = new System.Drawing.Point(901, 60);
            btnTest.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnTest.Name = "btnTest";
            btnTest.Size = new System.Drawing.Size(88, 27);
            btnTest.TabIndex = 5;
            btnTest.Text = "Connect";
            btnTest.UseVisualStyleBackColor = true;
            btnTest.Visible = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(262, 38);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(114, 15);
            label4.TabIndex = 4;
            label4.Text = "Google Subscription";
            label4.Visible = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(18, 38);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(85, 15);
            label3.TabIndex = 3;
            label3.Text = "Google Project";
            label3.Visible = false;
            // 
            // txtSubscription
            // 
            txtSubscription.Location = new System.Drawing.Point(266, 60);
            txtSubscription.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtSubscription.Name = "txtSubscription";
            txtSubscription.Size = new System.Drawing.Size(271, 23);
            txtSubscription.TabIndex = 1;
            txtSubscription.Visible = false;
            // 
            // txtGoogleProjectId
            // 
            txtGoogleProjectId.Location = new System.Drawing.Point(18, 60);
            txtGoogleProjectId.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtGoogleProjectId.Name = "txtGoogleProjectId";
            txtGoogleProjectId.Size = new System.Drawing.Size(221, 23);
            txtGoogleProjectId.TabIndex = 0;
            txtGoogleProjectId.Visible = false;
            // 
            // frmConnectToEndpoint
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1177, 431);
            Controls.Add(tabActivePolls);
            Controls.Add(label2);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "frmConnectToEndpoint";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Connect To Trace Endpoint";
            tabActivePolls.ResumeLayout(false);
            tpHttpPoll.ResumeLayout(false);
            tpHttpPoll.PerformLayout();
            tpGooglePs.ResumeLayout(false);
            tpGooglePs.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabActivePolls;
        private System.Windows.Forms.TabPage tpListActivePolls;
        private System.Windows.Forms.TabPage tpHttpPoll;
        private System.Windows.Forms.TabPage tpGooglePs;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSubscription;
        private System.Windows.Forms.TextBox txtGoogleProjectId;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnQcGetTrace;
        private System.Windows.Forms.Button btnQcSetTraceOn;
        private System.Windows.Forms.ListBox lbxResults;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtUri;
        private System.Windows.Forms.CheckBox chkPollForUpdates;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.TextBox txtBody;
        private System.Windows.Forms.TextBox txtUriBase;
        private System.Windows.Forms.TextBox txtFullUri;
        private System.Windows.Forms.Label lblAction;
        private System.Windows.Forms.Button btnAdd;
    }
}