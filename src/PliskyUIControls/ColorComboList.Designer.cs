namespace Plisky.UIWinforms.Controls {
    partial class ColorComboList {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.cboColorList = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cboColorList
            // 
            this.cboColorList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboColorList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboColorList.FormattingEnabled = true;
            this.cboColorList.Location = new System.Drawing.Point(0, 1);
            this.cboColorList.Name = "cboColorList";
            this.cboColorList.Size = new System.Drawing.Size(156, 21);
            this.cboColorList.TabIndex = 0;
            this.cboColorList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cboColorList_DrawItem);
            this.cboColorList.SelectedIndexChanged += new System.EventHandler(this.cboColorList_SelectedIndexChanged);
            // 
            // ColorComboList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cboColorList);
            this.Name = "ColorComboList";
            this.Size = new System.Drawing.Size(156, 24);
            this.FontChanged += new System.EventHandler(this.ColorComboList_FontChanged);
            this.SizeChanged += new System.EventHandler(this.ColorComboList_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cboColorList;
    }
}
