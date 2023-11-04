//using Plisky.Plumbing.Legacy;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Plisky.FlimFlam { 

    /// <summary>
    /// Summary description for frmExtendedDetails.
    /// </summary>
    internal class frmExtendedDetails : System.Windows.Forms.Form {
        internal TextBox txtDebugEntry;
        internal TextBox txtEntryFurtherDetails;
        internal TextBox txtSecondaryMessage;
        private Button btnOk;
        private CheckBox chkAllowEdit;
        private IContainer components;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem fileExistsToolStripMenuItem;
        private ToolStripMenuItem filterSelectionToolStripMenuItem;
        private string m_quickFilterText;
        private ToolStripMenuItem openContainingFolderToolStripMenuItem;
        private Panel panel1;
        private Panel pnlLogView;
        private ToolStripMenuItem selectionIsGayMlToolStripMenuItem;
        private ToolStripMenuItem selectionIsPathToolStripMenuItem;
        private Splitter splitter1;
        private Splitter splitter2;
        private ToolStripMenuItem tsmiFilterExcludeSelection;
        private ToolStripMenuItem tsmiFilterIncludeSelection;

        internal frmExtendedDetails() {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            components = new Container();
            var resources = new ComponentResourceManager(typeof(frmExtendedDetails));
            panel1 = new Panel();
            chkAllowEdit = new CheckBox();
            btnOk = new Button();
            pnlLogView = new Panel();
            txtSecondaryMessage = new TextBox();
            contextMenuStrip1 = new ContextMenuStrip(components);
            selectionIsPathToolStripMenuItem = new ToolStripMenuItem();
            fileExistsToolStripMenuItem = new ToolStripMenuItem();
            openContainingFolderToolStripMenuItem = new ToolStripMenuItem();
            selectionIsGayMlToolStripMenuItem = new ToolStripMenuItem();
            filterSelectionToolStripMenuItem = new ToolStripMenuItem();
            tsmiFilterExcludeSelection = new ToolStripMenuItem();
            tsmiFilterIncludeSelection = new ToolStripMenuItem();
            splitter2 = new Splitter();
            splitter1 = new Splitter();
            txtEntryFurtherDetails = new TextBox();
            txtDebugEntry = new TextBox();
            panel1.SuspendLayout();
            pnlLogView.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            //
            // panel1
            //
            panel1.BackColor = System.Drawing.Color.MistyRose;
            panel1.Controls.Add(chkAllowEdit);
            panel1.Controls.Add(btnOk);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new System.Drawing.Point(0, 442);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(827, 41);
            panel1.TabIndex = 4;
            //
            // chkAllowEdit
            //
            chkAllowEdit.AutoSize = true;
            chkAllowEdit.Location = new System.Drawing.Point(14, 7);
            chkAllowEdit.Name = "chkAllowEdit";
            chkAllowEdit.Size = new System.Drawing.Size(96, 19);
            chkAllowEdit.TabIndex = 5;
            chkAllowEdit.Text = "Allow editing";
            chkAllowEdit.UseVisualStyleBackColor = true;
            chkAllowEdit.CheckedChanged += chkAllowEdit_CheckedChanged;
            //
            // btnOk
            //
            btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOk.BackColor = System.Drawing.Color.RosyBrown;
            btnOk.DialogResult = DialogResult.No;
            btnOk.Location = new System.Drawing.Point(688, 6);
            btnOk.Name = "btnOk";
            btnOk.Size = new System.Drawing.Size(124, 28);
            btnOk.TabIndex = 2;
            btnOk.Text = "Close";
            btnOk.UseVisualStyleBackColor = false;
            //
            // pnlLogView
            //
            pnlLogView.Controls.Add(txtSecondaryMessage);
            pnlLogView.Controls.Add(splitter2);
            pnlLogView.Controls.Add(splitter1);
            pnlLogView.Controls.Add(txtEntryFurtherDetails);
            pnlLogView.Controls.Add(txtDebugEntry);
            pnlLogView.Dock = DockStyle.Fill;
            pnlLogView.Location = new System.Drawing.Point(0, 0);
            pnlLogView.Name = "pnlLogView";
            pnlLogView.Size = new System.Drawing.Size(827, 442);
            pnlLogView.TabIndex = 7;
            //
            // txtSecondaryMessage
            //
            txtSecondaryMessage.BackColor = System.Drawing.SystemColors.Window;
            txtSecondaryMessage.ContextMenuStrip = contextMenuStrip1;
            txtSecondaryMessage.Dock = DockStyle.Fill;
            txtSecondaryMessage.Location = new System.Drawing.Point(0, 176);
            txtSecondaryMessage.MinimumSize = new System.Drawing.Size(0, 19);
            txtSecondaryMessage.Multiline = true;
            txtSecondaryMessage.Name = "txtSecondaryMessage";
            txtSecondaryMessage.ReadOnly = true;
            txtSecondaryMessage.ScrollBars = ScrollBars.Vertical;
            txtSecondaryMessage.Size = new System.Drawing.Size(827, 128);
            txtSecondaryMessage.TabIndex = 3;
            txtSecondaryMessage.Text = "AA";
            //
            // contextMenuStrip1
            //
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { selectionIsPathToolStripMenuItem, selectionIsGayMlToolStripMenuItem, filterSelectionToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(161, 70);
            contextMenuStrip1.Opening += contextMenuStrip1_Opening;
            //
            // selectionIsPathToolStripMenuItem
            //
            selectionIsPathToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { fileExistsToolStripMenuItem, openContainingFolderToolStripMenuItem });
            selectionIsPathToolStripMenuItem.Name = "selectionIsPathToolStripMenuItem";
            selectionIsPathToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            selectionIsPathToolStripMenuItem.Text = "Selection Is Path";
            //
            // fileExistsToolStripMenuItem
            //
            fileExistsToolStripMenuItem.Name = "fileExistsToolStripMenuItem";
            fileExistsToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            fileExistsToolStripMenuItem.Text = "File Exists:";
            fileExistsToolStripMenuItem.Click += fileExistsToolStripMenuItem_Click;
            //
            // openContainingFolderToolStripMenuItem
            //
            openContainingFolderToolStripMenuItem.Name = "openContainingFolderToolStripMenuItem";
            openContainingFolderToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            openContainingFolderToolStripMenuItem.Text = "Open Containing Folder:";
            openContainingFolderToolStripMenuItem.Click += openContainingFolderToolStripMenuItem_Click;
            //
            // selectionIsGayMlToolStripMenuItem
            //
            selectionIsGayMlToolStripMenuItem.Name = "selectionIsGayMlToolStripMenuItem";
            selectionIsGayMlToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            selectionIsGayMlToolStripMenuItem.Text = "Selection is Xml";
            selectionIsGayMlToolStripMenuItem.Click += selectionIsGayMlToolStripMenuItem_Click;
            //
            // filterSelectionToolStripMenuItem
            //
            filterSelectionToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { tsmiFilterExcludeSelection, tsmiFilterIncludeSelection });
            filterSelectionToolStripMenuItem.Name = "filterSelectionToolStripMenuItem";
            filterSelectionToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            filterSelectionToolStripMenuItem.Text = "Filter Selection";
            //
            // tsmiFilterExcludeSelection
            //
            tsmiFilterExcludeSelection.Name = "tsmiFilterExcludeSelection";
            tsmiFilterExcludeSelection.Size = new System.Drawing.Size(126, 22);
            tsmiFilterExcludeSelection.Text = "Exclude ()";
            tsmiFilterExcludeSelection.Click += tsmiFilterExcludeSelection_Click;
            //
            // tsmiFilterIncludeSelection
            //
            tsmiFilterIncludeSelection.Name = "tsmiFilterIncludeSelection";
            tsmiFilterIncludeSelection.Size = new System.Drawing.Size(126, 22);
            tsmiFilterIncludeSelection.Text = "Include ()";
            tsmiFilterIncludeSelection.Click += txtmiFilterIncludeSelection_Click;
            //
            // splitter2
            //
            splitter2.BackColor = System.Drawing.Color.FromArgb(255, 192, 192);
            splitter2.Dock = DockStyle.Bottom;
            splitter2.Location = new System.Drawing.Point(0, 304);
            splitter2.Name = "splitter2";
            splitter2.Size = new System.Drawing.Size(827, 11);
            splitter2.TabIndex = 7;
            splitter2.TabStop = false;
            //
            // splitter1
            //
            splitter1.BackColor = System.Drawing.Color.FromArgb(255, 192, 192);
            splitter1.Dock = DockStyle.Top;
            splitter1.Location = new System.Drawing.Point(0, 164);
            splitter1.Name = "splitter1";
            splitter1.Size = new System.Drawing.Size(827, 12);
            splitter1.TabIndex = 6;
            splitter1.TabStop = false;
            //
            // txtEntryFurtherDetails
            //
            txtEntryFurtherDetails.BackColor = System.Drawing.SystemColors.Window;
            txtEntryFurtherDetails.ContextMenuStrip = contextMenuStrip1;
            txtEntryFurtherDetails.Dock = DockStyle.Bottom;
            txtEntryFurtherDetails.Location = new System.Drawing.Point(0, 315);
            txtEntryFurtherDetails.MinimumSize = new System.Drawing.Size(0, 19);
            txtEntryFurtherDetails.Multiline = true;
            txtEntryFurtherDetails.Name = "txtEntryFurtherDetails";
            txtEntryFurtherDetails.ReadOnly = true;
            txtEntryFurtherDetails.ScrollBars = ScrollBars.Vertical;
            txtEntryFurtherDetails.Size = new System.Drawing.Size(827, 127);
            txtEntryFurtherDetails.TabIndex = 4;
            txtEntryFurtherDetails.Text = "AA";
            //
            // txtDebugEntry
            //
            txtDebugEntry.BackColor = System.Drawing.Color.Lavender;
            txtDebugEntry.ContextMenuStrip = contextMenuStrip1;
            txtDebugEntry.Dock = DockStyle.Top;
            txtDebugEntry.Location = new System.Drawing.Point(0, 0);
            txtDebugEntry.MinimumSize = new System.Drawing.Size(0, 19);
            txtDebugEntry.Multiline = true;
            txtDebugEntry.Name = "txtDebugEntry";
            txtDebugEntry.ReadOnly = true;
            txtDebugEntry.ScrollBars = ScrollBars.Vertical;
            txtDebugEntry.Size = new System.Drawing.Size(827, 164);
            txtDebugEntry.TabIndex = 1;
            //
            // frmExtendedDetails
            //
            AutoScaleBaseSize = new System.Drawing.Size(6, 16);
            CancelButton = btnOk;
            ClientSize = new System.Drawing.Size(827, 483);
            Controls.Add(pnlLogView);
            Controls.Add(panel1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MinimumSize = new System.Drawing.Size(617, 314);
            Name = "frmExtendedDetails";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Log Entry Details";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            pnlLogView.ResumeLayout(false);
            pnlLogView.PerformLayout();
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion Windows Form Designer generated code

        // Default null

        private void chkAllowEdit_CheckedChanged(object sender, EventArgs e) {
            txtDebugEntry.ReadOnly = txtEntryFurtherDetails.ReadOnly = txtSecondaryMessage.ReadOnly = !chkAllowEdit.Checked;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e) {
            //Bilge.Log("ExtendedDetails View, opening context menu");

            tsmiFilterExcludeSelection.Enabled = false;
            tsmiFilterIncludeSelection.Enabled = false;

            TextBox source = ((sender as ContextMenuStrip).SourceControl as TextBox);
            if (source == null) {
                //Bilge.FurtherInfo("Right click did not occur on a textbox, returning");
                return;
            }  // The right click did not occur on a textbox.

            string selectedText = source.SelectedText.Trim();
            if ((selectedText == null) || (selectedText.Length == 0)) {
                //Bilge.FurtherInfo("There was no selected text in this texbox, returning. TB:" + source.Name);
            }

            //Bilge.Log("ExtendedDetails context menu opening for a textbox - " + source.Name, selectedText ?? "No text selected");

            // There are several things that we can do with the selected text, we test to see whether it could be a folder
            // we also test to see whether it looks like xml and finally we update the filters context menu items to allow
            // people to filter based on the selection.

            selectionIsGayMlToolStripMenuItem.Enabled = DoesTextLookLikeXml(selectedText);
            selectionIsGayMlToolStripMenuItem.Tag = selectedText;

            // Now work out whether or not it could be a file or folder that is selected.

            #region Enable or disable the folder and file based right click menu.

            fileExistsToolStripMenuItem.Text = "Not a File / Path";
            fileExistsToolStripMenuItem.Enabled = false;
            openContainingFolderToolStripMenuItem.Text = "Not a File / Path";
            openContainingFolderToolStripMenuItem.Enabled = false;

            if (File.Exists(selectedText)) {
                fileExistsToolStripMenuItem.Text = "Open In Notepad.";
                fileExistsToolStripMenuItem.Enabled = true;
                fileExistsToolStripMenuItem.Tag = selectedText;
            } else {
                fileExistsToolStripMenuItem.Text = "No Such File.";
                fileExistsToolStripMenuItem.Enabled = false;
            }
            if (!string.IsNullOrEmpty(selectedText)) {
                // If it does exist then the folder will be set to open containing folder.
                try {
                    string containingFolder = Path.GetDirectoryName(selectedText);
                    if (Directory.Exists(containingFolder)) {
                        openContainingFolderToolStripMenuItem.Tag = containingFolder;
                        openContainingFolderToolStripMenuItem.Text = "Open Containing Folder";
                        openContainingFolderToolStripMenuItem.Enabled = true;
                    } else {
                        openContainingFolderToolStripMenuItem.Enabled = false;
                    }
                } catch (ArgumentException) {
                    // This indicates that the selected text was not a valid directory, therefore we are ignoring it.
                }
            }

            #endregion Enable or disable the folder and file based right click menu.

            // Now set up the filters correctly so they can quick include and quick exclude.
            m_quickFilterText = selectedText;
            tsmiFilterExcludeSelection.Enabled = true;
            tsmiFilterIncludeSelection.Enabled = true;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private bool DoesTextLookLikeXml(string selectedText) {
            if (selectedText.StartsWith("<")) {
                return true;
            }
            return false;
        }

        private void fileExistsToolStripMenuItem_Click(object sender, EventArgs e) {
            string selectedText = ((sender as ToolStripMenuItem).Tag as string);

            //Bilge.Assert(selectedText != null, "The selected text that made it to open file  was null or empty");
            //Bilge.Assert(selectedText.Length > 0, "The selected text that made it to open file was null or empty");

            ProcessStartInfo psi = new ProcessStartInfo("notepad.exe", selectedText);
            Process.Start(psi);
        }

        private void ModifyQuickFilter(bool include) {
            //Bilge.Assert(m_quickFilterText != null, "Should not have null text selection possible, the button should be disabled if this is the case");
            //Bilge.Assert(m_quickFilterText.Length > 0, "Should not have empty text selection possible, the button should be disabled if this is the case");
            MexCore.TheCore.ViewManager.CurrentFilter.BeginFilterUpdate();
            if (include) {
                MexCore.TheCore.ViewManager.CurrentFilter.AppendIncludeString(m_quickFilterText);
            } else {
                MexCore.TheCore.ViewManager.CurrentFilter.AppendExcludeString(m_quickFilterText);
            }
            MexCore.TheCore.ViewManager.CurrentFilter.EndFilterUpdate();
        }

        private void openContainingFolderToolStripMenuItem_Click(object sender, EventArgs e) {
            string selectedText = ((sender as ToolStripMenuItem).Tag as string);

            //Bilge.Assert(selectedText != null, "The selected text that made it to open file  was null or empty");
            //Bilge.Assert(selectedText.Length > 0, "The selected text that made it to open file was null or empty");

            ProcessStartInfo psi = new ProcessStartInfo("Explorer.exe", "/e," + selectedText);
            Process.Start(psi);
        }

        private void selectionIsGayMlToolStripMenuItem_Click(object sender, EventArgs e) {
            // Take the selected text, store it to a file and fire up IE to go look at it.
            string selectedText = ((sender as ToolStripMenuItem).Tag as string);

            //Bilge.Assert(selectedText != null, "The selected text that made it to treatSelectionAsGayMl was null or empty");
            //Bilge.Assert(selectedText.Length > 0, "The selected text that made it to treatSelectionAsGayMl was null or empty");

            string fname = Path.GetTempFileName();

            using (StreamWriter sw = new StreamWriter(new FileStream(fname, FileMode.Open))) {
                sw.Write(selectedText);
                sw.Close();
            }

            //Bilge.Log("Written GayML to a file and now launching internet explorer to display contents");
            ProcessStartInfo psi = new ProcessStartInfo("Iexplore.exe", fname);
            Process.Start(psi);
        }

        private void tsmiFilterExcludeSelection_Click(object sender, EventArgs e) {
            ModifyQuickFilter(false);
        }

        private void txtmiFilterIncludeSelection_Click(object sender, EventArgs e) {
            ModifyQuickFilter(true);
        }
    }
}