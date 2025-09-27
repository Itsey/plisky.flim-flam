using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Plisky.Diagnostics;
using Plisky.Plumbing;

namespace Plisky.FlimFlam { 

    /// <summary>
    /// Used to manage the wizard pages, gives them names instead of numbers.
    /// </summary>
    internal enum AddHighlightWizPages {
        StartPage,
        AddHighlight,
        ChooseColor
    }

    /// <summary>
    /// Summary description for frmHighlighting.
    /// </summary>
    internal class frmHighlighting : System.Windows.Forms.Form {
        private bool autoloadspopulated;

        private Button btnAddAutoLoadHighlight;

        private Button btnAddHighlightIn;

        private Button btnAddNewHighlight;

        private Button btnAddNewNext;

        private Button btnAutoLoadHighlightRemove;

        private Button btnCancel;

        private Button btnDeleteSelected;

        private Button btnDeleteSelectedDiskhighlight;

        private Button btnGoBack;

        private Button btnLoadHighlight;

        private Button btnOk;

        private Button btnRemoveAllHighlights;

        private Button btnSaveSelected;

        private Button btnSetDefaultHighlights;

        private Button btnShowLoadSave;

        private Button button2;

        private Plisky.UIWinforms.Controls.ColorComboList cclCustomBackground;

        private Plisky.UIWinforms.Controls.ColorComboList cclPredefinedScheme;

        private CheckBox chkNotMatch;

        private CheckBox chkUseCase;

        private CheckedListBox clbKnownEventTypes;

        private ColumnHeader columnHeader1;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        private GroupBox groupBox1;

        private GroupBox grpAddNewTypeOfHighlight;

        private Label label1;

        private Label label2;

        private Label label3;

        private Label label4;

        private Label label5;

        private ListBox lbxAutoLoads;

        private ListBox lbxDiskHighlights;

        private ListBox lbxLoadedHighlights;

        private ListView listView1;

        private ListView lvwTextFilterHighlights;

        private List<AHighlightRequest> m_highLightRequests = new List<AHighlightRequest>();

        private RadioButton rdoAddTypeHighlight;

        private RadioButton rdoTextHighlight;

        private RadioButton rdoUseCustomScheme;

        private RadioButton rdoUseExistingScheme;

        private TabPage tabAddNewHighlight;

        private TabPage tabExistingHighlights;

        private TabPage tabLoadSave;

        private TabPage tabSelectColor;

        private TabControl tbcHighlightWizzard;

        private TextBox txtHighlightFindMatch;

        private TextBox txtHighlightName;

        private TextBox txtSampleHighlight;

        internal frmHighlighting() {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // Populate the EventType highlighing options with their current selection and or defaults.
            // TODO : Store and populte event types

            clbKnownEventTypes.Items.Clear();
            foreach (TraceCommandTypes traceType in Enum.GetValues(typeof(TraceCommandTypes))) {
                if ((traceType == TraceCommandTypes.Unknown) || (traceType == TraceCommandTypes.CommandOnly)) { continue; }
                clbKnownEventTypes.Items.Add(FlimFlamTraceCommands.TraceCommandToReadableString(traceType));
            }
        }

        internal HighlightRequestsStore GetNewHighlightStore() {
            HighlightRequestsStore result = new HighlightRequestsStore();
            result.highlightRequests = m_highLightRequests;
            return result;
        }

        internal void PopulateFromRequests(HighlightRequestsStore useThis) {
            lvwTextFilterHighlights.Items.Clear();
            m_highLightRequests.Clear();

            if ((useThis.highlightRequests != null) && (useThis.highlightRequests.Count > 0)) {
                foreach (AHighlightRequest ahr in useThis.highlightRequests) {
                    m_highLightRequests.Add(ahr);
                }
                RefreshListviewFromHighlightStore();
            } // end if there are existing highlights to be displayed
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHighlighting));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.tabSelectColor = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.txtHighlightName = new System.Windows.Forms.TextBox();
            this.btnAddHighlightIn = new System.Windows.Forms.Button();
            this.txtSampleHighlight = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cclCustomBackground = new Plisky.UIWinforms.Controls.ColorComboList();
            this.cclPredefinedScheme = new Plisky.UIWinforms.Controls.ColorComboList();
            this.rdoUseCustomScheme = new System.Windows.Forms.RadioButton();
            this.rdoUseExistingScheme = new System.Windows.Forms.RadioButton();
            this.tabAddNewHighlight = new System.Windows.Forms.TabPage();
            this.btnAddNewNext = new System.Windows.Forms.Button();
            this.grpAddNewTypeOfHighlight = new System.Windows.Forms.GroupBox();
            this.rdoTextHighlight = new System.Windows.Forms.RadioButton();
            this.rdoAddTypeHighlight = new System.Windows.Forms.RadioButton();
            this.chkNotMatch = new System.Windows.Forms.CheckBox();
            this.chkUseCase = new System.Windows.Forms.CheckBox();
            this.txtHighlightFindMatch = new System.Windows.Forms.TextBox();
            this.clbKnownEventTypes = new System.Windows.Forms.CheckedListBox();
            this.tabExistingHighlights = new System.Windows.Forms.TabPage();
            this.btnSetDefaultHighlights = new System.Windows.Forms.Button();
            this.btnShowLoadSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddNewHighlight = new System.Windows.Forms.Button();
            this.btnDeleteSelected = new System.Windows.Forms.Button();
            this.btnRemoveAllHighlights = new System.Windows.Forms.Button();
            this.lvwTextFilterHighlights = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tbcHighlightWizzard = new System.Windows.Forms.TabControl();
            this.tabLoadSave = new System.Windows.Forms.TabPage();
            this.btnDeleteSelectedDiskhighlight = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAutoLoadHighlightRemove = new System.Windows.Forms.Button();
            this.btnAddAutoLoadHighlight = new System.Windows.Forms.Button();
            this.lbxAutoLoads = new System.Windows.Forms.ListBox();
            this.btnGoBack = new System.Windows.Forms.Button();
            this.btnLoadHighlight = new System.Windows.Forms.Button();
            this.btnSaveSelected = new System.Windows.Forms.Button();
            this.lbxDiskHighlights = new System.Windows.Forms.ListBox();
            this.lbxLoadedHighlights = new System.Windows.Forms.ListBox();
            this.tabSelectColor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabAddNewHighlight.SuspendLayout();
            this.grpAddNewTypeOfHighlight.SuspendLayout();
            this.tabExistingHighlights.SuspendLayout();
            this.tbcHighlightWizzard.SuspendLayout();
            this.tabLoadSave.SuspendLayout();
            this.SuspendLayout();
            //
            // btnCancel
            //
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(499, 310);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "C&ancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            //
            // btnOk
            //
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(585, 310);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 14;
            this.btnOk.Text = "&Ok";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            //
            // button2
            //
            this.button2.Location = new System.Drawing.Point(460, 252);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(52, 23);
            this.button2.TabIndex = 18;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            //
            // listView1
            //
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listView1.Location = new System.Drawing.Point(6, 121);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(360, 178);
            this.listView1.TabIndex = 15;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            //
            // tabSelectColor
            //
            this.tabSelectColor.Controls.Add(this.label5);
            this.tabSelectColor.Controls.Add(this.txtHighlightName);
            this.tabSelectColor.Controls.Add(this.btnAddHighlightIn);
            this.tabSelectColor.Controls.Add(this.txtSampleHighlight);
            this.tabSelectColor.Controls.Add(this.groupBox1);
            this.tabSelectColor.Location = new System.Drawing.Point(4, 5);
            this.tabSelectColor.Name = "tabSelectColor";
            this.tabSelectColor.Padding = new System.Windows.Forms.Padding(3);
            this.tabSelectColor.Size = new System.Drawing.Size(675, 299);
            this.tabSelectColor.TabIndex = 2;
            this.tabSelectColor.Text = "tabPage3";
            this.tabSelectColor.UseVisualStyleBackColor = true;
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(29, 125);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 46;
            this.label5.Text = "Highlight Name";
            //
            // txtHighlightName
            //
            this.txtHighlightName.Location = new System.Drawing.Point(113, 122);
            this.txtHighlightName.Name = "txtHighlightName";
            this.txtHighlightName.Size = new System.Drawing.Size(281, 21);
            this.txtHighlightName.TabIndex = 45;
            this.txtHighlightName.Text = "<default>";
            this.txtHighlightName.TextChanged += new System.EventHandler(this.txtHighlightName_TextChanged);
            //
            // btnAddHighlightIn
            //
            this.btnAddHighlightIn.Enabled = false;
            this.btnAddHighlightIn.Location = new System.Drawing.Point(535, 259);
            this.btnAddHighlightIn.Name = "btnAddHighlightIn";
            this.btnAddHighlightIn.Size = new System.Drawing.Size(121, 23);
            this.btnAddHighlightIn.TabIndex = 44;
            this.btnAddHighlightIn.Text = "Add This Highlight";
            this.btnAddHighlightIn.UseVisualStyleBackColor = true;
            this.btnAddHighlightIn.Click += new System.EventHandler(this.btnAddHighlightIn_Click);
            //
            // txtSampleHighlight
            //
            this.txtSampleHighlight.Location = new System.Drawing.Point(15, 218);
            this.txtSampleHighlight.Name = "txtSampleHighlight";
            this.txtSampleHighlight.Size = new System.Drawing.Size(641, 21);
            this.txtSampleHighlight.TabIndex = 43;
            //
            // groupBox1
            //
            this.groupBox1.Controls.Add(this.cclCustomBackground);
            this.groupBox1.Controls.Add(this.cclPredefinedScheme);
            this.groupBox1.Controls.Add(this.rdoUseCustomScheme);
            this.groupBox1.Controls.Add(this.rdoUseExistingScheme);
            this.groupBox1.Location = new System.Drawing.Point(15, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(379, 100);
            this.groupBox1.TabIndex = 42;
            this.groupBox1.TabStop = false;
            //
            // cclCustomBackground
            //
            this.cclCustomBackground.ColorSet = Plisky.UIWinforms.Controls.ColorListSet.KnownColors;
            this.cclCustomBackground.Enabled = false;
            this.cclCustomBackground.Location = new System.Drawing.Point(204, 53);
            this.cclCustomBackground.Name = "cclCustomBackground";
            this.cclCustomBackground.Size = new System.Drawing.Size(156, 24);
            this.cclCustomBackground.TabIndex = 43;
            this.cclCustomBackground.SelectedColorChangedEventHandler += new Plisky.UIWinforms.Controls.ColorChangedEventHandler(this.cclCustomBackground_SelectedColorChangedEventHandler);
            //
            // cclPredefinedScheme
            //
            this.cclPredefinedScheme.ColorSet = Plisky.UIWinforms.Controls.ColorListSet.DefaultHighlights;
            this.cclPredefinedScheme.Location = new System.Drawing.Point(204, 21);
            this.cclPredefinedScheme.Name = "cclPredefinedScheme";
            this.cclPredefinedScheme.Size = new System.Drawing.Size(158, 24);
            this.cclPredefinedScheme.TabIndex = 40;
            this.cclPredefinedScheme.SelectedColorChangedEventHandler += new Plisky.UIWinforms.Controls.ColorChangedEventHandler(this.cclPredefinedScheme_SelectedColorChangedEventHandler);
            //
            // rdoUseCustomScheme
            //
            this.rdoUseCustomScheme.AutoSize = true;
            this.rdoUseCustomScheme.Location = new System.Drawing.Point(17, 53);
            this.rdoUseCustomScheme.Name = "rdoUseCustomScheme";
            this.rdoUseCustomScheme.Size = new System.Drawing.Size(105, 17);
            this.rdoUseCustomScheme.TabIndex = 1;
            this.rdoUseCustomScheme.Text = "Custom Scheme.";
            this.rdoUseCustomScheme.UseVisualStyleBackColor = true;
            //
            // rdoUseExistingScheme
            //
            this.rdoUseExistingScheme.AutoSize = true;
            this.rdoUseExistingScheme.Checked = true;
            this.rdoUseExistingScheme.Location = new System.Drawing.Point(17, 20);
            this.rdoUseExistingScheme.Name = "rdoUseExistingScheme";
            this.rdoUseExistingScheme.Size = new System.Drawing.Size(121, 17);
            this.rdoUseExistingScheme.TabIndex = 0;
            this.rdoUseExistingScheme.TabStop = true;
            this.rdoUseExistingScheme.Text = "Predefined Scheme.";
            this.rdoUseExistingScheme.UseVisualStyleBackColor = true;
            this.rdoUseExistingScheme.CheckedChanged += new System.EventHandler(this.rdoUseExistingScheme_CheckedChanged);
            //
            // tabAddNewHighlight
            //
            this.tabAddNewHighlight.Controls.Add(this.btnAddNewNext);
            this.tabAddNewHighlight.Controls.Add(this.grpAddNewTypeOfHighlight);
            this.tabAddNewHighlight.Location = new System.Drawing.Point(4, 5);
            this.tabAddNewHighlight.Name = "tabAddNewHighlight";
            this.tabAddNewHighlight.Padding = new System.Windows.Forms.Padding(3);
            this.tabAddNewHighlight.Size = new System.Drawing.Size(675, 299);
            this.tabAddNewHighlight.TabIndex = 1;
            this.tabAddNewHighlight.UseVisualStyleBackColor = true;
            //
            // btnAddNewNext
            //
            this.btnAddNewNext.Enabled = false;
            this.btnAddNewNext.Location = new System.Drawing.Point(580, 276);
            this.btnAddNewNext.Name = "btnAddNewNext";
            this.btnAddNewNext.Size = new System.Drawing.Size(75, 23);
            this.btnAddNewNext.TabIndex = 35;
            this.btnAddNewNext.Text = "Next >>";
            this.btnAddNewNext.UseVisualStyleBackColor = true;
            this.btnAddNewNext.Click += new System.EventHandler(this.btnAddNewNext_Click);
            //
            // grpAddNewTypeOfHighlight
            //
            this.grpAddNewTypeOfHighlight.Controls.Add(this.rdoTextHighlight);
            this.grpAddNewTypeOfHighlight.Controls.Add(this.rdoAddTypeHighlight);
            this.grpAddNewTypeOfHighlight.Controls.Add(this.chkNotMatch);
            this.grpAddNewTypeOfHighlight.Controls.Add(this.chkUseCase);
            this.grpAddNewTypeOfHighlight.Controls.Add(this.txtHighlightFindMatch);
            this.grpAddNewTypeOfHighlight.Controls.Add(this.clbKnownEventTypes);
            this.grpAddNewTypeOfHighlight.Location = new System.Drawing.Point(6, 6);
            this.grpAddNewTypeOfHighlight.Name = "grpAddNewTypeOfHighlight";
            this.grpAddNewTypeOfHighlight.Size = new System.Drawing.Size(649, 256);
            this.grpAddNewTypeOfHighlight.TabIndex = 34;
            this.grpAddNewTypeOfHighlight.TabStop = false;
            //
            // rdoTextHighlight
            //
            this.rdoTextHighlight.AutoSize = true;
            this.rdoTextHighlight.Location = new System.Drawing.Point(21, 158);
            this.rdoTextHighlight.Name = "rdoTextHighlight";
            this.rdoTextHighlight.Size = new System.Drawing.Size(240, 17);
            this.rdoTextHighlight.TabIndex = 39;
            this.rdoTextHighlight.TabStop = true;
            this.rdoTextHighlight.Text = "Add a highlight where the Text matches this:";
            this.rdoTextHighlight.UseVisualStyleBackColor = true;
            this.rdoTextHighlight.CheckedChanged += new System.EventHandler(this.rdoTextHighlight_CheckedChanged);
            //
            // rdoAddTypeHighlight
            //
            this.rdoAddTypeHighlight.AutoSize = true;
            this.rdoAddTypeHighlight.Location = new System.Drawing.Point(21, 21);
            this.rdoAddTypeHighlight.Name = "rdoAddTypeHighlight";
            this.rdoAddTypeHighlight.Size = new System.Drawing.Size(186, 17);
            this.rdoAddTypeHighlight.TabIndex = 38;
            this.rdoAddTypeHighlight.TabStop = true;
            this.rdoAddTypeHighlight.Text = "Add A Highlight To an Event Type";
            this.rdoAddTypeHighlight.UseVisualStyleBackColor = true;
            this.rdoAddTypeHighlight.CheckedChanged += new System.EventHandler(this.rdoAddTypeHighlight_CheckedChanged);
            //
            // chkNotMatch
            //
            this.chkNotMatch.Enabled = false;
            this.chkNotMatch.Location = new System.Drawing.Point(49, 208);
            this.chkNotMatch.Name = "chkNotMatch";
            this.chkNotMatch.Size = new System.Drawing.Size(538, 24);
            this.chkNotMatch.TabIndex = 37;
            this.chkNotMatch.Text = "Where the line does not contain <>";
            //
            // chkUseCase
            //
            this.chkUseCase.Enabled = false;
            this.chkUseCase.Location = new System.Drawing.Point(49, 226);
            this.chkUseCase.Name = "chkUseCase";
            this.chkUseCase.Size = new System.Drawing.Size(217, 24);
            this.chkUseCase.TabIndex = 36;
            this.chkUseCase.Text = "Make this comparison Case Sensitive.";
            //
            // txtHighlightFindMatch
            //
            this.txtHighlightFindMatch.Enabled = false;
            this.txtHighlightFindMatch.Location = new System.Drawing.Point(49, 181);
            this.txtHighlightFindMatch.Name = "txtHighlightFindMatch";
            this.txtHighlightFindMatch.Size = new System.Drawing.Size(538, 21);
            this.txtHighlightFindMatch.TabIndex = 35;
            this.txtHighlightFindMatch.TextChanged += new System.EventHandler(this.txtHighlightFindMatch_TextChanged);
            //
            // clbKnownEventTypes
            //
            this.clbKnownEventTypes.CheckOnClick = true;
            this.clbKnownEventTypes.ColumnWidth = 130;
            this.clbKnownEventTypes.Enabled = false;
            this.clbKnownEventTypes.FormattingEnabled = true;
            this.clbKnownEventTypes.Location = new System.Drawing.Point(49, 44);
            this.clbKnownEventTypes.MultiColumn = true;
            this.clbKnownEventTypes.Name = "clbKnownEventTypes";
            this.clbKnownEventTypes.Size = new System.Drawing.Size(538, 84);
            this.clbKnownEventTypes.TabIndex = 34;
            this.clbKnownEventTypes.SelectedIndexChanged += new System.EventHandler(this.clbKnownEventTypes_SelectedIndexChanged);
            //
            // tabExistingHighlights
            //
            this.tabExistingHighlights.Controls.Add(this.btnSetDefaultHighlights);
            this.tabExistingHighlights.Controls.Add(this.btnShowLoadSave);
            this.tabExistingHighlights.Controls.Add(this.label1);
            this.tabExistingHighlights.Controls.Add(this.btnAddNewHighlight);
            this.tabExistingHighlights.Controls.Add(this.btnDeleteSelected);
            this.tabExistingHighlights.Controls.Add(this.btnRemoveAllHighlights);
            this.tabExistingHighlights.Controls.Add(this.lvwTextFilterHighlights);
            this.tabExistingHighlights.Location = new System.Drawing.Point(4, 5);
            this.tabExistingHighlights.Name = "tabExistingHighlights";
            this.tabExistingHighlights.Padding = new System.Windows.Forms.Padding(3);
            this.tabExistingHighlights.Size = new System.Drawing.Size(675, 299);
            this.tabExistingHighlights.TabIndex = 0;
            this.tabExistingHighlights.Text = "tabPage1";
            this.tabExistingHighlights.UseVisualStyleBackColor = true;
            //
            // btnSetDefaultHighlights
            //
            this.btnSetDefaultHighlights.Location = new System.Drawing.Point(319, 269);
            this.btnSetDefaultHighlights.Name = "btnSetDefaultHighlights";
            this.btnSetDefaultHighlights.Size = new System.Drawing.Size(133, 23);
            this.btnSetDefaultHighlights.TabIndex = 41;
            this.btnSetDefaultHighlights.Text = "Add Default Highlights";
            this.btnSetDefaultHighlights.Click += new System.EventHandler(this.btnSetDefaultHighlights_Click);
            //
            // btnShowLoadSave
            //
            this.btnShowLoadSave.Location = new System.Drawing.Point(205, 269);
            this.btnShowLoadSave.Name = "btnShowLoadSave";
            this.btnShowLoadSave.Size = new System.Drawing.Size(108, 23);
            this.btnShowLoadSave.TabIndex = 40;
            this.btnShowLoadSave.Text = "Load / Save";
            this.btnShowLoadSave.UseVisualStyleBackColor = true;
            this.btnShowLoadSave.Click += new System.EventHandler(this.btnShowLoadSave_Click);
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 39;
            this.label1.Text = "Existing Highlights:";
            //
            // btnAddNewHighlight
            //
            this.btnAddNewHighlight.Location = new System.Drawing.Point(495, 269);
            this.btnAddNewHighlight.Name = "btnAddNewHighlight";
            this.btnAddNewHighlight.Size = new System.Drawing.Size(161, 23);
            this.btnAddNewHighlight.TabIndex = 38;
            this.btnAddNewHighlight.Text = "Add New Highlight";
            this.btnAddNewHighlight.UseVisualStyleBackColor = true;
            this.btnAddNewHighlight.Click += new System.EventHandler(this.btnAddNewHighlight_Click);
            //
            // btnDeleteSelected
            //
            this.btnDeleteSelected.Enabled = false;
            this.btnDeleteSelected.Location = new System.Drawing.Point(87, 269);
            this.btnDeleteSelected.Name = "btnDeleteSelected";
            this.btnDeleteSelected.Size = new System.Drawing.Size(112, 23);
            this.btnDeleteSelected.TabIndex = 35;
            this.btnDeleteSelected.Text = "Remove Selected";
            this.btnDeleteSelected.Click += new System.EventHandler(this.btnDeleteSelected_Click);
            //
            // btnRemoveAllHighlights
            //
            this.btnRemoveAllHighlights.Enabled = false;
            this.btnRemoveAllHighlights.Location = new System.Drawing.Point(9, 269);
            this.btnRemoveAllHighlights.Name = "btnRemoveAllHighlights";
            this.btnRemoveAllHighlights.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveAllHighlights.TabIndex = 33;
            this.btnRemoveAllHighlights.Text = "Remove All";
            this.btnRemoveAllHighlights.Click += new System.EventHandler(this.btnRemoveAllHighlights_Click);
            //
            // lvwTextFilterHighlights
            //
            this.lvwTextFilterHighlights.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lvwTextFilterHighlights.FullRowSelect = true;
            this.lvwTextFilterHighlights.GridLines = true;
            this.lvwTextFilterHighlights.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvwTextFilterHighlights.Location = new System.Drawing.Point(9, 38);
            this.lvwTextFilterHighlights.MultiSelect = false;
            this.lvwTextFilterHighlights.Name = "lvwTextFilterHighlights";
            this.lvwTextFilterHighlights.Size = new System.Drawing.Size(647, 226);
            this.lvwTextFilterHighlights.TabIndex = 23;
            this.lvwTextFilterHighlights.UseCompatibleStateImageBehavior = false;
            this.lvwTextFilterHighlights.View = System.Windows.Forms.View.Details;
            this.lvwTextFilterHighlights.SelectedIndexChanged += new System.EventHandler(this.lvwTextFilterHighlights_SelectedIndexChanged);
            //
            // columnHeader1
            //
            this.columnHeader1.Width = 530;
            //
            // tbcHighlightWizzard
            //
            this.tbcHighlightWizzard.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tbcHighlightWizzard.Controls.Add(this.tabExistingHighlights);
            this.tbcHighlightWizzard.Controls.Add(this.tabAddNewHighlight);
            this.tbcHighlightWizzard.Controls.Add(this.tabSelectColor);
            this.tbcHighlightWizzard.Controls.Add(this.tabLoadSave);
            this.tbcHighlightWizzard.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbcHighlightWizzard.ItemSize = new System.Drawing.Size(0, 1);
            this.tbcHighlightWizzard.Location = new System.Drawing.Point(0, 0);
            this.tbcHighlightWizzard.Name = "tbcHighlightWizzard";
            this.tbcHighlightWizzard.SelectedIndex = 0;
            this.tbcHighlightWizzard.Size = new System.Drawing.Size(683, 308);
            this.tbcHighlightWizzard.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tbcHighlightWizzard.TabIndex = 39;
            //
            // tabLoadSave
            //
            this.tabLoadSave.Controls.Add(this.btnDeleteSelectedDiskhighlight);
            this.tabLoadSave.Controls.Add(this.label4);
            this.tabLoadSave.Controls.Add(this.label3);
            this.tabLoadSave.Controls.Add(this.label2);
            this.tabLoadSave.Controls.Add(this.btnAutoLoadHighlightRemove);
            this.tabLoadSave.Controls.Add(this.btnAddAutoLoadHighlight);
            this.tabLoadSave.Controls.Add(this.lbxAutoLoads);
            this.tabLoadSave.Controls.Add(this.btnGoBack);
            this.tabLoadSave.Controls.Add(this.btnLoadHighlight);
            this.tabLoadSave.Controls.Add(this.btnSaveSelected);
            this.tabLoadSave.Controls.Add(this.lbxDiskHighlights);
            this.tabLoadSave.Controls.Add(this.lbxLoadedHighlights);
            this.tabLoadSave.Location = new System.Drawing.Point(4, 5);
            this.tabLoadSave.Name = "tabLoadSave";
            this.tabLoadSave.Size = new System.Drawing.Size(675, 299);
            this.tabLoadSave.TabIndex = 3;
            this.tabLoadSave.Text = "tabPage1";
            this.tabLoadSave.UseVisualStyleBackColor = true;
            //
            // btnDeleteSelectedDiskhighlight
            //
            this.btnDeleteSelectedDiskhighlight.Location = new System.Drawing.Point(260, 264);
            this.btnDeleteSelectedDiskhighlight.Name = "btnDeleteSelectedDiskhighlight";
            this.btnDeleteSelectedDiskhighlight.Size = new System.Drawing.Size(193, 23);
            this.btnDeleteSelectedDiskhighlight.TabIndex = 14;
            this.btnDeleteSelectedDiskhighlight.Text = "Delete";
            this.btnDeleteSelectedDiskhighlight.UseVisualStyleBackColor = true;
            this.btnDeleteSelectedDiskhighlight.Click += new System.EventHandler(this.btnDeleteSelectedDiskhighlight_Click);
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(511, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Highlights Auto Loaded";
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(284, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Highlights Saved To Disk";
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Highlights Loaded in Mex";
            //
            // btnAutoLoadHighlightRemove
            //
            this.btnAutoLoadHighlightRemove.Location = new System.Drawing.Point(459, 153);
            this.btnAutoLoadHighlightRemove.Name = "btnAutoLoadHighlightRemove";
            this.btnAutoLoadHighlightRemove.Size = new System.Drawing.Size(32, 105);
            this.btnAutoLoadHighlightRemove.TabIndex = 9;
            this.btnAutoLoadHighlightRemove.Text = "<";
            this.btnAutoLoadHighlightRemove.UseVisualStyleBackColor = true;
            this.btnAutoLoadHighlightRemove.Click += new System.EventHandler(this.btnAutoLoadHighlightRemove_Click);
            //
            // btnAddAutoLoadHighlight
            //
            this.btnAddAutoLoadHighlight.Location = new System.Drawing.Point(459, 46);
            this.btnAddAutoLoadHighlight.Name = "btnAddAutoLoadHighlight";
            this.btnAddAutoLoadHighlight.Size = new System.Drawing.Size(32, 92);
            this.btnAddAutoLoadHighlight.TabIndex = 8;
            this.btnAddAutoLoadHighlight.Text = ">";
            this.btnAddAutoLoadHighlight.UseVisualStyleBackColor = true;
            this.btnAddAutoLoadHighlight.Click += new System.EventHandler(this.btnAddAutoLoadHighlight_Click);
            //
            // lbxAutoLoads
            //
            this.lbxAutoLoads.FormattingEnabled = true;
            this.lbxAutoLoads.Location = new System.Drawing.Point(497, 46);
            this.lbxAutoLoads.Name = "lbxAutoLoads";
            this.lbxAutoLoads.Size = new System.Drawing.Size(161, 212);
            this.lbxAutoLoads.TabIndex = 7;
            //
            // btnGoBack
            //
            this.btnGoBack.Location = new System.Drawing.Point(497, 264);
            this.btnGoBack.Name = "btnGoBack";
            this.btnGoBack.Size = new System.Drawing.Size(161, 23);
            this.btnGoBack.TabIndex = 6;
            this.btnGoBack.Text = "Back";
            this.btnGoBack.UseVisualStyleBackColor = true;
            this.btnGoBack.Click += new System.EventHandler(this.btnGoBack_Click);
            //
            // btnLoadHighlight
            //
            this.btnLoadHighlight.Location = new System.Drawing.Point(222, 153);
            this.btnLoadHighlight.Name = "btnLoadHighlight";
            this.btnLoadHighlight.Size = new System.Drawing.Size(32, 105);
            this.btnLoadHighlight.TabIndex = 5;
            this.btnLoadHighlight.Text = "<";
            this.btnLoadHighlight.UseVisualStyleBackColor = true;
            this.btnLoadHighlight.Click += new System.EventHandler(this.btnLoadHighlight_Click);
            //
            // btnSaveSelected
            //
            this.btnSaveSelected.Location = new System.Drawing.Point(222, 46);
            this.btnSaveSelected.Name = "btnSaveSelected";
            this.btnSaveSelected.Size = new System.Drawing.Size(32, 92);
            this.btnSaveSelected.TabIndex = 4;
            this.btnSaveSelected.Text = ">";
            this.btnSaveSelected.UseVisualStyleBackColor = true;
            this.btnSaveSelected.Click += new System.EventHandler(this.btnSaveSelected_Click);
            //
            // lbxDiskHighlights
            //
            this.lbxDiskHighlights.FormattingEnabled = true;
            this.lbxDiskHighlights.Location = new System.Drawing.Point(260, 46);
            this.lbxDiskHighlights.Name = "lbxDiskHighlights";
            this.lbxDiskHighlights.Size = new System.Drawing.Size(193, 212);
            this.lbxDiskHighlights.TabIndex = 3;
            this.lbxDiskHighlights.SelectedIndexChanged += new System.EventHandler(this.lbxDiskHighlights_SelectedIndexChanged);
            //
            // lbxLoadedHighlights
            //
            this.lbxLoadedHighlights.FormattingEnabled = true;
            this.lbxLoadedHighlights.Location = new System.Drawing.Point(18, 46);
            this.lbxLoadedHighlights.Name = "lbxLoadedHighlights";
            this.lbxLoadedHighlights.Size = new System.Drawing.Size(198, 212);
            this.lbxLoadedHighlights.TabIndex = 2;
            //
            // frmHighlighting
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(683, 359);
            this.Controls.Add(this.tbcHighlightWizzard);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(691, 386);
            this.MinimumSize = new System.Drawing.Size(691, 386);
            this.Name = "frmHighlighting";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmHighlighting";
            this.tabSelectColor.ResumeLayout(false);
            this.tabSelectColor.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabAddNewHighlight.ResumeLayout(false);
            this.grpAddNewTypeOfHighlight.ResumeLayout(false);
            this.grpAddNewTypeOfHighlight.PerformLayout();
            this.tabExistingHighlights.ResumeLayout(false);
            this.tabExistingHighlights.PerformLayout();
            this.tbcHighlightWizzard.ResumeLayout(false);
            this.tabLoadSave.ResumeLayout(false);
            this.tabLoadSave.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion Windows Form Designer generated code

        private AHighlightRequest AddHighlightFromText() {
            AHighlightRequest ahr = new AHighlightRequest(Color.Empty, Color.Empty, txtHighlightFindMatch.Text, chkNotMatch.Checked, chkUseCase.Checked);

            return ahr;
        }

        private AHighlightRequest AddHighlightFromTypes(TraceCommandTypes newType) {
            AHighlightRequest result;

            // Check if there is already a highlight for this type and if there is prompt them to remove it, if they dont want
            // to remove it then we return null.
            if (isSelectedEventTypeAlreadyHighlighted(newType)) {
                return null;
            }

            result = new AHighlightRequest(Color.Empty, Color.Empty, newType, false, false);

            return result;
        }

        private void btnAddAutoLoadHighlight_Click(object sender, EventArgs e) {
            if (lbxDiskHighlights.SelectedItem == null) { return; }
            lbxAutoLoads.Items.Add(lbxDiskHighlights.SelectedItem);
        }

        private void btnAddHighlightIn_Click(object sender, EventArgs e) {
            AHighlightRequest nextRequest = null;

            if (rdoAddTypeHighlight.Checked) {
                TraceCommandTypes tct = TraceCommandTypes.Unknown;

                foreach (string s in clbKnownEventTypes.CheckedItems) {
                    tct |= FlimFlamTraceCommands.ReadableStringToTraceCommand(s);
                }

                nextRequest = AddHighlightFromTypes(tct);
                if (nextRequest == null) { return; }
            } else {
                nextRequest = AddHighlightFromText();
                if (nextRequest == null) { return; }
            }

            if (rdoUseExistingScheme.Checked) {
                nextRequest.BackgroundColor = cclPredefinedScheme.SelectedBackgroundColor;
                nextRequest.ForegroundColor = cclPredefinedScheme.SelectedForegroundColor;
            } else {
                nextRequest.ForegroundColor = cclPredefinedScheme.SelectedForegroundColor;
                nextRequest.BackgroundColor = cclCustomBackground.SelectedForegroundColor;
            }

            nextRequest.DescriptionString = txtSampleHighlight.Text;
            if (txtHighlightName.Text == "<default>") {
                nextRequest.NameString = nextRequest.DescriptionString;
            } else {
                nextRequest.NameString = txtHighlightName.Text;
            }
            txtHighlightName.Text = "<default>";

            m_highLightRequests.Add(nextRequest);

            RefreshListviewFromHighlightStore();
            ChangeAddHighlightWizzardPage(AddHighlightWizPages.StartPage);
        }

        private void btnAddNewHighlight_Click(object sender, EventArgs e) {
            ChangeAddHighlightWizzardPage(AddHighlightWizPages.AddHighlight);
        }

        private void btnAddNewNext_Click(object sender, EventArgs e) {
            ChangeAddHighlightWizzardPage(AddHighlightWizPages.ChooseColor);
        }

        private void btnAutoLoadHighlightRemove_Click(object sender, EventArgs e) {
            lbxAutoLoads.Items.Remove(lbxAutoLoads.SelectedItem);
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            if (tbcHighlightWizzard.SelectedTab != tabExistingHighlights) {
                ChangeAddHighlightWizzardPage(AddHighlightWizPages.StartPage);
            } else {
                this.DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void btnDeleteSelected_Click(object sender, EventArgs e) {
            string s = lvwTextFilterHighlights.SelectedItems[0].Text;
            int offset;
            for (offset = 0; offset < m_highLightRequests.Count; offset++) {
                if (m_highLightRequests[offset].DescriptionString == s) {
                    m_highLightRequests.RemoveAt(offset);
                    break;
                }
            }
            RefreshListviewFromHighlightStore();
        }

        private void btnDeleteSelectedDiskhighlight_Click(object sender, EventArgs e) {
            if (lbxDiskHighlights.SelectedItem == null) { return; }
            AHighlightRequest ahr = (AHighlightRequest)lbxDiskHighlights.SelectedItem;

            string pathToSave = Path.Combine(MexCore.TheCore.Options.FilterAndHighlightStoreDirectory, ahr.NameString);
            pathToSave = pathToSave + MexCore.TheCore.Options.HighlightExtension;

            if (File.Exists(pathToSave)) { File.Delete(pathToSave); }

            RefreshDiskHighlightList();
        }

        private void btnGoBack_Click(object sender, EventArgs e) {
            tbcHighlightWizzard.SelectedTab = tabExistingHighlights;
            RefreshListviewFromHighlightStore();
        }

        private void btnLoadHighlight_Click(object sender, EventArgs e) {
            if (lbxDiskHighlights.SelectedItem == null) { return; }
            AHighlightRequest ahr = (AHighlightRequest)lbxDiskHighlights.SelectedItem;
            m_highLightRequests.Add(ahr);
            lbxLoadedHighlights.Items.Add(ahr);
        }

        private void btnOk_Click(object sender, EventArgs e) {
            if (autoloadspopulated) {
                // If they have had a chance to mess with the autoloads then  save them back.
                if (lbxAutoLoads.Items.Count > 0) {
                    MexCore.TheCore.Options.HighlightDefaultProfileName = string.Empty;

                    foreach (AHighlightRequest ahr in lbxAutoLoads.Items) {
                        MexCore.TheCore.Options.HighlightDefaultProfileName += ahr.NameString + ";";
                    }
                } else {
                    MexCore.TheCore.Options.HighlightDefaultProfileName = string.Empty;
                }
            }
        }

        private void btnRemoveAllHighlights_Click(object sender, EventArgs e) {
            m_highLightRequests.Clear();
            RefreshListviewFromHighlightStore();
        }

        private void btnSaveSelected_Click(object sender, EventArgs e) {
            if (lbxLoadedHighlights.SelectedItem == null) { return; }

            AHighlightRequest ahr = (AHighlightRequest)lbxLoadedHighlights.SelectedItem;
            AHighlightRequest.SaveHighlight(ahr);
            lbxDiskHighlights.Items.Add(ahr);
        }

        private void btnSetDefaultHighlights_Click(object sender, EventArgs e) {
            AHighlightRequest ahr = new AHighlightRequest(Color.FromKnownColor(KnownColor.DarkOrange), Color.Empty, TraceCommandTypes.WarningMsg, false, false);
            ahr.DescriptionString = "Warning Messages In Orange";
            m_highLightRequests.Add(ahr);

            ahr = new AHighlightRequest(Color.FromKnownColor(KnownColor.Gold), Color.Empty, TraceCommandTypes.ExceptionBlock, false, false);
            ahr.DescriptionString = "Golden Exceptions";
            m_highLightRequests.Add(ahr);

            ahr = new AHighlightRequest(Color.FromKnownColor(KnownColor.Gold), Color.Empty, TraceCommandTypes.ExceptionData, false, false);
            ahr.DescriptionString = "Golden Exceptions (data)";
            m_highLightRequests.Add(ahr);

            ahr = new AHighlightRequest(Color.FromKnownColor(KnownColor.Gold), Color.Empty, TraceCommandTypes.ExcEnd, false, false);
            ahr.DescriptionString = "Golden Exceptions (end)";
            m_highLightRequests.Add(ahr);

            ahr = new AHighlightRequest(Color.FromKnownColor(KnownColor.Gold), Color.Empty, TraceCommandTypes.ExcStart, false, false);
            ahr.DescriptionString = "Golden Exceptions (start)";
            m_highLightRequests.Add(ahr);

            ahr = new AHighlightRequest(Color.FromKnownColor(KnownColor.Crimson), Color.FromKnownColor(KnownColor.FloralWhite), TraceCommandTypes.ErrorMsg, false, false);
            ahr.DescriptionString = "Crimson Errors";
            m_highLightRequests.Add(ahr);

            ahr = new AHighlightRequest(Color.FromKnownColor(KnownColor.Crimson), Color.FromKnownColor(KnownColor.FloralWhite), TraceCommandTypes.AssertionFailed, false, false);
            ahr.DescriptionString = "Crimson Assertions";
            m_highLightRequests.Add(ahr);

            RefreshListviewFromHighlightStore();
        }

        private void btnShowLoadSave_Click(object sender, EventArgs e) {
            tbcHighlightWizzard.SelectedTab = tabLoadSave;
            PopulateLoadSaveHighlightScreen();
        }

        private void cclCustomBackground_SelectedColorChangedEventHandler(object sender, Plisky.UIWinforms.Controls.ColorChangeEventArgs e) {
            if (rdoUseCustomScheme.Checked) {
                // We set the background to the foreground, confusing :)  This is because the CCL chooses the active color as the
                // foreground color
                txtSampleHighlight.BackColor = e.Foreground;
            }
        }

        private void cclPredefinedScheme_SelectedColorChangedEventHandler(object sender, Plisky.UIWinforms.Controls.ColorChangeEventArgs e) {
            if (rdoUseExistingScheme.Checked) {
                // This only sets the background if we are using the predefined scheme.
                if ((e.Background != Color.Empty) && (e.Background != txtSampleHighlight.BackColor)) {
                    txtSampleHighlight.BackColor = e.Background;
                }
            }
            if ((e.Foreground != Color.Empty) && (e.Foreground != txtSampleHighlight.ForeColor)) {
                txtSampleHighlight.ForeColor = e.Foreground;
            }

            btnAddHighlightIn.Enabled = true;
        }

        private void ChangeAddHighlightWizzardPage(AddHighlightWizPages pageSelector) {
            // Clear out the existing data.

            switch (pageSelector) {
                case AddHighlightWizPages.StartPage:
                    tbcHighlightWizzard.SelectedTab = tabExistingHighlights;
                    rdoAddTypeHighlight.Checked = false;
                    rdoTextHighlight.Checked = false;
                    txtHighlightFindMatch.Text = string.Empty;
                    chkNotMatch.Checked = chkUseCase.Checked = false;
                    rdoUseExistingScheme.Checked = true;
                    btnOk.Enabled = true;
                    break;

                case AddHighlightWizPages.AddHighlight:
                    tbcHighlightWizzard.SelectedTab = tabAddNewHighlight;
                    btnAddNewNext.Enabled = false;
                    btnOk.Enabled = false;
                    break;

                case AddHighlightWizPages.ChooseColor:
                    tbcHighlightWizzard.SelectedTab = tabSelectColor;
                    btnOk.Enabled = false;
                    break;

                default:
                    break;
            }
        }

        private void clbKnownEventTypes_SelectedIndexChanged(object sender, EventArgs e) {
            if ((clbKnownEventTypes.Enabled) && (clbKnownEventTypes.CheckedItems.Count > 0)) {
                txtSampleHighlight.Text = "Highlighting types: ";

                foreach (string s in clbKnownEventTypes.CheckedItems) {
                    txtSampleHighlight.Text += s + ",";
                }
                btnAddNewNext.Enabled = true;
            } else {
                btnAddNewNext.Enabled = false;
            }
        }

        private bool isSelectedEventTypeAlreadyHighlighted(TraceCommandTypes tct) {
            int matchPos = -1;
            string displayMessage = string.Format("The event type {0} is already highlighted.  Remove this highlight first.", tct.ToString());

            for (int i = 0; i < m_highLightRequests.Count; i++) {
                AHighlightRequest ahr = m_highLightRequests[i];
                if ((ahr.MakeMatchUsingType) && (ahr.matchTct == tct)) {
                    if (MessageBox.Show(displayMessage, "Confirm", MessageBoxButtons.YesNo) == DialogResult.No) {
                        matchPos = i;
                        break;
                    }
                }
            }
            if (matchPos >= 0) {
                m_highLightRequests.RemoveAt(matchPos);
                return true;
            } else {
                return false;
            }
        }

        private void lbxDiskHighlights_SelectedIndexChanged(object sender, EventArgs e) {
            if (lbxDiskHighlights.SelectedItem == null) { return; }

            AHighlightRequest loadable = (AHighlightRequest)lbxDiskHighlights.SelectedItem;
            foreach (AHighlightRequest ahr in lbxLoadedHighlights.Items) {
                if (loadable == ahr) {
                    btnLoadHighlight.Enabled = false;
                    break;
                }
            }

            foreach (AHighlightRequest ahr in lbxAutoLoads.Items) {
                if (loadable == ahr) {
                    btnAddAutoLoadHighlight.Enabled = false;
                }
            }
        }

        private void lvwTextFilterHighlights_SelectedIndexChanged(object sender, EventArgs e) {
            btnDeleteSelected.Enabled = true;
        }

        private void PopulateLoadSaveHighlightScreen() {
            lbxLoadedHighlights.Items.Clear();

            foreach (AHighlightRequest ahr in m_highLightRequests) {
                lbxLoadedHighlights.Items.Add(ahr);
            }

            RefreshDiskHighlightList();
            autoloadspopulated = true;

            if ((MexCore.TheCore.Options.HighlightDefaultProfileName != null) && (MexCore.TheCore.Options.HighlightDefaultProfileName.Length > 0)) {
                string[] autoLoads = MexCore.TheCore.Options.HighlightDefaultProfileName.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string s in autoLoads) {
                    AHighlightRequest ahr = AHighlightRequest.LoadHighlight(s);
                    if (ahr != null) {
                        lbxAutoLoads.Items.Add(ahr);
                    }
                }
            }
        }

        private void rdoAddTypeHighlight_CheckedChanged(object sender, EventArgs e) {
            SetAddNewHighlightSelection(rdoAddTypeHighlight.Checked);
        }

        private void rdoTextHighlight_CheckedChanged(object sender, EventArgs e) {
            txtHighlightFindMatch.Enabled = chkNotMatch.Enabled = chkUseCase.Enabled = rdoTextHighlight.Checked;
        }

        private void rdoUseExistingScheme_CheckedChanged(object sender, EventArgs e) {
            SetChooseColorHighlightSelection(rdoUseCustomScheme.Checked);
        }

        private void RefreshDiskHighlightList() {
            lbxDiskHighlights.Items.Clear();
            lbxDiskHighlights.Items.AddRange(AHighlightRequest.LoadKnownHighlights());
        }

        private void RefreshListviewFromHighlightStore() {
            lvwTextFilterHighlights.BeginUpdate();
            try {
                lvwTextFilterHighlights.Items.Clear();

                foreach (AHighlightRequest ahr in m_highLightRequests) {
                    ListViewItem lvi = new ListViewItem(ahr.DescriptionString);
                    if (ahr.BackColorSpecified) {
                        lvi.BackColor = ahr.BackgroundColor;
                    }
                    if (ahr.ForeColorSpecified) {
                        lvi.ForeColor = ahr.ForegroundColor;
                    }
                    lvwTextFilterHighlights.Items.Add(lvi);
                }
            } finally {
                lvwTextFilterHighlights.EndUpdate();
            }
            lvwTextFilterHighlights.Invalidate();

            btnRemoveAllHighlights.Enabled = (m_highLightRequests.Count > 0);
        }

        /// <summary>
        /// Sets the enabled state of the controls on the add new tab.  If we are adding a new type then the parameter should be
        /// true, but if we are adding a text based one it should be false
        /// </summary>
        /// <param name="addNewType">True if we are adding a type of control</param>
        private void SetAddNewHighlightSelection(bool addNewType) {
            clbKnownEventTypes.ClearSelected();

            // How lame is this!? Why on earth this does not exist on the control is beyond me.
            for (int i = 0; i < clbKnownEventTypes.CheckedIndices.Count; i++) {
                clbKnownEventTypes.SetItemChecked(clbKnownEventTypes.CheckedIndices[i], false);
            }

            txtHighlightFindMatch.Text = string.Empty;

            clbKnownEventTypes.Enabled = addNewType;
            txtHighlightFindMatch.Enabled = !addNewType;
            chkNotMatch.Enabled = !addNewType;
            chkUseCase.Enabled = !addNewType;
        }

        private void SetChooseColorHighlightSelection(bool selectionIsPredefined) {
            if (!selectionIsPredefined) {
                cclPredefinedScheme.ColorSet = Plisky.UIWinforms.Controls.ColorListSet.DefaultHighlights;
                cclPredefinedScheme.Enabled = true;
                cclCustomBackground.Visible = false;
            } else {
                cclCustomBackground.Visible = true;
                cclCustomBackground.Enabled = true;

                cclPredefinedScheme.ColorSet = Plisky.UIWinforms.Controls.ColorListSet.KnownColors;
                cclCustomBackground.ColorSet = Plisky.UIWinforms.Controls.ColorListSet.KnownColors;
            }
            btnAddHighlightIn.Enabled = true;
        }

        private void txtHighlightFindMatch_TextChanged(object sender, System.EventArgs e) {
            UpdateLabel();
            btnAddNewNext.Enabled = (txtHighlightFindMatch.Text.Length > 0);
        }

        private void txtHighlightName_TextChanged(object sender, EventArgs e) {
            if (txtHighlightName.Text != "<default>") {
                if (txtHighlightName.Text.IndexOfAny(Path.GetInvalidFileNameChars()) > 0) {
                    btnAddHighlightIn.Enabled = false;
                    txtHighlightName.BackColor = Color.IndianRed;
                } else {
                    btnAddHighlightIn.Enabled = true;
                    txtHighlightName.BackColor = SystemColors.Window;
                }
            }
        }

        private void UpdateLabel() {
            string labelText = "Highlight where the text ";
            if (chkNotMatch.Checked) {
                labelText += " does not match";
            } else {
                labelText += " matches";
            }
            labelText += " " + txtHighlightFindMatch.Text;

            txtSampleHighlight.Text = labelText;

            rdoTextHighlight.Text = "Add a highlight where the Text matches this: " + txtHighlightFindMatch.Text;

            labelText = string.Format("Where the line does not contain <{0}>", txtHighlightFindMatch.Text);
            chkNotMatch.Text = labelText;
            //lblDescription.Text = labelText;
        }
    }
}