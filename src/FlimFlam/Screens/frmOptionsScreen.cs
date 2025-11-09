
//using Plisky.Plumbing.Legacy;
//using Plisky.Plumbing.Listeners;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Windows.Forms;
using Plisky.Plumbing;

namespace Plisky.FlimFlam {
    /// <summary>
    /// Summary description for MexOptionsScreen.
    /// </summary>
    internal partial class frmMexOptionsScreen : System.Windows.Forms.Form {
        private Button btnOK;
        private Button btnCancel;
        private TabControl tabOptionsContainer;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabAdvancedOptions;
        private GroupBox groupBox4;
        private TextBox txtNormalisationMS;
        private CheckBox chkNormaliseRefresh;
        private Button btnSetODSAcl;
        private Label label6;
        private Label label5;
        private Label label4;
        private NumericUpDown nudPushbackLimitCount;
        private GroupBox grpUserMessageNotificationOptions;
        private RadioButton rdoShowUserNotificationsInLog;
        private RadioButton rdoShowUserNotificationsInStatusBar;
        private RadioButton rdoShowUserNotificationsasMessages;
        private RadioButton rdoDontProcessUserNotifications;
        private GroupBox groupBox2;
        private CheckBox chkMatchingNamePurgeAlsoClearsPartials;
        private CheckBox chkBeautifyOutput;
        private CheckBox chkWotTimedViewToo;
        private CheckBox chkDisplayGlobalIndexInMainView;
        private CheckBox chkSelectingProcessSelectsProcessView;
        private CheckBox chkAutoScroll;
        private CheckBox chkAutoRefresh;
        private CheckBox chkAllowCancelOperations;
        private CheckBox chkLeaveMatchingPidsInNonTracedToo;
        private CheckBox chkImportMatchingPIDODSIntoEvents;
        private CheckBox chkAutoSelectProcessIfNoneSelected;
        private CheckBox chkRespectFilter;
        private CheckBox chkRecycleProcessWhenNameMatches;
        private GroupBox groupBox5;
        private Label label8;
        private Label label7;
        private TextBox txtPortBinding;
        private TextBox txtIPBinding;
        private GroupBox groupBox1;
        private CheckBox chkXRefCheckAssertions;
        private CheckBox chkXRefResourceMessages;
        private CheckBox chkXRefVerbLogs;
        private CheckBox chkXRefMiniLogs;
        private CheckBox chkXRefLogs;
        private CheckBox chkXRefExceptions;
        private CheckBox chkXRefErrors;
        private Label label3;
        private CheckBox chkXRefWarnings;
        private CheckBox chkXRefAppInitialises;
        private TabPage tabPage4;
        private Label label1;
        private TextBox txtFilterAndHighlightDir;
        private Button btnBrowseForFilterDir;
        private CheckBox chkRelocateOnChange;
        private CheckBox chkHighlightCrossProcesses;
        private CheckBox chkTimingsViewIgnoresThreads;
        private GroupBox grpThreadDisplayOptions;
        private RadioButton rdoThreadShowFullInfo;
        private RadioButton rdoThreadShowNetId;
        private RadioButton rdoThreadShowOSId;
        private RadioButton rdoThreadShowDefault;
        private CheckBox chkAllowInternalMessageDisplays;
        private Label label2;
        private TextBox txtUIRefreshFrequency;
        private Label label10;
        private TextBox txtLongRunningOps;
        private Label label9;
        private TextBox txtUserLogSize;
        private TabPage tabTexSettings;
        private CheckBox chkExpandAssertions;
        private Button btnReadEnvironment;
        private Button btnSetEnvironment;
        private CheckBox chkUseHighPerf;
        private GroupBox groupBox7;
        private CheckBox chkMakeTcpInteractive;
        private CheckBox chkRemoveAll;
        private TextBox txtIPInit;
        private TextBox txtPortInit;
        private CheckBox chkAddTexListener;
        private GroupBox grpTraceLevel;
        private RadioButton rdoWarningLevel;
        private RadioButton rdoVerboseLevel;
        private RadioButton rdoErrorLevel;
        private RadioButton rdoTraceInfo;
        private RadioButton rdoTraceOff;
        private TextBox txtGeneratedString;
        private Label lblHelp;
        private CheckBox chkEnableEnhancements;
        private GroupBox groupBox9;
        private Label label14;
        private Label label13;
        private ListBox lbxNameMappings;
        private TextBox textBox2;
        private TextBox textBox1;
        private Button btnAddMapping;
        private Button btnDeleteMapping;
        private CheckBox chkEnableBacktrace;
        private CheckBox chkAddStackInfo;
        private Button btnAddNewListener;
        private Button btnRemoveSelected;
        private ListBox lbxAddTCPListeners;
        private Label lblErrorMsg;
        private Label label11;
        private Label label12;
        private Button btnReleaseDefaults;
        private Button btnDevelopmentDefaults;
        private Label lblRefreshWarning;
        private CheckBox chkUseRenderNameNotPID;
        private CheckBox chkRemoveDupes;
        private CheckBox chkRemoveDupesOnDisplay;
        private CheckBox chkFilterDefaultIncludesClasses;
        private CheckBox chkFilterDefaultIncludeModules;
        private CheckBox chkFilterDefaultIncludesThreads;
        private CheckBox chkFilterDefaultIncludesLocations;
        private LinkLabel llbRelatedContent;
        private GroupBox groupBox3;
        private CheckBox chkActivateImportLogging;
        private CheckBox chkIncludeFileContents;
        private CheckBox chkInlucdeTCPContents;
        private CheckBox chkImportLoggingInlcudedsODS;
        private ListBox lbxImportProcessPaths;
        private Button btnRemovePath;
        private Button btnAddPath;
        private TextBox txtAdditionalPath;
        private Label label15;
        private TextBox textBox4;
        private TextBox textBox3;
        private Label label16;
        private ErrorProvider errorProvider1;
        private System.ComponentModel.IContainer components;

        internal frmMexOptionsScreen() {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //

            // Look up the list of filters so that we can enable the dropdown of filters.
            RefreshTexTabFromEnvironmentVariable();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing) {
            if (disposing) {
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMexOptionsScreen));
            btnOK = new Button();
            btnCancel = new Button();
            tabOptionsContainer = new TabControl();
            tabPage1 = new TabPage();
            lblRefreshWarning = new Label();
            label10 = new Label();
            txtLongRunningOps = new TextBox();
            label9 = new Label();
            txtUserLogSize = new TextBox();
            label2 = new Label();
            txtUIRefreshFrequency = new TextBox();
            grpUserMessageNotificationOptions = new GroupBox();
            rdoShowUserNotificationsInLog = new RadioButton();
            rdoShowUserNotificationsInStatusBar = new RadioButton();
            rdoShowUserNotificationsasMessages = new RadioButton();
            rdoDontProcessUserNotifications = new RadioButton();
            tabPage2 = new TabPage();
            grpThreadDisplayOptions = new GroupBox();
            rdoThreadShowDefault = new RadioButton();
            rdoThreadShowFullInfo = new RadioButton();
            rdoThreadShowNetId = new RadioButton();
            rdoThreadShowOSId = new RadioButton();
            groupBox2 = new GroupBox();
            chkUseRenderNameNotPID = new CheckBox();
            chkEnableBacktrace = new CheckBox();
            chkHighlightCrossProcesses = new CheckBox();
            chkMatchingNamePurgeAlsoClearsPartials = new CheckBox();
            chkBeautifyOutput = new CheckBox();
            chkWotTimedViewToo = new CheckBox();
            chkDisplayGlobalIndexInMainView = new CheckBox();
            chkSelectingProcessSelectsProcessView = new CheckBox();
            chkAutoScroll = new CheckBox();
            chkAutoRefresh = new CheckBox();
            chkAllowCancelOperations = new CheckBox();
            chkLeaveMatchingPidsInNonTracedToo = new CheckBox();
            chkImportMatchingPIDODSIntoEvents = new CheckBox();
            chkAutoSelectProcessIfNoneSelected = new CheckBox();
            chkRespectFilter = new CheckBox();
            chkRecycleProcessWhenNameMatches = new CheckBox();
            tabPage3 = new TabPage();
            label16 = new Label();
            label15 = new Label();
            textBox4 = new TextBox();
            textBox3 = new TextBox();
            btnRemovePath = new Button();
            btnAddPath = new Button();
            txtAdditionalPath = new TextBox();
            lbxImportProcessPaths = new ListBox();
            chkRemoveDupesOnDisplay = new CheckBox();
            chkRemoveDupes = new CheckBox();
            groupBox5 = new GroupBox();
            label8 = new Label();
            label7 = new Label();
            txtPortBinding = new TextBox();
            txtIPBinding = new TextBox();
            groupBox1 = new GroupBox();
            chkXRefCheckAssertions = new CheckBox();
            chkXRefResourceMessages = new CheckBox();
            chkXRefVerbLogs = new CheckBox();
            chkXRefMiniLogs = new CheckBox();
            chkXRefLogs = new CheckBox();
            chkXRefExceptions = new CheckBox();
            chkXRefErrors = new CheckBox();
            label3 = new Label();
            chkXRefWarnings = new CheckBox();
            chkXRefAppInitialises = new CheckBox();
            tabAdvancedOptions = new TabPage();
            groupBox3 = new GroupBox();
            chkIncludeFileContents = new CheckBox();
            chkInlucdeTCPContents = new CheckBox();
            chkImportLoggingInlcudedsODS = new CheckBox();
            chkActivateImportLogging = new CheckBox();
            groupBox9 = new GroupBox();
            btnDeleteMapping = new Button();
            btnAddMapping = new Button();
            label14 = new Label();
            label13 = new Label();
            lbxNameMappings = new ListBox();
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            chkTimingsViewIgnoresThreads = new CheckBox();
            groupBox4 = new GroupBox();
            txtNormalisationMS = new TextBox();
            chkNormaliseRefresh = new CheckBox();
            btnSetODSAcl = new Button();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            nudPushbackLimitCount = new NumericUpDown();
            tabPage4 = new TabPage();
            chkFilterDefaultIncludesClasses = new CheckBox();
            chkFilterDefaultIncludeModules = new CheckBox();
            chkFilterDefaultIncludesThreads = new CheckBox();
            chkFilterDefaultIncludesLocations = new CheckBox();
            chkAllowInternalMessageDisplays = new CheckBox();
            chkRelocateOnChange = new CheckBox();
            btnBrowseForFilterDir = new Button();
            label1 = new Label();
            txtFilterAndHighlightDir = new TextBox();
            tabTexSettings = new TabPage();
            btnReleaseDefaults = new Button();
            btnDevelopmentDefaults = new Button();
            chkAddStackInfo = new CheckBox();
            chkEnableEnhancements = new CheckBox();
            chkExpandAssertions = new CheckBox();
            btnReadEnvironment = new Button();
            btnSetEnvironment = new Button();
            chkUseHighPerf = new CheckBox();
            groupBox7 = new GroupBox();
            label12 = new Label();
            label11 = new Label();
            lblErrorMsg = new Label();
            btnAddNewListener = new Button();
            btnRemoveSelected = new Button();
            lbxAddTCPListeners = new ListBox();
            chkMakeTcpInteractive = new CheckBox();
            chkRemoveAll = new CheckBox();
            txtIPInit = new TextBox();
            txtPortInit = new TextBox();
            chkAddTexListener = new CheckBox();
            grpTraceLevel = new GroupBox();
            rdoWarningLevel = new RadioButton();
            rdoVerboseLevel = new RadioButton();
            rdoErrorLevel = new RadioButton();
            rdoTraceInfo = new RadioButton();
            rdoTraceOff = new RadioButton();
            txtGeneratedString = new TextBox();
            lblHelp = new Label();
            llbRelatedContent = new LinkLabel();
            errorProvider1 = new ErrorProvider(components);
            tabOptionsContainer.SuspendLayout();
            tabPage1.SuspendLayout();
            grpUserMessageNotificationOptions.SuspendLayout();
            tabPage2.SuspendLayout();
            grpThreadDisplayOptions.SuspendLayout();
            groupBox2.SuspendLayout();
            tabPage3.SuspendLayout();
            groupBox5.SuspendLayout();
            groupBox1.SuspendLayout();
            tabAdvancedOptions.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox9.SuspendLayout();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudPushbackLimitCount).BeginInit();
            tabPage4.SuspendLayout();
            tabTexSettings.SuspendLayout();
            groupBox7.SuspendLayout();
            grpTraceLevel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            SuspendLayout();
            // 
            // btnOK
            // 
            btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOK.DialogResult = DialogResult.OK;
            btnOK.Location = new System.Drawing.Point(655, 446);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(75, 27);
            btnOK.TabIndex = 6;
            btnOK.Text = "&Ok";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += OK_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(574, 446);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 27);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "C&ancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // tabOptionsContainer
            // 
            tabOptionsContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabOptionsContainer.Controls.Add(tabPage1);
            tabOptionsContainer.Controls.Add(tabPage2);
            tabOptionsContainer.Controls.Add(tabPage3);
            tabOptionsContainer.Controls.Add(tabAdvancedOptions);
            tabOptionsContainer.Controls.Add(tabPage4);
            tabOptionsContainer.Controls.Add(tabTexSettings);
            tabOptionsContainer.Location = new System.Drawing.Point(2, 3);
            tabOptionsContainer.Name = "tabOptionsContainer";
            tabOptionsContainer.SelectedIndex = 0;
            tabOptionsContainer.Size = new System.Drawing.Size(731, 375);
            tabOptionsContainer.TabIndex = 17;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(lblRefreshWarning);
            tabPage1.Controls.Add(label10);
            tabPage1.Controls.Add(txtLongRunningOps);
            tabPage1.Controls.Add(label9);
            tabPage1.Controls.Add(txtUserLogSize);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(txtUIRefreshFrequency);
            tabPage1.Controls.Add(grpUserMessageNotificationOptions);
            tabPage1.Location = new System.Drawing.Point(4, 22);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new System.Drawing.Size(723, 349);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "User Preferences";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // lblRefreshWarning
            // 
            lblRefreshWarning.AutoSize = true;
            lblRefreshWarning.Location = new System.Drawing.Point(6, 97);
            lblRefreshWarning.Name = "lblRefreshWarning";
            lblRefreshWarning.Size = new System.Drawing.Size(48, 13);
            lblRefreshWarning.TabIndex = 20;
            lblRefreshWarning.Text = "label15";
            lblRefreshWarning.Visible = false;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(501, 62);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(167, 13);
            label10.TabIndex = 19;
            label10.Text = "No seconds for long running";
            // 
            // txtLongRunningOps
            // 
            txtLongRunningOps.Location = new System.Drawing.Point(444, 59);
            txtLongRunningOps.Name = "txtLongRunningOps";
            txtLongRunningOps.Size = new System.Drawing.Size(51, 21);
            txtLongRunningOps.TabIndex = 18;
            txtLongRunningOps.Text = "45";
            txtLongRunningOps.MouseHover += Generic_MouseHover;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(336, 62);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(85, 13);
            label9.TabIndex = 17;
            label9.Text = "User Log Size";
            // 
            // txtUserLogSize
            // 
            txtUserLogSize.Location = new System.Drawing.Point(271, 59);
            txtUserLogSize.Name = "txtUserLogSize";
            txtUserLogSize.Size = new System.Drawing.Size(59, 21);
            txtUserLogSize.TabIndex = 16;
            txtUserLogSize.Text = "150";
            txtUserLogSize.MouseHover += Generic_MouseHover;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(67, 64);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(146, 13);
            label2.TabIndex = 14;
            label2.Text = "Frequency of UI Refresh";
            // 
            // txtUIRefreshFrequency
            // 
            txtUIRefreshFrequency.Location = new System.Drawing.Point(10, 59);
            txtUIRefreshFrequency.Name = "txtUIRefreshFrequency";
            txtUIRefreshFrequency.Size = new System.Drawing.Size(51, 21);
            txtUIRefreshFrequency.TabIndex = 13;
            txtUIRefreshFrequency.Text = "1";
            txtUIRefreshFrequency.TextChanged += UIRefreshFrequency_TextChanged;
            txtUIRefreshFrequency.MouseHover += Generic_MouseHover;
            // 
            // grpUserMessageNotificationOptions
            // 
            grpUserMessageNotificationOptions.Controls.Add(rdoShowUserNotificationsInLog);
            grpUserMessageNotificationOptions.Controls.Add(rdoShowUserNotificationsInStatusBar);
            grpUserMessageNotificationOptions.Controls.Add(rdoShowUserNotificationsasMessages);
            grpUserMessageNotificationOptions.Controls.Add(rdoDontProcessUserNotifications);
            grpUserMessageNotificationOptions.Enabled = false;
            grpUserMessageNotificationOptions.Location = new System.Drawing.Point(9, 6);
            grpUserMessageNotificationOptions.Name = "grpUserMessageNotificationOptions";
            grpUserMessageNotificationOptions.Size = new System.Drawing.Size(702, 47);
            grpUserMessageNotificationOptions.TabIndex = 12;
            grpUserMessageNotificationOptions.TabStop = false;
            grpUserMessageNotificationOptions.Text = "Recieve User Messages";
            grpUserMessageNotificationOptions.Visible = false;
            grpUserMessageNotificationOptions.MouseHover += Generic_MouseHover;
            // 
            // rdoShowUserNotificationsInLog
            // 
            rdoShowUserNotificationsInLog.AutoSize = true;
            rdoShowUserNotificationsInLog.Location = new System.Drawing.Point(454, 19);
            rdoShowUserNotificationsInLog.Name = "rdoShowUserNotificationsInLog";
            rdoShowUserNotificationsInLog.Size = new System.Drawing.Size(108, 17);
            rdoShowUserNotificationsInLog.TabIndex = 5;
            rdoShowUserNotificationsInLog.Text = "In the log only";
            rdoShowUserNotificationsInLog.UseVisualStyleBackColor = true;
            // 
            // rdoShowUserNotificationsInStatusBar
            // 
            rdoShowUserNotificationsInStatusBar.AutoSize = true;
            rdoShowUserNotificationsInStatusBar.Location = new System.Drawing.Point(301, 20);
            rdoShowUserNotificationsInStatusBar.Name = "rdoShowUserNotificationsInStatusBar";
            rdoShowUserNotificationsInStatusBar.Size = new System.Drawing.Size(120, 17);
            rdoShowUserNotificationsInStatusBar.TabIndex = 2;
            rdoShowUserNotificationsInStatusBar.Text = "In the status bar";
            rdoShowUserNotificationsInStatusBar.UseVisualStyleBackColor = true;
            // 
            // rdoShowUserNotificationsasMessages
            // 
            rdoShowUserNotificationsasMessages.AutoSize = true;
            rdoShowUserNotificationsasMessages.Checked = true;
            rdoShowUserNotificationsasMessages.Location = new System.Drawing.Point(128, 20);
            rdoShowUserNotificationsasMessages.Name = "rdoShowUserNotificationsasMessages";
            rdoShowUserNotificationsasMessages.Size = new System.Drawing.Size(90, 17);
            rdoShowUserNotificationsasMessages.TabIndex = 1;
            rdoShowUserNotificationsasMessages.TabStop = true;
            rdoShowUserNotificationsasMessages.Text = "Interactivly";
            rdoShowUserNotificationsasMessages.UseVisualStyleBackColor = true;
            // 
            // rdoDontProcessUserNotifications
            // 
            rdoDontProcessUserNotifications.AutoSize = true;
            rdoDontProcessUserNotifications.Location = new System.Drawing.Point(7, 19);
            rdoDontProcessUserNotifications.Name = "rdoDontProcessUserNotifications";
            rdoDontProcessUserNotifications.Size = new System.Drawing.Size(54, 17);
            rdoDontProcessUserNotifications.TabIndex = 0;
            rdoDontProcessUserNotifications.Text = "None";
            rdoDontProcessUserNotifications.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(grpThreadDisplayOptions);
            tabPage2.Controls.Add(groupBox2);
            tabPage2.Location = new System.Drawing.Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new System.Drawing.Size(723, 347);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Mex Behaviour";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // grpThreadDisplayOptions
            // 
            grpThreadDisplayOptions.Controls.Add(rdoThreadShowDefault);
            grpThreadDisplayOptions.Controls.Add(rdoThreadShowFullInfo);
            grpThreadDisplayOptions.Controls.Add(rdoThreadShowNetId);
            grpThreadDisplayOptions.Controls.Add(rdoThreadShowOSId);
            grpThreadDisplayOptions.Location = new System.Drawing.Point(7, 269);
            grpThreadDisplayOptions.Name = "grpThreadDisplayOptions";
            grpThreadDisplayOptions.Size = new System.Drawing.Size(710, 74);
            grpThreadDisplayOptions.TabIndex = 21;
            grpThreadDisplayOptions.TabStop = false;
            grpThreadDisplayOptions.Text = "Thread Display";
            grpThreadDisplayOptions.MouseHover += Generic_MouseHover;
            // 
            // rdoThreadShowDefault
            // 
            rdoThreadShowDefault.AutoSize = true;
            rdoThreadShowDefault.Location = new System.Drawing.Point(15, 20);
            rdoThreadShowDefault.Name = "rdoThreadShowDefault";
            rdoThreadShowDefault.Size = new System.Drawing.Size(347, 17);
            rdoThreadShowDefault.TabIndex = 4;
            rdoThreadShowDefault.TabStop = true;
            rdoThreadShowDefault.Text = "Default display - net threadname and OS thread identity";
            rdoThreadShowDefault.UseVisualStyleBackColor = true;
            // 
            // rdoThreadShowFullInfo
            // 
            rdoThreadShowFullInfo.AutoSize = true;
            rdoThreadShowFullInfo.Location = new System.Drawing.Point(279, 51);
            rdoThreadShowFullInfo.Name = "rdoThreadShowFullInfo";
            rdoThreadShowFullInfo.Size = new System.Drawing.Size(144, 17);
            rdoThreadShowFullInfo.TabIndex = 3;
            rdoThreadShowFullInfo.TabStop = true;
            rdoThreadShowFullInfo.Text = "show full information";
            rdoThreadShowFullInfo.UseVisualStyleBackColor = true;
            // 
            // rdoThreadShowNetId
            // 
            rdoThreadShowNetId.AutoSize = true;
            rdoThreadShowNetId.Location = new System.Drawing.Point(134, 51);
            rdoThreadShowNetId.Name = "rdoThreadShowNetId";
            rdoThreadShowNetId.Size = new System.Drawing.Size(102, 17);
            rdoThreadShowNetId.TabIndex = 1;
            rdoThreadShowNetId.TabStop = true;
            rdoThreadShowNetId.Text = ".net thread id";
            rdoThreadShowNetId.UseVisualStyleBackColor = true;
            // 
            // rdoThreadShowOSId
            // 
            rdoThreadShowOSId.AutoSize = true;
            rdoThreadShowOSId.Location = new System.Drawing.Point(16, 51);
            rdoThreadShowOSId.Name = "rdoThreadShowOSId";
            rdoThreadShowOSId.Size = new System.Drawing.Size(102, 17);
            rdoThreadShowOSId.TabIndex = 0;
            rdoThreadShowOSId.TabStop = true;
            rdoThreadShowOSId.Text = "OS Thread Id";
            rdoThreadShowOSId.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(chkUseRenderNameNotPID);
            groupBox2.Controls.Add(chkEnableBacktrace);
            groupBox2.Controls.Add(chkHighlightCrossProcesses);
            groupBox2.Controls.Add(chkMatchingNamePurgeAlsoClearsPartials);
            groupBox2.Controls.Add(chkBeautifyOutput);
            groupBox2.Controls.Add(chkWotTimedViewToo);
            groupBox2.Controls.Add(chkDisplayGlobalIndexInMainView);
            groupBox2.Controls.Add(chkSelectingProcessSelectsProcessView);
            groupBox2.Controls.Add(chkAutoScroll);
            groupBox2.Controls.Add(chkAutoRefresh);
            groupBox2.Controls.Add(chkAllowCancelOperations);
            groupBox2.Controls.Add(chkLeaveMatchingPidsInNonTracedToo);
            groupBox2.Controls.Add(chkImportMatchingPIDODSIntoEvents);
            groupBox2.Controls.Add(chkAutoSelectProcessIfNoneSelected);
            groupBox2.Controls.Add(chkRespectFilter);
            groupBox2.Controls.Add(chkRecycleProcessWhenNameMatches);
            groupBox2.Location = new System.Drawing.Point(6, 6);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(710, 257);
            groupBox2.TabIndex = 9;
            groupBox2.TabStop = false;
            groupBox2.Text = " Mex Behaviour ";
            // 
            // chkUseRenderNameNotPID
            // 
            chkUseRenderNameNotPID.AutoSize = true;
            chkUseRenderNameNotPID.Location = new System.Drawing.Point(412, 223);
            chkUseRenderNameNotPID.Name = "chkUseRenderNameNotPID";
            chkUseRenderNameNotPID.Size = new System.Drawing.Size(212, 17);
            chkUseRenderNameNotPID.TabIndex = 20;
            chkUseRenderNameNotPID.Text = "Prefer to use a processes name.";
            chkUseRenderNameNotPID.UseVisualStyleBackColor = true;
            chkUseRenderNameNotPID.MouseHover += Generic_MouseHover;
            // 
            // chkEnableBacktrace
            // 
            chkEnableBacktrace.AutoSize = true;
            chkEnableBacktrace.Location = new System.Drawing.Point(15, 143);
            chkEnableBacktrace.Name = "chkEnableBacktrace";
            chkEnableBacktrace.Size = new System.Drawing.Size(214, 17);
            chkEnableBacktrace.TabIndex = 19;
            chkEnableBacktrace.Text = "Backtrace Extended Details View";
            chkEnableBacktrace.UseVisualStyleBackColor = true;
            chkEnableBacktrace.MouseHover += Generic_MouseHover;
            // 
            // chkHighlightCrossProcesses
            // 
            chkHighlightCrossProcesses.AutoSize = true;
            chkHighlightCrossProcesses.Location = new System.Drawing.Point(16, 74);
            chkHighlightCrossProcesses.Name = "chkHighlightCrossProcesses";
            chkHighlightCrossProcesses.Size = new System.Drawing.Size(191, 17);
            chkHighlightCrossProcesses.TabIndex = 18;
            chkHighlightCrossProcesses.Text = "Cross Process View Highlight";
            chkHighlightCrossProcesses.UseVisualStyleBackColor = true;
            chkHighlightCrossProcesses.MouseHover += Generic_MouseHover;
            // 
            // chkMatchingNamePurgeAlsoClearsPartials
            // 
            chkMatchingNamePurgeAlsoClearsPartials.AutoSize = true;
            chkMatchingNamePurgeAlsoClearsPartials.Location = new System.Drawing.Point(16, 53);
            chkMatchingNamePurgeAlsoClearsPartials.Name = "chkMatchingNamePurgeAlsoClearsPartials";
            chkMatchingNamePurgeAlsoClearsPartials.Size = new System.Drawing.Size(259, 17);
            chkMatchingNamePurgeAlsoClearsPartials.TabIndex = 15;
            chkMatchingNamePurgeAlsoClearsPartials.Text = "Partial Purge Elimiates Partial Copies too";
            chkMatchingNamePurgeAlsoClearsPartials.UseVisualStyleBackColor = true;
            chkMatchingNamePurgeAlsoClearsPartials.MouseHover += Generic_MouseHover;
            // 
            // chkBeautifyOutput
            // 
            chkBeautifyOutput.AutoSize = true;
            chkBeautifyOutput.Location = new System.Drawing.Point(412, 96);
            chkBeautifyOutput.Name = "chkBeautifyOutput";
            chkBeautifyOutput.Size = new System.Drawing.Size(176, 17);
            chkBeautifyOutput.TabIndex = 14;
            chkBeautifyOutput.Text = "Beautify Output Of Strings";
            chkBeautifyOutput.UseVisualStyleBackColor = true;
            chkBeautifyOutput.MouseHover += Generic_MouseHover;
            // 
            // chkWotTimedViewToo
            // 
            chkWotTimedViewToo.AutoSize = true;
            chkWotTimedViewToo.Location = new System.Drawing.Point(175, 97);
            chkWotTimedViewToo.Name = "chkWotTimedViewToo";
            chkWotTimedViewToo.Size = new System.Drawing.Size(134, 17);
            chkWotTimedViewToo.TabIndex = 12;
            chkWotTimedViewToo.Text = "Even in timed view";
            chkWotTimedViewToo.UseVisualStyleBackColor = true;
            chkWotTimedViewToo.MouseHover += Generic_MouseHover;
            // 
            // chkDisplayGlobalIndexInMainView
            // 
            chkDisplayGlobalIndexInMainView.AutoSize = true;
            chkDisplayGlobalIndexInMainView.Location = new System.Drawing.Point(412, 200);
            chkDisplayGlobalIndexInMainView.Name = "chkDisplayGlobalIndexInMainView";
            chkDisplayGlobalIndexInMainView.Size = new System.Drawing.Size(229, 17);
            chkDisplayGlobalIndexInMainView.TabIndex = 11;
            chkDisplayGlobalIndexInMainView.Text = "Show Global Index In Process View";
            chkDisplayGlobalIndexInMainView.UseVisualStyleBackColor = true;
            chkDisplayGlobalIndexInMainView.MouseHover += Generic_MouseHover;
            // 
            // chkSelectingProcessSelectsProcessView
            // 
            chkSelectingProcessSelectsProcessView.AutoSize = true;
            chkSelectingProcessSelectsProcessView.Location = new System.Drawing.Point(15, 187);
            chkSelectingProcessSelectsProcessView.Name = "chkSelectingProcessSelectsProcessView";
            chkSelectingProcessSelectsProcessView.Size = new System.Drawing.Size(370, 17);
            chkSelectingProcessSelectsProcessView.TabIndex = 9;
            chkSelectingProcessSelectsProcessView.Text = "When selecting a new process change view to process view.";
            chkSelectingProcessSelectsProcessView.UseVisualStyleBackColor = true;
            chkSelectingProcessSelectsProcessView.MouseHover += Generic_MouseHover;
            // 
            // chkAutoScroll
            // 
            chkAutoScroll.AutoSize = true;
            chkAutoScroll.Location = new System.Drawing.Point(412, 146);
            chkAutoScroll.Name = "chkAutoScroll";
            chkAutoScroll.Size = new System.Drawing.Size(139, 17);
            chkAutoScroll.TabIndex = 8;
            chkAutoScroll.Text = "Automatically Scroll";
            chkAutoScroll.UseVisualStyleBackColor = true;
            chkAutoScroll.MouseHover += Generic_MouseHover;
            // 
            // chkAutoRefresh
            // 
            chkAutoRefresh.AutoSize = true;
            chkAutoRefresh.Location = new System.Drawing.Point(412, 123);
            chkAutoRefresh.Name = "chkAutoRefresh";
            chkAutoRefresh.Size = new System.Drawing.Size(148, 17);
            chkAutoRefresh.TabIndex = 7;
            chkAutoRefresh.Text = "Automatically refresh";
            chkAutoRefresh.UseVisualStyleBackColor = true;
            chkAutoRefresh.MouseHover += Generic_MouseHover;
            // 
            // chkAllowCancelOperations
            // 
            chkAllowCancelOperations.AutoSize = true;
            chkAllowCancelOperations.Enabled = false;
            chkAllowCancelOperations.Location = new System.Drawing.Point(15, 120);
            chkAllowCancelOperations.Name = "chkAllowCancelOperations";
            chkAllowCancelOperations.Size = new System.Drawing.Size(193, 17);
            chkAllowCancelOperations.TabIndex = 5;
            chkAllowCancelOperations.Text = "Support Refresh Cancellation";
            chkAllowCancelOperations.UseVisualStyleBackColor = true;
            chkAllowCancelOperations.Visible = false;
            chkAllowCancelOperations.MouseHover += Generic_MouseHover;
            // 
            // chkLeaveMatchingPidsInNonTracedToo
            // 
            chkLeaveMatchingPidsInNonTracedToo.AutoSize = true;
            chkLeaveMatchingPidsInNonTracedToo.Location = new System.Drawing.Point(440, 51);
            chkLeaveMatchingPidsInNonTracedToo.Name = "chkLeaveMatchingPidsInNonTracedToo";
            chkLeaveMatchingPidsInNonTracedToo.Size = new System.Drawing.Size(214, 17);
            chkLeaveMatchingPidsInNonTracedToo.TabIndex = 4;
            chkLeaveMatchingPidsInNonTracedToo.Text = "When this occurs, copy the entry";
            chkLeaveMatchingPidsInNonTracedToo.UseVisualStyleBackColor = true;
            chkLeaveMatchingPidsInNonTracedToo.MouseHover += Generic_MouseHover;
            // 
            // chkImportMatchingPIDODSIntoEvents
            // 
            chkImportMatchingPIDODSIntoEvents.AutoSize = true;
            chkImportMatchingPIDODSIntoEvents.Location = new System.Drawing.Point(412, 28);
            chkImportMatchingPIDODSIntoEvents.Name = "chkImportMatchingPIDODSIntoEvents";
            chkImportMatchingPIDODSIntoEvents.Size = new System.Drawing.Size(280, 17);
            chkImportMatchingPIDODSIntoEvents.TabIndex = 3;
            chkImportMatchingPIDODSIntoEvents.Text = "Messages from same PID join Tex Messages";
            chkImportMatchingPIDODSIntoEvents.UseVisualStyleBackColor = true;
            chkImportMatchingPIDODSIntoEvents.MouseHover += Generic_MouseHover;
            // 
            // chkAutoSelectProcessIfNoneSelected
            // 
            chkAutoSelectProcessIfNoneSelected.AutoSize = true;
            chkAutoSelectProcessIfNoneSelected.Location = new System.Drawing.Point(15, 210);
            chkAutoSelectProcessIfNoneSelected.Name = "chkAutoSelectProcessIfNoneSelected";
            chkAutoSelectProcessIfNoneSelected.Size = new System.Drawing.Size(268, 17);
            chkAutoSelectProcessIfNoneSelected.TabIndex = 2;
            chkAutoSelectProcessIfNoneSelected.Text = "Auto Select Process View For First Process";
            chkAutoSelectProcessIfNoneSelected.UseVisualStyleBackColor = true;
            chkAutoSelectProcessIfNoneSelected.MouseHover += Generic_MouseHover;
            // 
            // chkRespectFilter
            // 
            chkRespectFilter.AutoSize = true;
            chkRespectFilter.Location = new System.Drawing.Point(16, 97);
            chkRespectFilter.Name = "chkRespectFilter";
            chkRespectFilter.Size = new System.Drawing.Size(153, 17);
            chkRespectFilter.TabIndex = 1;
            chkRespectFilter.Text = "Respect Filter Settings";
            chkRespectFilter.UseVisualStyleBackColor = true;
            chkRespectFilter.MouseHover += Generic_MouseHover;
            // 
            // chkRecycleProcessWhenNameMatches
            // 
            chkRecycleProcessWhenNameMatches.AutoSize = true;
            chkRecycleProcessWhenNameMatches.Location = new System.Drawing.Point(16, 30);
            chkRecycleProcessWhenNameMatches.Name = "chkRecycleProcessWhenNameMatches";
            chkRecycleProcessWhenNameMatches.Size = new System.Drawing.Size(283, 17);
            chkRecycleProcessWhenNameMatches.TabIndex = 0;
            chkRecycleProcessWhenNameMatches.Text = "On initialise partially purge matching process";
            chkRecycleProcessWhenNameMatches.UseVisualStyleBackColor = true;
            chkRecycleProcessWhenNameMatches.MouseEnter += RecycleProcessWhenNameMatches_MouseEnter;
            chkRecycleProcessWhenNameMatches.MouseHover += Generic_MouseHover;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(label16);
            tabPage3.Controls.Add(label15);
            tabPage3.Controls.Add(textBox4);
            tabPage3.Controls.Add(textBox3);
            tabPage3.Controls.Add(btnRemovePath);
            tabPage3.Controls.Add(btnAddPath);
            tabPage3.Controls.Add(txtAdditionalPath);
            tabPage3.Controls.Add(lbxImportProcessPaths);
            tabPage3.Controls.Add(chkRemoveDupesOnDisplay);
            tabPage3.Controls.Add(chkRemoveDupes);
            tabPage3.Controls.Add(groupBox5);
            tabPage3.Controls.Add(groupBox1);
            tabPage3.Location = new System.Drawing.Point(4, 22);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new System.Drawing.Size(723, 349);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Importer";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new System.Drawing.Point(497, 158);
            label16.Name = "label16";
            label16.Size = new System.Drawing.Size(121, 13);
            label16.TabIndex = 28;
            label16.Text = "Google Subscription";
            label16.Visible = false;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new System.Drawing.Point(346, 158);
            label15.Name = "label15";
            label15.Size = new System.Drawing.Size(91, 13);
            label15.TabIndex = 27;
            label15.Text = "Google Project";
            label15.Visible = false;
            // 
            // textBox4
            // 
            textBox4.Location = new System.Drawing.Point(497, 174);
            textBox4.Name = "textBox4";
            textBox4.Size = new System.Drawing.Size(143, 21);
            textBox4.TabIndex = 26;
            textBox4.Visible = false;
            // 
            // textBox3
            // 
            textBox3.Location = new System.Drawing.Point(345, 174);
            textBox3.Name = "textBox3";
            textBox3.Size = new System.Drawing.Size(146, 21);
            textBox3.TabIndex = 25;
            textBox3.Visible = false;
            // 
            // btnRemovePath
            // 
            btnRemovePath.Location = new System.Drawing.Point(649, 246);
            btnRemovePath.Name = "btnRemovePath";
            btnRemovePath.Size = new System.Drawing.Size(53, 23);
            btnRemovePath.TabIndex = 24;
            btnRemovePath.Text = "➖";
            btnRemovePath.UseVisualStyleBackColor = true;
            btnRemovePath.Click += BtnRemovePath_Click;
            // 
            // btnAddPath
            // 
            btnAddPath.Location = new System.Drawing.Point(647, 220);
            btnAddPath.Name = "btnAddPath";
            btnAddPath.Size = new System.Drawing.Size(55, 23);
            btnAddPath.TabIndex = 22;
            btnAddPath.Text = "➕";
            btnAddPath.UseVisualStyleBackColor = true;
            btnAddPath.Click += BtnAddPath_Click;
            // 
            // txtAdditionalPath
            // 
            txtAdditionalPath.Location = new System.Drawing.Point(345, 221);
            txtAdditionalPath.Name = "txtAdditionalPath";
            txtAdditionalPath.Size = new System.Drawing.Size(298, 21);
            txtAdditionalPath.TabIndex = 21;
            // 
            // lbxImportProcessPaths
            // 
            lbxImportProcessPaths.FormattingEnabled = true;
            lbxImportProcessPaths.ItemHeight = 13;
            lbxImportProcessPaths.Location = new System.Drawing.Point(343, 248);
            lbxImportProcessPaths.Name = "lbxImportProcessPaths";
            lbxImportProcessPaths.Size = new System.Drawing.Size(300, 95);
            lbxImportProcessPaths.TabIndex = 20;
            // 
            // chkRemoveDupesOnDisplay
            // 
            chkRemoveDupesOnDisplay.AutoSize = true;
            chkRemoveDupesOnDisplay.Location = new System.Drawing.Point(6, 252);
            chkRemoveDupesOnDisplay.Name = "chkRemoveDupesOnDisplay";
            chkRemoveDupesOnDisplay.Size = new System.Drawing.Size(211, 17);
            chkRemoveDupesOnDisplay.TabIndex = 19;
            chkRemoveDupesOnDisplay.Text = "Re-apply the filtering on display.";
            chkRemoveDupesOnDisplay.UseVisualStyleBackColor = true;
            chkRemoveDupesOnDisplay.MouseHover += Generic_MouseHover;
            // 
            // chkRemoveDupes
            // 
            chkRemoveDupes.AutoSize = true;
            chkRemoveDupes.Location = new System.Drawing.Point(6, 229);
            chkRemoveDupes.Name = "chkRemoveDupes";
            chkRemoveDupes.Size = new System.Drawing.Size(297, 17);
            chkRemoveDupes.TabIndex = 18;
            chkRemoveDupes.Text = "Detect and remove duplicate entries on import.";
            chkRemoveDupes.UseVisualStyleBackColor = true;
            chkRemoveDupes.MouseHover += Generic_MouseHover;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(label8);
            groupBox5.Controls.Add(label7);
            groupBox5.Controls.Add(txtPortBinding);
            groupBox5.Controls.Add(txtIPBinding);
            groupBox5.Location = new System.Drawing.Point(6, 147);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new System.Drawing.Size(297, 57);
            groupBox5.TabIndex = 17;
            groupBox5.TabStop = false;
            groupBox5.Text = "Listener Config.";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(210, 27);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(12, 13);
            label8.TabIndex = 3;
            label8.Text = ":";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(6, 27);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(74, 13);
            label7.TabIndex = 2;
            label7.Text = "IP Address:";
            // 
            // txtPortBinding
            // 
            txtPortBinding.Location = new System.Drawing.Point(241, 24);
            txtPortBinding.Name = "txtPortBinding";
            txtPortBinding.Size = new System.Drawing.Size(39, 21);
            txtPortBinding.TabIndex = 1;
            txtPortBinding.MouseHover += Generic_MouseHover;
            // 
            // txtIPBinding
            // 
            txtIPBinding.Location = new System.Drawing.Point(104, 24);
            txtIPBinding.Name = "txtIPBinding";
            txtIPBinding.Size = new System.Drawing.Size(100, 21);
            txtIPBinding.TabIndex = 0;
            txtIPBinding.MouseHover += Generic_MouseHover;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(chkXRefCheckAssertions);
            groupBox1.Controls.Add(chkXRefResourceMessages);
            groupBox1.Controls.Add(chkXRefVerbLogs);
            groupBox1.Controls.Add(chkXRefMiniLogs);
            groupBox1.Controls.Add(chkXRefLogs);
            groupBox1.Controls.Add(chkXRefExceptions);
            groupBox1.Controls.Add(chkXRefErrors);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(chkXRefWarnings);
            groupBox1.Controls.Add(chkXRefAppInitialises);
            groupBox1.Location = new System.Drawing.Point(6, 6);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(702, 135);
            groupBox1.TabIndex = 7;
            groupBox1.TabStop = false;
            groupBox1.Text = " Cross Reference Options";
            // 
            // chkXRefCheckAssertions
            // 
            chkXRefCheckAssertions.AutoSize = true;
            chkXRefCheckAssertions.Location = new System.Drawing.Point(337, 61);
            chkXRefCheckAssertions.Name = "chkXRefCheckAssertions";
            chkXRefCheckAssertions.Size = new System.Drawing.Size(105, 17);
            chkXRefCheckAssertions.TabIndex = 11;
            chkXRefCheckAssertions.Text = "XR Assertions";
            chkXRefCheckAssertions.UseVisualStyleBackColor = true;
            chkXRefCheckAssertions.MouseHover += Generic_MouseHover;
            // 
            // chkXRefResourceMessages
            // 
            chkXRefResourceMessages.AutoSize = true;
            chkXRefResourceMessages.Location = new System.Drawing.Point(337, 41);
            chkXRefResourceMessages.Name = "chkXRefResourceMessages";
            chkXRefResourceMessages.Size = new System.Drawing.Size(158, 17);
            chkXRefResourceMessages.TabIndex = 10;
            chkXRefResourceMessages.Text = "XR Resource Messages";
            chkXRefResourceMessages.UseVisualStyleBackColor = true;
            chkXRefResourceMessages.MouseHover += Generic_MouseHover;
            // 
            // chkXRefVerbLogs
            // 
            chkXRefVerbLogs.AutoSize = true;
            chkXRefVerbLogs.Location = new System.Drawing.Point(337, 20);
            chkXRefVerbLogs.Name = "chkXRefVerbLogs";
            chkXRefVerbLogs.Size = new System.Drawing.Size(102, 17);
            chkXRefVerbLogs.TabIndex = 9;
            chkXRefVerbLogs.Text = "XR Verb Logs";
            chkXRefVerbLogs.UseVisualStyleBackColor = true;
            chkXRefVerbLogs.MouseHover += Generic_MouseHover;
            // 
            // chkXRefMiniLogs
            // 
            chkXRefMiniLogs.AutoSize = true;
            chkXRefMiniLogs.Location = new System.Drawing.Point(182, 60);
            chkXRefMiniLogs.Name = "chkXRefMiniLogs";
            chkXRefMiniLogs.Size = new System.Drawing.Size(98, 17);
            chkXRefMiniLogs.TabIndex = 8;
            chkXRefMiniLogs.Text = "XR Mini Logs";
            chkXRefMiniLogs.UseVisualStyleBackColor = true;
            chkXRefMiniLogs.Visible = false;
            chkXRefMiniLogs.MouseHover += Generic_MouseHover;
            // 
            // chkXRefLogs
            // 
            chkXRefLogs.AutoSize = true;
            chkXRefLogs.Location = new System.Drawing.Point(182, 40);
            chkXRefLogs.Name = "chkXRefLogs";
            chkXRefLogs.Size = new System.Drawing.Size(72, 17);
            chkXRefLogs.TabIndex = 7;
            chkXRefLogs.Text = "XR Logs";
            chkXRefLogs.UseVisualStyleBackColor = true;
            chkXRefLogs.MouseHover += Generic_MouseHover;
            // 
            // chkXRefExceptions
            // 
            chkXRefExceptions.AutoSize = true;
            chkXRefExceptions.Location = new System.Drawing.Point(182, 19);
            chkXRefExceptions.Name = "chkXRefExceptions";
            chkXRefExceptions.Size = new System.Drawing.Size(107, 17);
            chkXRefExceptions.TabIndex = 6;
            chkXRefExceptions.Text = "XR Exceptions";
            chkXRefExceptions.UseVisualStyleBackColor = true;
            chkXRefExceptions.MouseHover += Generic_MouseHover;
            // 
            // chkXRefErrors
            // 
            chkXRefErrors.AutoSize = true;
            chkXRefErrors.Location = new System.Drawing.Point(9, 60);
            chkXRefErrors.Name = "chkXRefErrors";
            chkXRefErrors.Size = new System.Drawing.Size(81, 17);
            chkXRefErrors.TabIndex = 5;
            chkXRefErrors.Text = "XR Errors";
            chkXRefErrors.UseVisualStyleBackColor = true;
            chkXRefErrors.MouseHover += Generic_MouseHover;
            // 
            // label3
            // 
            label3.Location = new System.Drawing.Point(6, 81);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(422, 43);
            label3.TabIndex = 4;
            label3.Text = "Enabling these options will cause certain events to be cross referenced into the main view.  Turning all of these on simultaniously will turn the main view into a viewer showing all data.";
            // 
            // chkXRefWarnings
            // 
            chkXRefWarnings.AutoSize = true;
            chkXRefWarnings.Location = new System.Drawing.Point(9, 40);
            chkXRefWarnings.Name = "chkXRefWarnings";
            chkXRefWarnings.Size = new System.Drawing.Size(98, 17);
            chkXRefWarnings.TabIndex = 1;
            chkXRefWarnings.Text = "XR Warnings";
            chkXRefWarnings.UseVisualStyleBackColor = true;
            chkXRefWarnings.MouseHover += Generic_MouseHover;
            // 
            // chkXRefAppInitialises
            // 
            chkXRefAppInitialises.AutoSize = true;
            chkXRefAppInitialises.Location = new System.Drawing.Point(9, 19);
            chkXRefAppInitialises.Name = "chkXRefAppInitialises";
            chkXRefAppInitialises.Size = new System.Drawing.Size(120, 17);
            chkXRefAppInitialises.TabIndex = 0;
            chkXRefAppInitialises.Text = "XR App Initialise";
            chkXRefAppInitialises.UseVisualStyleBackColor = true;
            chkXRefAppInitialises.MouseHover += Generic_MouseHover;
            // 
            // tabAdvancedOptions
            // 
            tabAdvancedOptions.Controls.Add(groupBox3);
            tabAdvancedOptions.Controls.Add(groupBox9);
            tabAdvancedOptions.Controls.Add(chkTimingsViewIgnoresThreads);
            tabAdvancedOptions.Controls.Add(groupBox4);
            tabAdvancedOptions.Location = new System.Drawing.Point(4, 24);
            tabAdvancedOptions.Name = "tabAdvancedOptions";
            tabAdvancedOptions.Padding = new Padding(3);
            tabAdvancedOptions.Size = new System.Drawing.Size(723, 347);
            tabAdvancedOptions.TabIndex = 3;
            tabAdvancedOptions.Text = "Advanced Options";
            tabAdvancedOptions.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(chkIncludeFileContents);
            groupBox3.Controls.Add(chkInlucdeTCPContents);
            groupBox3.Controls.Add(chkImportLoggingInlcudedsODS);
            groupBox3.Controls.Add(chkActivateImportLogging);
            groupBox3.Location = new System.Drawing.Point(18, 60);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new System.Drawing.Size(288, 121);
            groupBox3.TabIndex = 15;
            groupBox3.TabStop = false;
            groupBox3.Text = " Logging. ";
            // 
            // chkIncludeFileContents
            // 
            chkIncludeFileContents.AutoSize = true;
            chkIncludeFileContents.Location = new System.Drawing.Point(31, 88);
            chkIncludeFileContents.Name = "chkIncludeFileContents";
            chkIncludeFileContents.Size = new System.Drawing.Size(213, 17);
            chkIncludeFileContents.TabIndex = 3;
            chkIncludeFileContents.Text = "Include Imported File messages.";
            chkIncludeFileContents.UseVisualStyleBackColor = true;
            // 
            // chkInlucdeTCPContents
            // 
            chkInlucdeTCPContents.AutoSize = true;
            chkInlucdeTCPContents.Location = new System.Drawing.Point(31, 68);
            chkInlucdeTCPContents.Name = "chkInlucdeTCPContents";
            chkInlucdeTCPContents.Size = new System.Drawing.Size(160, 17);
            chkInlucdeTCPContents.TabIndex = 2;
            chkInlucdeTCPContents.Text = "Include TCP messages.";
            chkInlucdeTCPContents.UseVisualStyleBackColor = true;
            // 
            // chkImportLoggingInlcudedsODS
            // 
            chkImportLoggingInlcudedsODS.AutoSize = true;
            chkImportLoggingInlcudedsODS.Location = new System.Drawing.Point(31, 45);
            chkImportLoggingInlcudedsODS.Name = "chkImportLoggingInlcudedsODS";
            chkImportLoggingInlcudedsODS.Size = new System.Drawing.Size(246, 17);
            chkImportLoggingInlcudedsODS.TabIndex = 1;
            chkImportLoggingInlcudedsODS.Text = "Include OutputDebugString messages.";
            chkImportLoggingInlcudedsODS.UseVisualStyleBackColor = true;
            // 
            // chkActivateImportLogging
            // 
            chkActivateImportLogging.AutoSize = true;
            chkActivateImportLogging.Location = new System.Drawing.Point(10, 23);
            chkActivateImportLogging.Name = "chkActivateImportLogging";
            chkActivateImportLogging.Size = new System.Drawing.Size(167, 17);
            chkActivateImportLogging.TabIndex = 0;
            chkActivateImportLogging.Text = "Activate Import Logging.";
            chkActivateImportLogging.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            groupBox9.Controls.Add(btnDeleteMapping);
            groupBox9.Controls.Add(btnAddMapping);
            groupBox9.Controls.Add(label14);
            groupBox9.Controls.Add(label13);
            groupBox9.Controls.Add(lbxNameMappings);
            groupBox9.Controls.Add(textBox2);
            groupBox9.Controls.Add(textBox1);
            groupBox9.Location = new System.Drawing.Point(339, 131);
            groupBox9.Name = "groupBox9";
            groupBox9.Size = new System.Drawing.Size(378, 160);
            groupBox9.TabIndex = 14;
            groupBox9.TabStop = false;
            groupBox9.Text = "Workstation Name Mappings";
            groupBox9.Visible = false;
            // 
            // btnDeleteMapping
            // 
            btnDeleteMapping.Location = new System.Drawing.Point(11, 110);
            btnDeleteMapping.Name = "btnDeleteMapping";
            btnDeleteMapping.Size = new System.Drawing.Size(123, 23);
            btnDeleteMapping.TabIndex = 6;
            btnDeleteMapping.Text = "Delete Mapping";
            btnDeleteMapping.UseVisualStyleBackColor = true;
            // 
            // btnAddMapping
            // 
            btnAddMapping.Location = new System.Drawing.Point(6, 131);
            btnAddMapping.Name = "btnAddMapping";
            btnAddMapping.Size = new System.Drawing.Size(103, 23);
            btnAddMapping.TabIndex = 5;
            btnAddMapping.Text = "Add Mapping";
            btnAddMapping.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new System.Drawing.Point(28, 64);
            label14.Name = "label14";
            label14.Size = new System.Drawing.Size(212, 13);
            label14.TabIndex = 4;
            label14.Text = "Shall from henceforth be known as:";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new System.Drawing.Point(51, 40);
            label13.Name = "label13";
            label13.Size = new System.Drawing.Size(162, 13);
            label13.TabIndex = 3;
            label13.Text = "The box formerly know as:";
            // 
            // lbxNameMappings
            // 
            lbxNameMappings.FormattingEnabled = true;
            lbxNameMappings.ItemHeight = 13;
            lbxNameMappings.Location = new System.Drawing.Point(11, 20);
            lbxNameMappings.Name = "lbxNameMappings";
            lbxNameMappings.Size = new System.Drawing.Size(113, 30);
            lbxNameMappings.TabIndex = 2;
            // 
            // textBox2
            // 
            textBox2.Location = new System.Drawing.Point(11, 83);
            textBox2.Name = "textBox2";
            textBox2.Size = new System.Drawing.Size(113, 21);
            textBox2.TabIndex = 1;
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(11, 56);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(113, 21);
            textBox1.TabIndex = 0;
            // 
            // chkTimingsViewIgnoresThreads
            // 
            chkTimingsViewIgnoresThreads.AutoSize = true;
            chkTimingsViewIgnoresThreads.Location = new System.Drawing.Point(18, 19);
            chkTimingsViewIgnoresThreads.Name = "chkTimingsViewIgnoresThreads";
            chkTimingsViewIgnoresThreads.Size = new System.Drawing.Size(197, 17);
            chkTimingsViewIgnoresThreads.TabIndex = 13;
            chkTimingsViewIgnoresThreads.Text = "Timings view ignores threads.";
            chkTimingsViewIgnoresThreads.UseVisualStyleBackColor = true;
            chkTimingsViewIgnoresThreads.MouseHover += Generic_MouseHover;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(txtNormalisationMS);
            groupBox4.Controls.Add(chkNormaliseRefresh);
            groupBox4.Controls.Add(btnSetODSAcl);
            groupBox4.Controls.Add(label6);
            groupBox4.Controls.Add(label5);
            groupBox4.Controls.Add(label4);
            groupBox4.Controls.Add(nudPushbackLimitCount);
            groupBox4.Enabled = false;
            groupBox4.Location = new System.Drawing.Point(339, 19);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new System.Drawing.Size(377, 94);
            groupBox4.TabIndex = 12;
            groupBox4.TabStop = false;
            groupBox4.Text = "Advanced Options";
            groupBox4.Visible = false;
            // 
            // txtNormalisationMS
            // 
            txtNormalisationMS.Location = new System.Drawing.Point(9, 67);
            txtNormalisationMS.Name = "txtNormalisationMS";
            txtNormalisationMS.Size = new System.Drawing.Size(67, 21);
            txtNormalisationMS.TabIndex = 7;
            // 
            // chkNormaliseRefresh
            // 
            chkNormaliseRefresh.AutoSize = true;
            chkNormaliseRefresh.Location = new System.Drawing.Point(9, 45);
            chkNormaliseRefresh.Name = "chkNormaliseRefresh";
            chkNormaliseRefresh.Size = new System.Drawing.Size(131, 17);
            chkNormaliseRefresh.TabIndex = 6;
            chkNormaliseRefresh.Text = "Normalise Refresh";
            chkNormaliseRefresh.UseVisualStyleBackColor = true;
            // 
            // btnSetODSAcl
            // 
            btnSetODSAcl.Location = new System.Drawing.Point(9, 19);
            btnSetODSAcl.Name = "btnSetODSAcl";
            btnSetODSAcl.Size = new System.Drawing.Size(67, 20);
            btnSetODSAcl.TabIndex = 5;
            btnSetODSAcl.Text = "Set ODS ACL";
            btnSetODSAcl.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(160, 70);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(55, 13);
            label6.TabIndex = 4;
            label6.Text = "Seconds";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(92, 70);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(62, 13);
            label5.TabIndex = 3;
            label5.Text = "Delays or";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(83, 23);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(114, 13);
            label4.TabIndex = 1;
            label4.Text = "Pushback Limiting:";
            // 
            // nudPushbackLimitCount
            // 
            nudPushbackLimitCount.Location = new System.Drawing.Point(146, 41);
            nudPushbackLimitCount.Name = "nudPushbackLimitCount";
            nudPushbackLimitCount.Size = new System.Drawing.Size(38, 21);
            nudPushbackLimitCount.TabIndex = 0;
            nudPushbackLimitCount.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(chkFilterDefaultIncludesClasses);
            tabPage4.Controls.Add(chkFilterDefaultIncludeModules);
            tabPage4.Controls.Add(chkFilterDefaultIncludesThreads);
            tabPage4.Controls.Add(chkFilterDefaultIncludesLocations);
            tabPage4.Controls.Add(chkAllowInternalMessageDisplays);
            tabPage4.Controls.Add(chkRelocateOnChange);
            tabPage4.Controls.Add(btnBrowseForFilterDir);
            tabPage4.Controls.Add(label1);
            tabPage4.Controls.Add(txtFilterAndHighlightDir);
            tabPage4.Location = new System.Drawing.Point(4, 24);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new System.Drawing.Size(723, 347);
            tabPage4.TabIndex = 4;
            tabPage4.Text = "Filter Behaviour";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // chkFilterDefaultIncludesClasses
            // 
            chkFilterDefaultIncludesClasses.AutoSize = true;
            chkFilterDefaultIncludesClasses.Location = new System.Drawing.Point(79, 185);
            chkFilterDefaultIncludesClasses.Name = "chkFilterDefaultIncludesClasses";
            chkFilterDefaultIncludesClasses.Size = new System.Drawing.Size(213, 17);
            chkFilterDefaultIncludesClasses.TabIndex = 27;
            chkFilterDefaultIncludesClasses.Text = "By default filters include classes.";
            chkFilterDefaultIncludesClasses.UseVisualStyleBackColor = true;
            chkFilterDefaultIncludesClasses.MouseHover += Generic_MouseHover;
            // 
            // chkFilterDefaultIncludeModules
            // 
            chkFilterDefaultIncludeModules.AutoSize = true;
            chkFilterDefaultIncludeModules.Location = new System.Drawing.Point(79, 162);
            chkFilterDefaultIncludeModules.Name = "chkFilterDefaultIncludeModules";
            chkFilterDefaultIncludeModules.Size = new System.Drawing.Size(220, 17);
            chkFilterDefaultIncludeModules.TabIndex = 26;
            chkFilterDefaultIncludeModules.Text = "By default filters include modules.";
            chkFilterDefaultIncludeModules.UseVisualStyleBackColor = true;
            chkFilterDefaultIncludeModules.MouseHover += Generic_MouseHover;
            // 
            // chkFilterDefaultIncludesThreads
            // 
            chkFilterDefaultIncludesThreads.AutoSize = true;
            chkFilterDefaultIncludesThreads.Location = new System.Drawing.Point(79, 139);
            chkFilterDefaultIncludesThreads.Name = "chkFilterDefaultIncludesThreads";
            chkFilterDefaultIncludesThreads.Size = new System.Drawing.Size(215, 17);
            chkFilterDefaultIncludesThreads.TabIndex = 25;
            chkFilterDefaultIncludesThreads.Text = "By default filters include threads.";
            chkFilterDefaultIncludesThreads.UseVisualStyleBackColor = true;
            chkFilterDefaultIncludesThreads.MouseHover += Generic_MouseHover;
            // 
            // chkFilterDefaultIncludesLocations
            // 
            chkFilterDefaultIncludesLocations.AutoSize = true;
            chkFilterDefaultIncludesLocations.Location = new System.Drawing.Point(79, 116);
            chkFilterDefaultIncludesLocations.Name = "chkFilterDefaultIncludesLocations";
            chkFilterDefaultIncludesLocations.Size = new System.Drawing.Size(222, 17);
            chkFilterDefaultIncludesLocations.TabIndex = 24;
            chkFilterDefaultIncludesLocations.Text = "By default filters include locations.";
            chkFilterDefaultIncludesLocations.UseVisualStyleBackColor = true;
            chkFilterDefaultIncludesLocations.MouseHover += Generic_MouseHover;
            // 
            // chkAllowInternalMessageDisplays
            // 
            chkAllowInternalMessageDisplays.AutoSize = true;
            chkAllowInternalMessageDisplays.Location = new System.Drawing.Point(79, 69);
            chkAllowInternalMessageDisplays.Name = "chkAllowInternalMessageDisplays";
            chkAllowInternalMessageDisplays.Size = new System.Drawing.Size(255, 17);
            chkAllowInternalMessageDisplays.TabIndex = 23;
            chkAllowInternalMessageDisplays.Text = "Allow internal messages to be displayed";
            chkAllowInternalMessageDisplays.UseVisualStyleBackColor = true;
            chkAllowInternalMessageDisplays.MouseHover += Generic_MouseHover;
            // 
            // chkRelocateOnChange
            // 
            chkRelocateOnChange.AutoSize = true;
            chkRelocateOnChange.Checked = true;
            chkRelocateOnChange.CheckState = CheckState.Checked;
            chkRelocateOnChange.Location = new System.Drawing.Point(79, 46);
            chkRelocateOnChange.Name = "chkRelocateOnChange";
            chkRelocateOnChange.Size = new System.Drawing.Size(281, 17);
            chkRelocateOnChange.TabIndex = 22;
            chkRelocateOnChange.Text = "Relocate existing filters on directory change.";
            chkRelocateOnChange.UseVisualStyleBackColor = true;
            chkRelocateOnChange.MouseHover += Generic_MouseHover;
            // 
            // btnBrowseForFilterDir
            // 
            btnBrowseForFilterDir.Location = new System.Drawing.Point(641, 33);
            btnBrowseForFilterDir.Name = "btnBrowseForFilterDir";
            btnBrowseForFilterDir.Size = new System.Drawing.Size(75, 23);
            btnBrowseForFilterDir.TabIndex = 21;
            btnBrowseForFilterDir.Text = "...";
            btnBrowseForFilterDir.UseVisualStyleBackColor = true;
            btnBrowseForFilterDir.Click += BrowseForFilterDir_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(16, 13);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(61, 13);
            label1.TabIndex = 16;
            label1.Text = "Filter Dir:";
            // 
            // txtFilterAndHighlightDir
            // 
            txtFilterAndHighlightDir.Location = new System.Drawing.Point(79, 10);
            txtFilterAndHighlightDir.Name = "txtFilterAndHighlightDir";
            txtFilterAndHighlightDir.ReadOnly = true;
            txtFilterAndHighlightDir.Size = new System.Drawing.Size(638, 21);
            txtFilterAndHighlightDir.TabIndex = 15;
            // 
            // tabTexSettings
            // 
            tabTexSettings.Controls.Add(btnReleaseDefaults);
            tabTexSettings.Controls.Add(btnDevelopmentDefaults);
            tabTexSettings.Controls.Add(chkAddStackInfo);
            tabTexSettings.Controls.Add(chkEnableEnhancements);
            tabTexSettings.Controls.Add(chkExpandAssertions);
            tabTexSettings.Controls.Add(btnReadEnvironment);
            tabTexSettings.Controls.Add(btnSetEnvironment);
            tabTexSettings.Controls.Add(chkUseHighPerf);
            tabTexSettings.Controls.Add(groupBox7);
            tabTexSettings.Controls.Add(grpTraceLevel);
            tabTexSettings.Controls.Add(txtGeneratedString);
            tabTexSettings.Location = new System.Drawing.Point(4, 24);
            tabTexSettings.Name = "tabTexSettings";
            tabTexSettings.Padding = new Padding(3);
            tabTexSettings.Size = new System.Drawing.Size(723, 347);
            tabTexSettings.TabIndex = 5;
            tabTexSettings.Text = "Tex Behaviour";
            tabTexSettings.UseVisualStyleBackColor = true;
            // 
            // btnReleaseDefaults
            // 
            btnReleaseDefaults.Location = new System.Drawing.Point(555, 174);
            btnReleaseDefaults.Name = "btnReleaseDefaults";
            btnReleaseDefaults.Size = new System.Drawing.Size(151, 23);
            btnReleaseDefaults.TabIndex = 31;
            btnReleaseDefaults.Text = "Rel. Defaults";
            btnReleaseDefaults.UseVisualStyleBackColor = true;
            btnReleaseDefaults.Click += ReleaseDefaults_Click;
            btnReleaseDefaults.MouseHover += Generic_MouseHover;
            // 
            // btnDevelopmentDefaults
            // 
            btnDevelopmentDefaults.Location = new System.Drawing.Point(555, 145);
            btnDevelopmentDefaults.Name = "btnDevelopmentDefaults";
            btnDevelopmentDefaults.Size = new System.Drawing.Size(151, 23);
            btnDevelopmentDefaults.TabIndex = 30;
            btnDevelopmentDefaults.Text = "Dev. Defaults";
            btnDevelopmentDefaults.UseVisualStyleBackColor = true;
            btnDevelopmentDefaults.Click += DevelopmentDefaults_Click;
            btnDevelopmentDefaults.MouseHover += Generic_MouseHover;
            // 
            // chkAddStackInfo
            // 
            chkAddStackInfo.AutoSize = true;
            chkAddStackInfo.Location = new System.Drawing.Point(555, 108);
            chkAddStackInfo.Name = "chkAddStackInfo";
            chkAddStackInfo.Size = new System.Drawing.Size(155, 17);
            chkAddStackInfo.TabIndex = 29;
            chkAddStackInfo.Text = "Add Stack Information";
            chkAddStackInfo.UseVisualStyleBackColor = true;
            chkAddStackInfo.CheckedChanged += TexOptionsScreen_CheckedChanged;
            chkAddStackInfo.MouseHover += Generic_MouseHover;
            // 
            // chkEnableEnhancements
            // 
            chkEnableEnhancements.AutoSize = true;
            chkEnableEnhancements.Location = new System.Drawing.Point(555, 16);
            chkEnableEnhancements.Name = "chkEnableEnhancements";
            chkEnableEnhancements.Size = new System.Drawing.Size(151, 17);
            chkEnableEnhancements.TabIndex = 28;
            chkEnableEnhancements.Text = "Enable Enhancements";
            chkEnableEnhancements.UseVisualStyleBackColor = true;
            chkEnableEnhancements.CheckedChanged += TexOptionsScreen_CheckedChanged;
            chkEnableEnhancements.MouseHover += Generic_MouseHover;
            // 
            // chkExpandAssertions
            // 
            chkExpandAssertions.AutoSize = true;
            chkExpandAssertions.Location = new System.Drawing.Point(555, 39);
            chkExpandAssertions.Name = "chkExpandAssertions";
            chkExpandAssertions.Size = new System.Drawing.Size(131, 17);
            chkExpandAssertions.TabIndex = 27;
            chkExpandAssertions.Text = "Expand Assertions";
            chkExpandAssertions.UseVisualStyleBackColor = true;
            chkExpandAssertions.CheckedChanged += TexOptionsScreen_CheckedChanged;
            chkExpandAssertions.MouseHover += Generic_MouseHover;
            // 
            // btnReadEnvironment
            // 
            btnReadEnvironment.Location = new System.Drawing.Point(307, 310);
            btnReadEnvironment.Name = "btnReadEnvironment";
            btnReadEnvironment.Size = new System.Drawing.Size(195, 23);
            btnReadEnvironment.TabIndex = 21;
            btnReadEnvironment.Text = "Get Environment Variable";
            btnReadEnvironment.UseVisualStyleBackColor = true;
            btnReadEnvironment.Click += ReadEnvironment_Click;
            btnReadEnvironment.MouseHover += Generic_MouseHover;
            // 
            // btnSetEnvironment
            // 
            btnSetEnvironment.Location = new System.Drawing.Point(523, 310);
            btnSetEnvironment.Name = "btnSetEnvironment";
            btnSetEnvironment.Size = new System.Drawing.Size(193, 23);
            btnSetEnvironment.TabIndex = 20;
            btnSetEnvironment.Text = "Set Environment (Machine)";
            btnSetEnvironment.UseVisualStyleBackColor = true;
            btnSetEnvironment.Click += SetEnvironment_Click;
            btnSetEnvironment.MouseHover += Generic_MouseHover;
            // 
            // chkUseHighPerf
            // 
            chkUseHighPerf.AutoSize = true;
            chkUseHighPerf.Location = new System.Drawing.Point(555, 85);
            chkUseHighPerf.Name = "chkUseHighPerf";
            chkUseHighPerf.Size = new System.Drawing.Size(125, 17);
            chkUseHighPerf.TabIndex = 18;
            chkUseHighPerf.Text = "High Perf support";
            chkUseHighPerf.UseVisualStyleBackColor = true;
            chkUseHighPerf.CheckedChanged += TexOptionsScreen_CheckedChanged;
            chkUseHighPerf.MouseHover += Generic_MouseHover;
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(label12);
            groupBox7.Controls.Add(label11);
            groupBox7.Controls.Add(lblErrorMsg);
            groupBox7.Controls.Add(btnAddNewListener);
            groupBox7.Controls.Add(btnRemoveSelected);
            groupBox7.Controls.Add(lbxAddTCPListeners);
            groupBox7.Controls.Add(chkMakeTcpInteractive);
            groupBox7.Controls.Add(chkRemoveAll);
            groupBox7.Controls.Add(txtIPInit);
            groupBox7.Controls.Add(txtPortInit);
            groupBox7.Controls.Add(chkAddTexListener);
            groupBox7.Location = new System.Drawing.Point(137, 6);
            groupBox7.Name = "groupBox7";
            groupBox7.Size = new System.Drawing.Size(375, 271);
            groupBox7.TabIndex = 2;
            groupBox7.TabStop = false;
            groupBox7.Text = "Listeners";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new System.Drawing.Point(144, 76);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(35, 13);
            label12.TabIndex = 13;
            label12.Text = "Port:";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new System.Drawing.Point(18, 76);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(69, 13);
            label11.TabIndex = 12;
            label11.Text = "Hostname:";
            // 
            // lblErrorMsg
            // 
            lblErrorMsg.Location = new System.Drawing.Point(10, 217);
            lblErrorMsg.Name = "lblErrorMsg";
            lblErrorMsg.Size = new System.Drawing.Size(355, 51);
            lblErrorMsg.TabIndex = 11;
            // 
            // btnAddNewListener
            // 
            btnAddNewListener.Enabled = false;
            btnAddNewListener.Location = new System.Drawing.Point(263, 119);
            btnAddNewListener.Name = "btnAddNewListener";
            btnAddNewListener.Size = new System.Drawing.Size(75, 23);
            btnAddNewListener.TabIndex = 6;
            btnAddNewListener.Text = "Add";
            btnAddNewListener.UseVisualStyleBackColor = true;
            btnAddNewListener.Click += AddNewListener_Click;
            // 
            // btnRemoveSelected
            // 
            btnRemoveSelected.Enabled = false;
            btnRemoveSelected.Location = new System.Drawing.Point(263, 148);
            btnRemoveSelected.Name = "btnRemoveSelected";
            btnRemoveSelected.Size = new System.Drawing.Size(75, 23);
            btnRemoveSelected.TabIndex = 7;
            btnRemoveSelected.Text = "Remove";
            btnRemoveSelected.UseVisualStyleBackColor = true;
            btnRemoveSelected.Click += RemoveSelected_Click;
            // 
            // lbxAddTCPListeners
            // 
            lbxAddTCPListeners.FormattingEnabled = true;
            lbxAddTCPListeners.ItemHeight = 13;
            lbxAddTCPListeners.Location = new System.Drawing.Point(21, 119);
            lbxAddTCPListeners.Name = "lbxAddTCPListeners";
            lbxAddTCPListeners.Size = new System.Drawing.Size(236, 95);
            lbxAddTCPListeners.TabIndex = 5;
            lbxAddTCPListeners.SelectedIndexChanged += AddTCPListeners_SelectedIndexChanged;
            lbxAddTCPListeners.MouseHover += Generic_MouseHover;
            // 
            // chkMakeTcpInteractive
            // 
            chkMakeTcpInteractive.AutoSize = true;
            chkMakeTcpInteractive.Checked = true;
            chkMakeTcpInteractive.CheckState = CheckState.Checked;
            chkMakeTcpInteractive.Location = new System.Drawing.Point(244, 94);
            chkMakeTcpInteractive.Name = "chkMakeTcpInteractive";
            chkMakeTcpInteractive.Size = new System.Drawing.Size(88, 17);
            chkMakeTcpInteractive.TabIndex = 4;
            chkMakeTcpInteractive.Text = "Interactive";
            chkMakeTcpInteractive.UseVisualStyleBackColor = true;
            chkMakeTcpInteractive.MouseHover += Generic_MouseHover;
            // 
            // chkRemoveAll
            // 
            chkRemoveAll.AutoSize = true;
            chkRemoveAll.Location = new System.Drawing.Point(21, 25);
            chkRemoveAll.Name = "chkRemoveAll";
            chkRemoveAll.Size = new System.Drawing.Size(253, 17);
            chkRemoveAll.TabIndex = 0;
            chkRemoveAll.Text = "Remove All Listeners before adding......";
            chkRemoveAll.UseVisualStyleBackColor = true;
            chkRemoveAll.CheckedChanged += TexOptionsScreen_CheckedChanged;
            chkRemoveAll.MouseHover += Generic_MouseHover;
            // 
            // txtIPInit
            // 
            txtIPInit.Location = new System.Drawing.Point(20, 92);
            txtIPInit.MaxLength = 255;
            txtIPInit.Name = "txtIPInit";
            txtIPInit.Size = new System.Drawing.Size(121, 21);
            txtIPInit.TabIndex = 2;
            txtIPInit.TextChanged += IPInit_TextChanged;
            txtIPInit.MouseHover += Generic_MouseHover;
            // 
            // txtPortInit
            // 
            txtPortInit.Location = new System.Drawing.Point(147, 92);
            txtPortInit.MaxLength = 8;
            txtPortInit.Name = "txtPortInit";
            txtPortInit.Size = new System.Drawing.Size(64, 21);
            txtPortInit.TabIndex = 3;
            txtPortInit.TextChanged += IPInit_TextChanged;
            txtPortInit.MouseHover += Generic_MouseHover;
            // 
            // chkAddTexListener
            // 
            chkAddTexListener.AutoSize = true;
            chkAddTexListener.Location = new System.Drawing.Point(21, 48);
            chkAddTexListener.Name = "chkAddTexListener";
            chkAddTexListener.Size = new System.Drawing.Size(95, 17);
            chkAddTexListener.TabIndex = 1;
            chkAddTexListener.Text = "Tex Listener";
            chkAddTexListener.UseVisualStyleBackColor = true;
            chkAddTexListener.CheckedChanged += TexOptionsScreen_CheckedChanged;
            chkAddTexListener.MouseHover += Generic_MouseHover;
            // 
            // grpTraceLevel
            // 
            grpTraceLevel.Controls.Add(rdoWarningLevel);
            grpTraceLevel.Controls.Add(rdoVerboseLevel);
            grpTraceLevel.Controls.Add(rdoErrorLevel);
            grpTraceLevel.Controls.Add(rdoTraceInfo);
            grpTraceLevel.Controls.Add(rdoTraceOff);
            grpTraceLevel.Location = new System.Drawing.Point(6, 6);
            grpTraceLevel.Name = "grpTraceLevel";
            grpTraceLevel.Size = new System.Drawing.Size(125, 150);
            grpTraceLevel.TabIndex = 1;
            grpTraceLevel.TabStop = false;
            grpTraceLevel.Text = "Trace Level";
            grpTraceLevel.MouseHover += Generic_MouseHover;
            // 
            // rdoWarningLevel
            // 
            rdoWarningLevel.AutoSize = true;
            rdoWarningLevel.Location = new System.Drawing.Point(15, 119);
            rdoWarningLevel.Name = "rdoWarningLevel";
            rdoWarningLevel.Size = new System.Drawing.Size(71, 17);
            rdoWarningLevel.TabIndex = 4;
            rdoWarningLevel.TabStop = true;
            rdoWarningLevel.Text = "Warning";
            rdoWarningLevel.UseVisualStyleBackColor = true;
            rdoWarningLevel.CheckedChanged += TexOptionsScreen_CheckedChanged;
            rdoWarningLevel.MouseHover += Generic_MouseHover;
            // 
            // rdoVerboseLevel
            // 
            rdoVerboseLevel.AutoSize = true;
            rdoVerboseLevel.Location = new System.Drawing.Point(15, 96);
            rdoVerboseLevel.Name = "rdoVerboseLevel";
            rdoVerboseLevel.Size = new System.Drawing.Size(71, 17);
            rdoVerboseLevel.TabIndex = 3;
            rdoVerboseLevel.TabStop = true;
            rdoVerboseLevel.Text = "Verbose";
            rdoVerboseLevel.UseVisualStyleBackColor = true;
            rdoVerboseLevel.CheckedChanged += TexOptionsScreen_CheckedChanged;
            rdoVerboseLevel.MouseHover += Generic_MouseHover;
            // 
            // rdoErrorLevel
            // 
            rdoErrorLevel.AutoSize = true;
            rdoErrorLevel.Location = new System.Drawing.Point(15, 72);
            rdoErrorLevel.Name = "rdoErrorLevel";
            rdoErrorLevel.Size = new System.Drawing.Size(54, 17);
            rdoErrorLevel.TabIndex = 2;
            rdoErrorLevel.TabStop = true;
            rdoErrorLevel.Text = "Error";
            rdoErrorLevel.UseVisualStyleBackColor = true;
            rdoErrorLevel.CheckedChanged += TexOptionsScreen_CheckedChanged;
            rdoErrorLevel.MouseHover += Generic_MouseHover;
            // 
            // rdoTraceInfo
            // 
            rdoTraceInfo.AutoSize = true;
            rdoTraceInfo.Location = new System.Drawing.Point(15, 48);
            rdoTraceInfo.Name = "rdoTraceInfo";
            rdoTraceInfo.Size = new System.Drawing.Size(48, 17);
            rdoTraceInfo.TabIndex = 1;
            rdoTraceInfo.TabStop = true;
            rdoTraceInfo.Text = "Info";
            rdoTraceInfo.UseVisualStyleBackColor = true;
            rdoTraceInfo.CheckedChanged += TexOptionsScreen_CheckedChanged;
            rdoTraceInfo.MouseHover += Generic_MouseHover;
            // 
            // rdoTraceOff
            // 
            rdoTraceOff.AutoSize = true;
            rdoTraceOff.Location = new System.Drawing.Point(15, 24);
            rdoTraceOff.Name = "rdoTraceOff";
            rdoTraceOff.Size = new System.Drawing.Size(42, 17);
            rdoTraceOff.TabIndex = 0;
            rdoTraceOff.TabStop = true;
            rdoTraceOff.Text = "Off";
            rdoTraceOff.UseVisualStyleBackColor = true;
            rdoTraceOff.CheckedChanged += TexOptionsScreen_CheckedChanged;
            rdoTraceOff.MouseHover += Generic_MouseHover;
            // 
            // txtGeneratedString
            // 
            txtGeneratedString.Location = new System.Drawing.Point(15, 283);
            txtGeneratedString.Name = "txtGeneratedString";
            txtGeneratedString.ReadOnly = true;
            txtGeneratedString.Size = new System.Drawing.Size(701, 21);
            txtGeneratedString.TabIndex = 15;
            txtGeneratedString.MouseHover += Generic_MouseHover;
            // 
            // lblHelp
            // 
            lblHelp.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            lblHelp.Location = new System.Drawing.Point(4, 383);
            lblHelp.Name = "lblHelp";
            lblHelp.Size = new System.Drawing.Size(726, 57);
            lblHelp.TabIndex = 27;
            lblHelp.Text = "Hover the mouse over a setting for information....";
            // 
            // llbRelatedContent
            // 
            llbRelatedContent.AutoSize = true;
            llbRelatedContent.Location = new System.Drawing.Point(4, 446);
            llbRelatedContent.Name = "llbRelatedContent";
            llbRelatedContent.Size = new System.Drawing.Size(162, 13);
            llbRelatedContent.TabIndex = 28;
            llbRelatedContent.TabStop = true;
            llbRelatedContent.Text = "Click Here for related help.";
            llbRelatedContent.Visible = false;
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            // 
            // frmMexOptionsScreen
            // 
            AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(726, 476);
            Controls.Add(llbRelatedContent);
            Controls.Add(lblHelp);
            Controls.Add(tabOptionsContainer);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximumSize = new System.Drawing.Size(742, 515);
            MinimumSize = new System.Drawing.Size(742, 515);
            Name = "frmMexOptionsScreen";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "MexOptionsScreen";
            Load += FrmMexOptionsScreen_Load;
            tabOptionsContainer.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            grpUserMessageNotificationOptions.ResumeLayout(false);
            grpUserMessageNotificationOptions.PerformLayout();
            tabPage2.ResumeLayout(false);
            grpThreadDisplayOptions.ResumeLayout(false);
            grpThreadDisplayOptions.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabAdvancedOptions.ResumeLayout(false);
            tabAdvancedOptions.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox9.ResumeLayout(false);
            groupBox9.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudPushbackLimitCount).EndInit();
            tabPage4.ResumeLayout(false);
            tabPage4.PerformLayout();
            tabTexSettings.ResumeLayout(false);
            tabTexSettings.PerformLayout();
            groupBox7.ResumeLayout(false);
            groupBox7.PerformLayout();
            grpTraceLevel.ResumeLayout(false);
            grpTraceLevel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion Windows Form Designer generated code

        internal void PopulateOptionsScreenFromOptions(MexOptions mo) {
            txtFilterAndHighlightDir.Text = mo.FilterAndHighlightStoreDirectory;
            filterDirInitialiseValue = mo.FilterAndHighlightStoreDirectory;  // This is used so we know if its changed

            chkRecycleProcessWhenNameMatches.Checked = mo.AutoPurgeApplicationOnMatchingName;
            chkAutoRefresh.Checked = mo.AutoRefresh;
            chkAutoScroll.Checked = mo.AutoScroll;
            chkNormaliseRefresh.Checked = mo.NormaliseRefreshActive;
            chkSelectingProcessSelectsProcessView.Checked = mo.SelectingProcessSelectsProcessView;
            chkAutoSelectProcessIfNoneSelected.Checked = mo.AutoSelectFirstProcess;

            txtNormalisationMS.Text = mo.NormaliseRefreshActive ? mo.NormalisationLimitMilliseconds.ToString() : "0";

            nudPushbackLimitCount.Value = mo.PushbackCountDelayLimitForInteractiveJobs;

            chkRespectFilter.Checked = mo.RespectFilter;
            chkDisplayGlobalIndexInMainView.Checked = mo.ShowGlobalIndexInView;
            chkAllowCancelOperations.Checked = mo.SupportCancellationOfRefresh;
            chkWotTimedViewToo.Checked = mo.TimedViewRespectsFilter;

            chkLeaveMatchingPidsInNonTracedToo.Checked = mo.XRefReverseCopyEnabled;
            chkImportMatchingPIDODSIntoEvents.Checked = mo.XRefMatchingProcessIdsIntoEventEntries;

            chkXRefAppInitialises.Checked = mo.XRefAppInitialiseToMain;
            chkXRefCheckAssertions.Checked = mo.XRefAssertionsToMain;
            chkXRefErrors.Checked = mo.XRefErrorsToMain;
            chkXRefExceptions.Checked = mo.XRefExceptionsToMain;
            chkXRefLogs.Checked = mo.XRefLogsToMain;
            chkXRefMiniLogs.Checked = mo.XRefMiniLogsToMain;
            chkXRefResourceMessages.Checked = mo.XRefResourceMessagesToMain;
            chkXRefVerbLogs.Checked = mo.XRefVerbLogsToMain;
            chkXRefWarnings.Checked = mo.XRefWarningsToMain;
            chkBeautifyOutput.Checked = mo.BeautifyDisplayedStrings;
            chkRemoveDupes.Checked = mo.RemoveDuplicatesOnImport;
            chkRemoveDupesOnDisplay.Checked = mo.RemoveDuplicatesOnView;

            txtIPBinding.Text = mo.IPAddressToBind;
            txtPortBinding.Text = mo.PortAddressToBind.ToString();

            chkMatchingNamePurgeAlsoClearsPartials.Checked = mo.MatchingNamePurgeAlsoClearsPartials;

            chkTimingsViewIgnoresThreads.Checked = mo.TimingsViewIgnoresThreads;

            switch (mo.ThreadDisplayOption) {
                case ThreadDisplayMode.UseDotNetThread:
                    rdoThreadShowNetId.Checked = true;
                    break;

                case ThreadDisplayMode.UseOSThread:
                    rdoThreadShowOSId.Checked = true;
                    break;

                case ThreadDisplayMode.DefaultUseThreadNameAndOS:
                    rdoThreadShowDefault.Checked = true;
                    break;

                case ThreadDisplayMode.ShowFullInformation:
                    rdoThreadShowFullInfo.Checked = true;
                    break;

                default:
                    throw new InvalidOperationException("Should not be able to have another possibility in thread display options enum");
            }

            txtUIRefreshFrequency.Text = mo.NoSecondsForUIUpdate.ToString();
            txtUserLogSize.Text = mo.NoUserNotificationsToStoreInLog.ToString();
            txtLongRunningOps.Text = mo.NoSecondsForRefreshOnImport.ToString();
            chkAllowInternalMessageDisplays.Checked = mo.DisplayInternalMessages;
            chkEnableBacktrace.Checked = mo.EnableBackTrace;
            chkHighlightCrossProcesses.Checked = mo.CrossProcessViewHighlight;
            chkUseRenderNameNotPID.Checked = mo.UsePreferredNameInsteadOfProcessId;

            chkFilterDefaultIncludesClasses.Checked = mo.FilterDefaultSaveClassLocation;
            chkFilterDefaultIncludesThreads.Checked = mo.FilterDefaultSaveThreads;
            chkFilterDefaultIncludesLocations.Checked = mo.FilterDefaultSaveLocations;
            chkFilterDefaultIncludeModules.Checked = mo.FilterDefaultSaveModules;

            foreach (string s in mo.PathsToCheckForImporters) {
                _ = lbxImportProcessPaths.Items.Add(s);
            }
        }

        internal MexOptions GetOptionsFromDialog() {
            var mo = new MexOptions {
                FilterAndHighlightStoreDirectory = txtFilterAndHighlightDir.Text,

                AutoPurgeApplicationOnMatchingName = chkRecycleProcessWhenNameMatches.Checked,
                AutoRefresh = chkAutoRefresh.Checked,
                AutoScroll = chkAutoScroll.Checked,
                SelectingProcessSelectsProcessView = chkSelectingProcessSelectsProcessView.Checked,
                AutoSelectFirstProcess = chkAutoSelectProcessIfNoneSelected.Checked,
                RemoveDuplicatesOnImport = chkRemoveDupes.Checked,
                RemoveDuplicatesOnView = chkRemoveDupesOnDisplay.Checked,

                NormaliseRefreshActive = chkNormaliseRefresh.Checked
            };
            mo.NormalisationLimitMilliseconds = mo.NormaliseRefreshActive ? int.Parse(txtNormalisationMS.Text) : 0;

            mo.PushbackCountDelayLimitForInteractiveJobs = (int)nudPushbackLimitCount.Value;
            mo.RespectFilter = chkRespectFilter.Checked;
            mo.ShowGlobalIndexInView = chkDisplayGlobalIndexInMainView.Checked;

            mo.SupportCancellationOfRefresh = chkAllowCancelOperations.Checked;
            mo.TimedViewRespectsFilter = chkWotTimedViewToo.Checked;

            mo.XRefReverseCopyEnabled = chkLeaveMatchingPidsInNonTracedToo.Checked;
            mo.XRefMatchingProcessIdsIntoEventEntries = chkImportMatchingPIDODSIntoEvents.Checked;

            mo.XRefAppInitialiseToMain = chkXRefAppInitialises.Checked;
            mo.XRefAssertionsToMain = chkXRefCheckAssertions.Checked;
            mo.XRefErrorsToMain = chkXRefErrors.Checked;
            mo.XRefExceptionsToMain = chkXRefExceptions.Checked;
            mo.XRefLogsToMain = chkXRefLogs.Checked;
            mo.XRefMiniLogsToMain = chkXRefMiniLogs.Checked;
            mo.XRefResourceMessagesToMain = chkXRefResourceMessages.Checked;
            mo.XRefVerbLogsToMain = chkXRefVerbLogs.Checked;
            mo.XRefWarningsToMain = chkXRefWarnings.Checked;
            mo.BeautifyDisplayedStrings = chkBeautifyOutput.Checked;

            mo.IPAddressToBind = txtIPBinding.Text;
            mo.PortAddressToBind = int.Parse(txtPortBinding.Text);

            mo.MatchingNamePurgeAlsoClearsPartials = chkMatchingNamePurgeAlsoClearsPartials.Checked;

            mo.CrossProcessViewHighlight = chkHighlightCrossProcesses.Checked;
            mo.EnableBackTrace = chkEnableBacktrace.Checked;

            mo.TimingsViewIgnoresThreads = chkTimingsViewIgnoresThreads.Checked;

            if (rdoThreadShowDefault.Checked) {
                mo.ThreadDisplayOption = ThreadDisplayMode.DefaultUseThreadNameAndOS;
            } else if (rdoThreadShowNetId.Checked) {
                mo.ThreadDisplayOption = ThreadDisplayMode.UseDotNetThread;
            } else if (rdoThreadShowOSId.Checked) {
                mo.ThreadDisplayOption = ThreadDisplayMode.UseOSThread;
            } else if (rdoThreadShowFullInfo.Checked) {
                mo.ThreadDisplayOption = ThreadDisplayMode.ShowFullInformation;
            }

            mo.NoSecondsForUIUpdate = int.Parse(txtUIRefreshFrequency.Text);
            mo.NoUserNotificationsToStoreInLog = int.Parse(txtUserLogSize.Text);
            mo.NoSecondsForRefreshOnImport = int.Parse(txtLongRunningOps.Text);
            mo.UsePreferredNameInsteadOfProcessId = chkUseRenderNameNotPID.Checked;
            mo.DisplayInternalMessages = chkAllowInternalMessageDisplays.Checked;

            mo.FilterDefaultSaveClassLocation = chkFilterDefaultIncludesClasses.Checked; ;
            mo.FilterDefaultSaveThreads = chkFilterDefaultIncludesThreads.Checked;
            mo.FilterDefaultSaveLocations = chkFilterDefaultIncludesLocations.Checked;
            mo.FilterDefaultSaveModules = chkFilterDefaultIncludeModules.Checked;

            mo.PathsToCheckForImporters = lbxImportProcessPaths.Items.Cast<string>().ToArray();
            return mo;
        }

        private void FrmMexOptionsScreen_Load(object sender, EventArgs e) {
        }

        private void RecycleProcessWhenNameMatches_MouseEnter(object sender, EventArgs e) {
        }

        private void BrowseForFilterDir_Click(object sender, EventArgs e) {
            using var fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;
            if (fbd.ShowDialog() == DialogResult.OK) {
                txtFilterAndHighlightDir.Text = fbd.SelectedPath;
            }
        }

        private string filterDirInitialiseValue;

        private void OK_Click(object sender, EventArgs e) {
            // Relocate the filters if the directory has changed.
            //Bilge.E();
            try {
                string error = string.Empty;

                //Bilge.Assert(m_filterDirInitialiseValue != null, "The filter initialise value can not be null, this causes an error");

                if ((txtFilterAndHighlightDir.Text != filterDirInitialiseValue) && chkRelocateOnChange.Checked) {
                    string[] filenames = Directory.GetFiles(filterDirInitialiseValue, "*" + MexCore.TheCore.Options.FilterExtension);
                    foreach (string s in filenames) {
                        try {
                            // Try to relocate this file to the new directory that has been chosen by the all knowing user.
                            File.Move(Path.Combine(filterDirInitialiseValue, s), Path.Combine(txtFilterAndHighlightDir.Text, s));

                            // And then check for one of the zillion errors that could occur. Before i knew you were supposed to do it properly
                            // code like this was much faster to write, now it tages ages.
                        } catch (FileNotFoundException) {
                            //Bilge.Dump(fnfx, "Exception trying to relocate filters, trying to continue");
                            error += "File " + s + " could not be relocated." + Environment.NewLine;
                        } catch (UnauthorizedAccessException) {
                            //Bilge.Dump(uaxx, "Exception trying to relocate filters, trying to continue");
                            error += "File " + s + " could not be relocated." + Environment.NewLine;
                            break;
                        } catch (DirectoryNotFoundException) {
                            //Bilge.Dump(dnfx, "Exception trying to relocate filters, aborting operation");
                            error += "File " + s + " could not be relocated." + Environment.NewLine;
                            break;
                        } catch (NotSupportedException) {
                            //Bilge.Dump(nsx, "Exception trying to relocate filters, aborting operation");
                            error += "File " + s + " could not be relocated." + Environment.NewLine;
                            break;
                        } catch (PathTooLongException) {
                            //Bilge.Dump(ptlx, "Exception trying to relocate filters, aborting operation");
                            error += "File " + s + " could not be relocated." + Environment.NewLine;
                            break;
                        } catch (IOException) {
                            //Bilge.Dump(iox, "Exception trying to relocate filters, attempting to continue");
                            error += "File " + s + " could not be relocated." + Environment.NewLine;
                        }
                    } // End for each of the fitlers that are to be relocated
                } // IF the directorys did not match and the option to copy is ticked
            } finally {
                //Bilge.X();
            }
        }

        private readonly Settings formInitSettings = new();

        private bool thisFormBeingUpdated;  // Default false

        private void ReadEnvironment_Click(object sender, EventArgs e) {
            RefreshTexTabFromEnvironmentVariable();
        }

        private void RefreshTexTabFromEnvironmentVariable() {
            try {
                lblErrorMsg.Text = string.Empty;

                string envVar = Environment.GetEnvironmentVariable(Settings.ENVIRONMENTVARIABLENAME, EnvironmentVariableTarget.Machine);
                if (envVar != null) {
                    formInitSettings.PopulateFromString(envVar);
                    RefreshOptionsScreenFromSettings();
                } else {
                    //Bilge.Log("The environment variable could not be read, not altering screen information");
                    MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.OptionsConfiguraitonError, UserMessageType.WarningMessage, "Unable to retrieve the Tex Environment Variable, it does not appear to be set for this machine");
                }
            } catch (SecurityException sx) {
                //Bilge.Dump(sx, "Security Exception retrieving the environment name");
                lblErrorMsg.Text = "Security Exception, could not read environment";
                MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.OptionsConfiguraitonError, UserMessageType.ErrorMessage, "Security Exception reading Tex Environment Variable >>" + sx.Message);
            } catch (ArgumentException ax) {
                //Bilge.Dump(ax, "Argument Exception retrieving the environment value");
                lblErrorMsg.Text = "Argument Exception, the environment variable was in the wrong format";
                MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.OptionsConfiguraitonError, UserMessageType.ErrorMessage, "Argument Exception reading Tex Environment Variable >>" + ax.Message);
            }
        }

        /// <summary>
        /// Applys a changed structure setting to the form that is used to display the information.  This method first disables
        /// events that apply when the settings are being updated and then makes the screen display the settings according to
        /// the newly applied settings.
        /// </summary>
        private void RefreshOptionsScreenFromSettings() {
            thisFormBeingUpdated = true;
            try {

                #region apply the trace level to the radio group

                switch (formInitSettings.CurrentTraceLevel) {
                    case TraceLevel.Error:
                        rdoErrorLevel.Checked = true;
                        break;

                    case TraceLevel.Warning:
                        rdoWarningLevel.Checked = true;
                        break;

                    case TraceLevel.Info:
                        rdoTraceInfo.Checked = true;
                        break;

                    case TraceLevel.Verbose:
                        rdoVerboseLevel.Checked = true;
                        break;

                    default:
                        rdoTraceOff.Checked = true;
                        break;
                }

                #endregion apply the trace level to the radio group

                lbxAddTCPListeners.Items.Clear();

                foreach (string s in formInitSettings.ListenersToAdd) {
                    if (s.StartsWith("TEX")) {
                        chkAddTexListener.Checked = true;
                    }
                    if (s.StartsWith("TCP")) {
                        _ = lbxAddTCPListeners.Items.Add(s);
                    }
                }
                chkAddStackInfo.Checked = formInitSettings.AddStackInformation;
                chkUseHighPerf.Checked = formInitSettings.QueueMessages;
                chkRemoveAll.Checked = formInitSettings.ClearListenersOnStartup;
                chkEnableEnhancements.Checked = formInitSettings.EnableEnhancements;
                txtGeneratedString.Text = formInitSettings.ToString();

                lbxImportProcessPaths.Items.Clear();
                foreach (string s in formInitSettings.ImporterPathsToCheck) {
                    _ = lbxImportProcessPaths.Items.Add(s);
                }

            } finally {
                thisFormBeingUpdated = false;
            }
        }

        /// <summary>
        /// Repopulates the internal TexInitSettings class using all of the currently selected values on the form,
        /// this should be called after every form change to ensure that the m_formsSettings strucutre has the
        /// correct values for the form.
        /// </summary>
        private void RefreshStructureFromForm() {
            if (thisFormBeingUpdated) { return; }  // In the middle of an update from a set

            txtGeneratedString.Text = formInitSettings.ToString();

            if (rdoErrorLevel.Checked) { formInitSettings.CurrentTraceLevel = TraceLevel.Error; }
            if (rdoTraceInfo.Checked) { formInitSettings.CurrentTraceLevel = TraceLevel.Info; }
            if (rdoVerboseLevel.Checked) { formInitSettings.CurrentTraceLevel = TraceLevel.Verbose; }
            if (rdoWarningLevel.Checked) { formInitSettings.CurrentTraceLevel = TraceLevel.Warning; }
            if (rdoTraceOff.Checked) { formInitSettings.CurrentTraceLevel = TraceLevel.Off; }

            formInitSettings.ClearAddedListeners();
            foreach (string s in lbxAddTCPListeners.Items) {
                formInitSettings.AddListener(s);
            }

            if (chkAddTexListener.Checked) {
                formInitSettings.AddListener("TEX;");
            }

            formInitSettings.QueueMessages = chkUseHighPerf.Checked;
            formInitSettings.AddStackInformation = chkAddStackInfo.Checked;
            formInitSettings.EnableEnhancements = chkEnableEnhancements.Checked;
            formInitSettings.ClearListenersOnStartup = chkRemoveAll.Checked;

            // Make sure the correct string is displayed.
            txtGeneratedString.Text = formInitSettings.ToString();

            formInitSettings.ImporterPathsToCheck = lbxImportProcessPaths.Items.Cast<string>().ToList();
        }

        private void SetEnvironment_Click(object sender, EventArgs e) {
            var curr = Cursor.Current; ;
            try {
                Cursor.Current = Cursors.WaitCursor;

                RefreshStructureFromForm();

                txtGeneratedString.Text = formInitSettings.ToString();
                Environment.SetEnvironmentVariable(Settings.ENVIRONMENTVARIABLENAME, txtGeneratedString.Text, EnvironmentVariableTarget.Machine);
                string expandAssertions = chkExpandAssertions.Checked ? "True" : "False";
                Environment.SetEnvironmentVariable("TEXASSERTEXPAND", expandAssertions, EnvironmentVariableTarget.Machine);
                Cursor.Current = curr;
            } catch (SecurityException) {
                //Bilge.Warning("This user does not have sufficient rights to update the environment.");
                //Bilge.Dump(ax, "Failed to update the environment variable");
                _ = MessageBox.Show("Environment variable could not be set.  You do not have sufficient access rights.");
                return;
            } finally {
                Cursor.Current = curr;
            }
            _ = MessageBox.Show("Environment variable stored.");
        }

        private void AddNewListener_Click(object sender, EventArgs e) {
            txtIPInit.Text = txtIPInit.Text.Trim();
            txtPortInit.Text = txtPortInit.Text.Trim();

            if ((txtIPInit.Text.Length == 0) && (MessageBox.Show("The IP address appears to be invalid when the TCP listener is set.  This will cause no trace to be stored- contine?", "Invalid Settings", MessageBoxButtons.YesNo) == DialogResult.No)) {
                return;
            }
            if ((txtPortInit.Text.Length == 0) && (MessageBox.Show("The port value appears to be invalid when the TCP listener is set.  This will cause no trace to be stored- contine?", "Invalid Settings", MessageBoxButtons.YesNo) == DialogResult.No)) {
                return;
            }

            string tcpListenerText = "TCP;" + txtIPInit.Text + "," + txtPortInit.Text;
            if (chkMakeTcpInteractive.Checked) { tcpListenerText += ",INTERACTIVE"; }
            ;
            _ = lbxAddTCPListeners.Items.Add(tcpListenerText);

            RefreshStructureFromForm();
        }

        private void RemoveSelected_Click(object sender, EventArgs e) {
            if (lbxAddTCPListeners.SelectedIndex >= 0) {
                lbxAddTCPListeners.Items.RemoveAt(lbxAddTCPListeners.SelectedIndex);
            }
            RefreshStructureFromForm();
        }

        private void TexOptionsScreen_CheckedChanged(object sender, EventArgs e) {
            //Common handler for the tickboxes on the Tex options part of the options screen.
            RefreshStructureFromForm();
        }

        private void DevelopmentDefaults_Click(object sender, EventArgs e) {
            rdoVerboseLevel.Checked = true;
            chkRemoveAll.Checked = true;
            lbxAddTCPListeners.Items.Clear();
            chkAddTexListener.Checked = true;
            txtIPInit.Text = txtPortInit.Text = string.Empty;
            chkEnableEnhancements.Checked = true;
            chkExpandAssertions.Checked = true;
            chkUseHighPerf.Checked = true;
            chkAddStackInfo.Checked = true;
        }

        private void ReleaseDefaults_Click(object sender, EventArgs e) {
            rdoTraceOff.Checked = true;
            chkRemoveAll.Checked = true;
            chkAddTexListener.Checked = false;
            lbxAddTCPListeners.Items.Clear();
            txtIPInit.Text = txtPortInit.Text = string.Empty;
            chkEnableEnhancements.Checked = false;
            chkExpandAssertions.Checked = false;
            chkUseHighPerf.Checked = false;
            chkAddStackInfo.Checked = false;
        }

        private void IPInit_TextChanged(object sender, EventArgs e) {
            btnAddNewListener.Enabled = (txtIPInit.Text.Trim().Length > 0) && (txtPortInit.Text.Trim().Length > 0);
        }

        private void AddTCPListeners_SelectedIndexChanged(object sender, EventArgs e) {
            btnRemoveSelected.Enabled = lbxAddTCPListeners.SelectedIndex >= 0;
        }

        #region support for the environment setting and unsetting part of the options

        /*
        private void btnSetEnvironment_Click(object sender, EventArgs e) {
        }

        private void InitialiseTexSettingTab() {
           //Bilge.Log("Initialising Tex Settings Tab.");

            m_formsSettings = new TexInitSettings();

            // This is required so that we do not supply the name of this tool as the default application name.
            m_formsSettings.ResetForConfigurationTool();

           //Bilge.Log("Reading configuration inforamtion from the environment");
            m_formsSettings.PopulateFromEnvironmentVariable();

           //Bilge.Log("Environment configuration now set to" + m_formsSettings.ToString());
            // Put the initial values on the form.
            RefreshFormFromStructure();
        }
*/

        #endregion support for the environment setting and unsetting part of the options

        /// <summary>
        /// This method is essentially a massive switch statement to provide the help for hovering over the controls that are on the options
        /// screen. It uses the sender to identify which message is to be displayed and while this is a little crude it seemed over complex
        /// to bounce it through a enum.
        /// </summary>
        /// <remarks>This method is designed to be hooked up to the mouse hover for all of the controls</remarks>
        /// <param name="sender">The control which has triggered the hover event</param>
        /// <param name="e">The arguments provided with the mouse hover</param>
        private void Generic_MouseHover(object sender, EventArgs e) {
            string helpText;

            var ahp = new AppHelpProvider(new ResourceAccessor());
            string identifer = ((Control)sender).Name;
            helpText = ahp.GetHelpDescription(identifer);
            if (string.IsNullOrEmpty(helpText)) {
                //Bilge.Warning("Missing help text for " + identifer);
                helpText = "No help available, raise a bug for the control which you are expecting help for. (" + identifer + ")";
            }
            lblHelp.Text = helpText;
        }

        private void UIRefreshFrequency_TextChanged(object sender, EventArgs e) {
            try {
                lblRefreshWarning.Visible = false;

                string s = txtUIRefreshFrequency.Text;
                int i = int.Parse(s);
                if (i > 5) {
                    lblRefreshWarning.Text = "N.B. If your UI refresh is set too high you will not recieve real time notification of trace messages.";
                    lblRefreshWarning.Visible = true;
                }
            } catch (FormatException) {
            }
        }

        private void BtnAddPath_Click(object sender, EventArgs e) {
            if (!string.IsNullOrWhiteSpace(txtAdditionalPath.Text)) {
                _ = lbxImportProcessPaths.Items.Add(txtAdditionalPath.Text.Trim());
            }
            RefreshStructureFromForm();
        }

        private void BtnRemovePath_Click(object sender, EventArgs e) {
            lbxImportProcessPaths.Items.RemoveAt(lbxImportProcessPaths.SelectedIndex);
            RefreshStructureFromForm();
        }
    }
}