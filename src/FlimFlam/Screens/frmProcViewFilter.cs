//using Plisky.Diagnostics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Plisky.Diagnostics;
using Plisky.Diagnostics.FlimFlam;

namespace Plisky.FlimFlam {

    /// <summary>
    /// Summary description for frmProcViewFilter.
    /// </summary>
    internal class frmProcViewFilter : System.Windows.Forms.Form {
        internal bool FullRefreshRequired { get; set; }

        // Determines when tickboxes being changed programatically
        private bool autochanging;

        private System.Windows.Forms.Button btnCancel;
        private Button btnClearAdditionalLocFilter;
        private Button btnClearLocClassOverflow;
        private Button btnClearUnmatchedModules;
        private Button btnClearUnmatchedThreads;
        private Button btnLoadFilterConfiguration;
        private Button btnLoadMexDefaultFilter;
        private System.Windows.Forms.Button btnOK;
        private Button btnRefreshFilterDir;
        private Button btnSaveFilterConfiguration;
        private Button btnSetDefaultFilter;
        private ComboBox cboQuickLoad;
        private CheckBox chkAllowOverwrite;
        private CheckBox chkCaseSensitive;
        private CheckBox chkEnableIndexFilter;
        private CheckBox chkmtfAssertionMsgs;
        private CheckBox chkmtfChainMsgs;
        private CheckBox chkmtfCommandMsgs;
        private CheckBox chkmtfErrorMsgs;
        private CheckBox chkmtfExceptionContent;
        private CheckBox chkmtfExceptionHeaders;
        private CheckBox chkmtfIncludeLogs;
        private CheckBox chkmtfInternalMessages;
        private CheckBox chkmtfResourceMessages;
        private CheckBox chkmtfSessionMsgs;
        private CheckBox chkmtfTraceEnterMsgs;
        private CheckBox chkmtfTraceExitMsgs;
        private CheckBox chkmtfVerboseLogMessages;
        private CheckBox chkmtfWarningMsgs;
        private CheckBox chkSaveClassLocationInfo;
        private CheckBox chkSaveIncludeThreads;
        private CheckBox chkSaveLocationInformation;
        private CheckBox chkSaveModules;
        private CheckBox chkUseFilterOnLoad;
        private CheckedListBox clbAdditionalLocationsFilter;
        private CheckedListBox clbLocationClassFilter;
        private CheckedListBox clbModulesFilter;
        private CheckedListBox clbThreadsFilter;
        private readonly System.ComponentModel.IContainer components;
        private GroupBox groupBox1;
        private int higherIndexValue = int.MaxValue;
        private Label label1;
        private Label label2;
        private Label label4;
        private Label label5;
        private Label label7;
        private Label label8;
        private Label lblSaveData;
        private ListBox lbxAllMatchedFilters;
        private ListBox lbxLocAdditionalOverflow;
        private ListBox lbxLocClassOverflow;
        private ListBox lbxUnmatchedModules;
        private ListBox lbxUnmatchedThreads;
        private int lowerIndexValue = int.MinValue;
        private Panel panel2;
        private Panel pnlDockSplitContainer;
        private Panel pnlFilterButtons;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private TabPage tabLocationFiltering;
        private TabPage tabOptionsEvents;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private TabControl tbcFilterDetails;
        private TextBox txtExcludeAllAboveThisIndex;
        private TextBox txtExcludeAllBelowThisIndex;
        private TextBox txtFilterDirectory;
        private TextBox txtFilterExcludeStrings;
        private TextBox txtFilterExtension;
        private TextBox txtFilterFilename;
        private TextBox txtFilterIncludeStrings;
        //private Bilge b;

        internal frmProcViewFilter() {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //b = new Bilge();

            // Populate the event type filter box here
            // Get the image list and set the icons - point them all that the static instance the form has
            chkmtfChainMsgs.ImageList = chkmtfCommandMsgs.ImageList = chkmtfErrorMsgs.ImageList = chkmtfExceptionContent.ImageList =
            chkmtfExceptionHeaders.ImageList = chkmtfIncludeLogs.ImageList = chkmtfInternalMessages.ImageList =
            chkmtfResourceMessages.ImageList = chkmtfSessionMsgs.ImageList = chkmtfTraceEnterMsgs.ImageList = chkmtfTraceExitMsgs.ImageList =
            chkmtfVerboseLogMessages.ImageList = chkmtfWarningMsgs.ImageList = chkmtfAssertionMsgs.ImageList = frmMexMainView.AppImages;

            chkmtfAssertionMsgs.ImageIndex = MexCore.TheCore.ViewManager.ImageIndexFromEventType(TraceCommandTypes.AssertionFailed);
            chkmtfChainMsgs.ImageIndex = MexCore.TheCore.ViewManager.ImageIndexFromEventType(TraceCommandTypes.MoreInfo);
            chkmtfCommandMsgs.ImageIndex = MexCore.TheCore.ViewManager.ImageIndexFromEventType(TraceCommandTypes.CommandOnly);
            chkmtfErrorMsgs.ImageIndex = MexCore.TheCore.ViewManager.ImageIndexFromEventType(TraceCommandTypes.ErrorMsg);
            chkmtfExceptionContent.ImageIndex = MexCore.TheCore.ViewManager.ImageIndexFromEventType(TraceCommandTypes.ExceptionData);
            chkmtfExceptionHeaders.ImageIndex = MexCore.TheCore.ViewManager.ImageIndexFromEventType(TraceCommandTypes.ExceptionBlock);
            chkmtfIncludeLogs.ImageIndex = MexCore.TheCore.ViewManager.ImageIndexFromEventType(TraceCommandTypes.LogMessage);
            chkmtfInternalMessages.ImageIndex = MexCore.TheCore.ViewManager.ImageIndexFromEventType(TraceCommandTypes.InternalMsg);
            chkmtfResourceMessages.ImageIndex = MexCore.TheCore.ViewManager.ImageIndexFromEventType(TraceCommandTypes.ResourceCount);
            chkmtfSessionMsgs.ImageIndex = MexCore.TheCore.ViewManager.ImageIndexFromEventType(TraceCommandTypes.SectionStart);
            chkmtfTraceEnterMsgs.ImageIndex = MexCore.TheCore.ViewManager.ImageIndexFromEventType(TraceCommandTypes.TraceMessageIn);
            chkmtfTraceExitMsgs.ImageIndex = MexCore.TheCore.ViewManager.ImageIndexFromEventType(TraceCommandTypes.TraceMessageOut);
            chkmtfVerboseLogMessages.ImageIndex = MexCore.TheCore.ViewManager.ImageIndexFromEventType(TraceCommandTypes.LogMessageVerb);
            chkmtfWarningMsgs.ImageIndex = MexCore.TheCore.ViewManager.ImageIndexFromEventType(TraceCommandTypes.WarningMsg);

            chkSaveClassLocationInfo.Checked = MexCore.TheCore.Options.FilterDefaultSaveClassLocation;
            chkSaveIncludeThreads.Checked = MexCore.TheCore.Options.FilterDefaultSaveThreads;
            chkSaveModules.Checked = MexCore.TheCore.Options.FilterDefaultSaveModules;
            chkSaveLocationInformation.Checked = MexCore.TheCore.Options.FilterDefaultSaveLocations;

            // Find any known filters that are saved
            //Bilge.Assert(MexCore.TheCore.Options.FilterAndHighlightStoreDirectory != null, "there is no filter directory");
            //Bilge.Assert(Directory.Exists(MexCore.TheCore.Options.FilterAndHighlightStoreDirectory), "The preferred location for loading filters from does not exist and is not null.  If this doesent exist it should be null for the application");

            InitialiseLoadSaveScreen();
        }

        internal ViewFilter GetFilterFromDialog() {
            //Bilge.E();

            ViewFilter result;

            try {
                // looks through the dialog settings and gets the filter.
                result = new ViewFilter();
                //result.SetMessageTypeInclude();
                result.BeginFilterUpdate();
                try {
                    //Bilge.Log("Updating Filter with included types of message");
                    result.SetAllMessageTypesToNotIncluded();

                    UpdateViewFilterFromCheckedTypes(result);

                    //Bilge.VerboseLog("Updating filter with inclusion and exclusion strings");
                    // Sort out any inclusion / exclusion strings
                    string[] setIncludes = null;
                    string[] setExcludes = null;

                    if ((txtFilterIncludeStrings.Text.Length > 0) && (txtFilterIncludeStrings.Text != "*")) {
                        // there is a include filter to worry about.
                        setIncludes = txtFilterIncludeStrings.Text.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    }

                    if (txtFilterExcludeStrings.Text.Length > 0) {
                        // there is a include filter to worry about.
                        setExcludes = txtFilterExcludeStrings.Text.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    }

                    result.SetIncludeExcludeStrings(setIncludes, setExcludes, chkCaseSensitive.Checked);

                    if (chkEnableIndexFilter.Checked) {
                        result.SetIndexFilters(true, lowerIndexValue.ToString(), higherIndexValue.ToString());
                    }

                    //Bilge.VerboseLog("Updating filter with thread/  module / location exclusions etc");
                    // Now look at thread and module and location excludes. The screen for this shows them as Includes (you tick what is to be included) but the
                    // actual filter shows them as excludes so that new locations and so on do actually show up.  This way round is less intuitive but does stop
                    // the problem where data appears to be missing which in the end increases confidence in the logger.
                    List<KeyDisplayRepresentation> threadExcludes = null;
                    List<string> moduleExcludes = null;
                    List<string> additionalLocationExcludes = null;
                    List<string> additionalLocationClassExcludes = null;

                    if (clbThreadsFilter.CheckedItems.Count != clbThreadsFilter.Items.Count) {
                        //Bilge.VerboseLog("Looking at the threads filter that was ticked.  clbThreadsfilter.CheckedItems.Count >0");
                        threadExcludes = [];
                        foreach (KeyDisplayRepresentation s in clbThreadsFilter.Items) {
                            if (!clbThreadsFilter.CheckedItems.Contains(s)) {
                                threadExcludes.Add(s);
                            }
                        }
                    } // End if we are filtering by thread ID.

                    if (clbModulesFilter.CheckedItems.Count != clbModulesFilter.Items.Count) {
                        //Bilge.VerboseLog("Looking at the modules filter that was ticked, clbModulesFilter.Checked.Count > 0");
                        moduleExcludes = [];

                        foreach (string s in clbModulesFilter.Items) {
                            if (!clbModulesFilter.CheckedItems.Contains(s)) {
                                moduleExcludes.Add(s);
                            }
                        }
                    }

                    if (clbAdditionalLocationsFilter.CheckedItems.Count != clbAdditionalLocationsFilter.Items.Count) {
                        //Bilge.VerboseLog("Looking at the additional locations that were ticked, AdditionalLocationsFilter.Checked.Count >0");

                        additionalLocationExcludes = [];

                        foreach (string s in clbAdditionalLocationsFilter.Items) {
                            if (!clbAdditionalLocationsFilter.CheckedItems.Contains(s)) {
                                additionalLocationExcludes.Add(s);
                            }
                        }
                    }

                    if (clbLocationClassFilter.CheckedItems.Count != clbLocationClassFilter.Items.Count) {
                        //Bilge.VerboseLog("Looking at the class location filter, clbLocationClassFilter.Checked.Count > 0");
                        additionalLocationClassExcludes = [];
                        foreach (string s in clbLocationClassFilter.Items) {
                            if (!clbLocationClassFilter.CheckedItems.Contains(s)) {
                                additionalLocationClassExcludes.Add(s);
                            }
                        }
                    }

                    //Bilge.VerboseLog("Setting the threads, moudles, locations, aditional locations into the filter");
                    result.SetThreadsModulesLocations(threadExcludes, moduleExcludes, additionalLocationExcludes, additionalLocationClassExcludes);

                    //Bilge.Log("Filter update completes");
                } finally {
                    result.EndFilterUpdate();
                }
            } finally {
                //Bilge.X();
            }

            // all done?!
            return result;
        }

        internal void InitialiseForTracedApplication() {
            // Populate the dialog box for the selected application
            var threadDisplayNames = MexCore.TheCore.ViewManager.GetThreadNameListForSelectedApp();
            var moduleDisplayNames = MexCore.TheCore.ViewManager.GetAllModules();
            MexCore.TheCore.ViewManager.GetAdditionalLocationsAndClasses(out _, out var additionalLocationClasses);

            clbThreadsFilter.Items.Clear();
            clbModulesFilter.Items.Clear();

            if (threadDisplayNames != null) {
                clbThreadsFilter.Items.AddRange(threadDisplayNames.ToArray());
            }
            if (moduleDisplayNames != null) {
                clbModulesFilter.Items.AddRange(moduleDisplayNames.ToArray());
            }
            if (threadDisplayNames != null) {
                clbAdditionalLocationsFilter.Items.AddRange(threadDisplayNames.ToArray());
            }
            if (additionalLocationClasses != null) {
                clbLocationClassFilter.Items.AddRange(additionalLocationClasses.ToArray());
            }
        }

        /// <summary>
        /// This will take the contents of an existing filter and update the dialog with those contents so that it displays the correct information for that filter.
        /// This routine will also check the currently selected appliation to ensure that the correct ticks etc are used when this type of filter is applied.
        /// </summary>
        /// <param name="currentFilter">The filter that data is to be loaded from</param>
        internal void InitialiseFromExistingFilter(ViewFilter currentFilter) {
            //b.Info.E();
            try {
                //Bilge.Log("Initialising the screen from an existing filter", currentFilter.ToString());

                txtFilterExcludeStrings.Text = currentFilter.GetExclusionStrings();
                txtFilterIncludeStrings.Text = currentFilter.GetInclusionStrings();

                //Bilge.VerboseLog("About to pull information from the selected application itself");
                if (MexCore.TheCore.ViewManager.SelectedTracedAppIdx != -1) {
                    // Helper functions to add strings to checked list boxes, ticking
                    PolpulateCheckedListBoxFromStringLists(clbModulesFilter, lbxUnmatchedModules, MexCore.TheCore.ViewManager.GetAllModules(), currentFilter.GetModuleExclusionNames());
                    PolpulateCheckedListBoxFromKeyDisplay(clbThreadsFilter, lbxUnmatchedThreads, MexCore.TheCore.ViewManager.GetThreadNameListForSelectedApp(), currentFilter.GetThreadExclusionNames());

                    // Now get all of the known locations and the known class locations and add them to their cboListboxes.
                    var additionalLocations = MexCore.TheCore.ViewManager.GetAllAdditionalLocations();
                    PolpulateCheckedListBoxFromStringLists(clbAdditionalLocationsFilter, lbxLocAdditionalOverflow, additionalLocations, currentFilter.GetAdditonalLocationExclusions());
                    PolpulateCheckedListBoxFromStringLists(clbLocationClassFilter, lbxLocClassOverflow, MexCore.TheCore.ViewManager.GetAdditionalLocationClassesFromAdditionalLocations(additionalLocations), currentFilter.GetAdditionalLocationClassExclusions());
                } else {
                    //Bilge.VerboseLog("There was no selected application, defaulting things like the threads and modules etc");
                    clbModulesFilter.Items.Clear();
                    clbThreadsFilter.Items.Clear();
                    clbAdditionalLocationsFilter.Items.Clear();
                }

                UpdateCheckedTypesFromViewFilter(currentFilter);

                //Bilge.VerboseLog("Setting up the index filter information");


                if (currentFilter.GetIndexFilters(out string belowidx, out string aboveidx)) {
                    chkEnableIndexFilter.Checked = true;
                    if (belowidx.Length == 0) { belowidx = "0"; }
                    if (aboveidx.Length == 0) { aboveidx = long.MaxValue.ToString(); }
                    txtExcludeAllBelowThisIndex.Text = belowidx;
                    txtExcludeAllAboveThisIndex.Text = aboveidx;
                } else {
                    chkEnableIndexFilter.Checked = false;
                    txtExcludeAllBelowThisIndex.Text = "0";
                    txtExcludeAllAboveThisIndex.Text = MexCore.TheCore.DataManager.LasatUsedGlobalIndexWas.ToString();
                }

                //Bilge.Log("Initialisation from existing filter completes");
            } finally {
                //Bilge.X();
            }
        }

        /// <summary>
        /// Populates a checked list box taking all of the items in completeListOfAllItems and ticking the ones which appear in thoseItems to be ticked, so that the
        /// list in the ticked listbox is initialised with the values of those in thoseItemsToBeTicked
        /// <remarks> Any elements in thosetoBeTicked which are not in completeListOfAllItems will be added to the overflow ListBox if its not null</remarks>
        /// </summary>
        /// <param name="target">The checked list box to fill</param>
        /// <param name="overflow">A Listbox of any ticked items that arent in the main list</param>
        /// <param name="completeListOfAllItems">The complete list of items to add to the checked list box</param>
        /// <param name="thoseItemsToBeTicked">The list of items that are to be ticked when added to the complete list box</param>
        internal void PolpulateCheckedListBoxFromKeyDisplay(CheckedListBox target, ListBox overflow, List<KeyDisplayRepresentation> completeListOfAllItems, List<KeyDisplayRepresentation> thoseItemsToBeTicked) {
            target.Items.Clear();
            overflow.Items.Clear();

            if (overflow != null) {
                foreach (var s in thoseItemsToBeTicked) {
                    if (!completeListOfAllItems.Contains(s)) {
                        _ = overflow.Items.Add(s);
                    }
                }
            }

            foreach (var s in completeListOfAllItems) {
                if (thoseItemsToBeTicked.Contains(s)) {
                    _ = target.Items.Add(s, false);
                } else {
                    _ = target.Items.Add(s, true);
                }
            }
        }

        /// <summary>
        /// Populates a checked list box taking all of the items in completeListOfAllItems and ticking the ones which appear in thoseItems to be ticked, so that the
        /// list in the ticked listbox is initialised with the values of those in thoseItemsToBeTicked
        /// <remarks> Any elements in thosetoBeTicked which are not in completeListOfAllItems will be added to the overflow ListBox if its not null</remarks>
        /// </summary>
        /// <param name="target">The checked list box to fill</param>
        /// <param name="overflow">A Listbox of any ticked items that arent in the main list</param>
        /// <param name="completeListOfAllItems">The complete list of items to add to the checked list box</param>
        /// <param name="thoseItemsToBeTicked">The list of items that are to be ticked when added to the complete list box</param>
        internal void PolpulateCheckedListBoxFromStringLists(CheckedListBox target, ListBox overflow, List<string> completeListOfAllItems, List<string> thoseItemsToBeTicked) {
            target.Items.Clear();
            overflow.Items.Clear();

            if (overflow != null) {
                foreach (string s in thoseItemsToBeTicked) {
                    if (!completeListOfAllItems.Contains(s)) {
                        _ = overflow.Items.Add(s);
                    }
                }
            }

            foreach (string s in completeListOfAllItems) {
                if (thoseItemsToBeTicked.Contains(s)) {
                    _ = target.Items.Add(s, false);
                } else {
                    _ = target.Items.Add(s, true);
                }
            }
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProcViewFilter));
            btnOK = new Button();
            btnCancel = new Button();
            btnLoadMexDefaultFilter = new Button();
            tbcFilterDetails = new TabControl();
            tabOptionsEvents = new TabPage();
            pnlFilterButtons = new Panel();
            label8 = new Label();
            chkmtfWarningMsgs = new CheckBox();
            chkmtfExceptionHeaders = new CheckBox();
            chkmtfTraceExitMsgs = new CheckBox();
            chkmtfTraceEnterMsgs = new CheckBox();
            chkmtfErrorMsgs = new CheckBox();
            chkmtfAssertionMsgs = new CheckBox();
            chkmtfVerboseLogMessages = new CheckBox();
            chkmtfIncludeLogs = new CheckBox();
            chkCaseSensitive = new CheckBox();
            label4 = new Label();
            txtFilterExcludeStrings = new TextBox();
            label5 = new Label();
            txtFilterIncludeStrings = new TextBox();
            tabPage2 = new TabPage();
            pnlDockSplitContainer = new Panel();
            splitContainer1 = new SplitContainer();
            clbModulesFilter = new CheckedListBox();
            clbThreadsFilter = new CheckedListBox();
            lbxUnmatchedModules = new ListBox();
            lbxUnmatchedThreads = new ListBox();
            btnClearUnmatchedModules = new Button();
            btnClearUnmatchedThreads = new Button();
            tabLocationFiltering = new TabPage();
            splitContainer2 = new SplitContainer();
            clbAdditionalLocationsFilter = new CheckedListBox();
            clbLocationClassFilter = new CheckedListBox();
            lbxLocAdditionalOverflow = new ListBox();
            lbxLocClassOverflow = new ListBox();
            btnClearAdditionalLocFilter = new Button();
            btnClearLocClassOverflow = new Button();
            tabPage3 = new TabPage();
            panel2 = new Panel();
            label7 = new Label();
            chkmtfSessionMsgs = new CheckBox();
            chkmtfChainMsgs = new CheckBox();
            chkmtfCommandMsgs = new CheckBox();
            chkmtfInternalMessages = new CheckBox();
            chkmtfResourceMessages = new CheckBox();
            chkmtfExceptionContent = new CheckBox();
            label2 = new Label();
            txtExcludeAllAboveThisIndex = new TextBox();
            txtExcludeAllBelowThisIndex = new TextBox();
            chkEnableIndexFilter = new CheckBox();
            label1 = new Label();
            tabPage4 = new TabPage();
            txtFilterExtension = new TextBox();
            chkUseFilterOnLoad = new CheckBox();
            lblSaveData = new Label();
            btnRefreshFilterDir = new Button();
            txtFilterFilename = new TextBox();
            txtFilterDirectory = new TextBox();
            groupBox1 = new GroupBox();
            chkSaveModules = new CheckBox();
            chkSaveClassLocationInfo = new CheckBox();
            chkSaveLocationInformation = new CheckBox();
            chkSaveIncludeThreads = new CheckBox();
            lbxAllMatchedFilters = new ListBox();
            chkAllowOverwrite = new CheckBox();
            btnLoadFilterConfiguration = new Button();
            btnSaveFilterConfiguration = new Button();
            btnSetDefaultFilter = new Button();
            cboQuickLoad = new ComboBox();
            tbcFilterDetails.SuspendLayout();
            tabOptionsEvents.SuspendLayout();
            pnlFilterButtons.SuspendLayout();
            tabPage2.SuspendLayout();
            pnlDockSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tabLocationFiltering.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            tabPage3.SuspendLayout();
            panel2.SuspendLayout();
            tabPage4.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // btnOK
            // 
            btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOK.DialogResult = DialogResult.OK;
            btnOK.Location = new Point(505, 472);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(89, 28);
            btnOK.TabIndex = 2;
            btnOK.Text = "OK";
            btnOK.Click += OKWithFullRefresh_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(408, 472);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(90, 28);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            // 
            // btnLoadMexDefaultFilter
            // 
            btnLoadMexDefaultFilter.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnLoadMexDefaultFilter.DialogResult = DialogResult.OK;
            btnLoadMexDefaultFilter.Location = new Point(305, 472);
            btnLoadMexDefaultFilter.Name = "btnLoadMexDefaultFilter";
            btnLoadMexDefaultFilter.Size = new Size(89, 28);
            btnLoadMexDefaultFilter.TabIndex = 99;
            btnLoadMexDefaultFilter.Text = "Use Default";
            btnLoadMexDefaultFilter.Click += LoadMexDefaultFilter_Click;
            // 
            // tbcFilterDetails
            // 
            tbcFilterDetails.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tbcFilterDetails.Controls.Add(tabOptionsEvents);
            tbcFilterDetails.Controls.Add(tabPage2);
            tbcFilterDetails.Controls.Add(tabLocationFiltering);
            tbcFilterDetails.Controls.Add(tabPage3);
            tbcFilterDetails.Controls.Add(tabPage4);
            tbcFilterDetails.Font = new Font("Verdana", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tbcFilterDetails.Location = new Point(2, 4);
            tbcFilterDetails.Name = "tbcFilterDetails";
            tbcFilterDetails.SelectedIndex = 0;
            tbcFilterDetails.Size = new Size(600, 458);
            tbcFilterDetails.TabIndex = 106;
            // 
            // tabOptionsEvents
            // 
            tabOptionsEvents.Controls.Add(pnlFilterButtons);
            tabOptionsEvents.Controls.Add(chkCaseSensitive);
            tabOptionsEvents.Controls.Add(label4);
            tabOptionsEvents.Controls.Add(txtFilterExcludeStrings);
            tabOptionsEvents.Controls.Add(label5);
            tabOptionsEvents.Controls.Add(txtFilterIncludeStrings);
            tabOptionsEvents.Location = new Point(4, 22);
            tabOptionsEvents.Name = "tabOptionsEvents";
            tabOptionsEvents.Padding = new Padding(3);
            tabOptionsEvents.Size = new Size(592, 432);
            tabOptionsEvents.TabIndex = 0;
            tabOptionsEvents.Text = "Text / Type";
            tabOptionsEvents.UseVisualStyleBackColor = true;
            // 
            // pnlFilterButtons
            // 
            pnlFilterButtons.BackColor = SystemColors.Window;
            pnlFilterButtons.Controls.Add(label8);
            pnlFilterButtons.Controls.Add(chkmtfWarningMsgs);
            pnlFilterButtons.Controls.Add(chkmtfExceptionHeaders);
            pnlFilterButtons.Controls.Add(chkmtfTraceExitMsgs);
            pnlFilterButtons.Controls.Add(chkmtfTraceEnterMsgs);
            pnlFilterButtons.Controls.Add(chkmtfErrorMsgs);
            pnlFilterButtons.Controls.Add(chkmtfAssertionMsgs);
            pnlFilterButtons.Controls.Add(chkmtfVerboseLogMessages);
            pnlFilterButtons.Controls.Add(chkmtfIncludeLogs);
            pnlFilterButtons.Dock = DockStyle.Top;
            pnlFilterButtons.Location = new Point(3, 3);
            pnlFilterButtons.Name = "pnlFilterButtons";
            pnlFilterButtons.Size = new Size(586, 162);
            pnlFilterButtons.TabIndex = 124;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Verdana", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.Location = new Point(8, 12);
            label8.Name = "label8";
            label8.Size = new Size(354, 13);
            label8.TabIndex = 139;
            label8.Text = "Include / Exclude messages based on message type:";
            // 
            // chkmtfWarningMsgs
            // 
            chkmtfWarningMsgs.Appearance = Appearance.Button;
            chkmtfWarningMsgs.FlatAppearance.CheckedBackColor = Color.FromArgb(192, 255, 192);
            chkmtfWarningMsgs.FlatStyle = FlatStyle.Flat;
            chkmtfWarningMsgs.ImageAlign = ContentAlignment.MiddleRight;
            chkmtfWarningMsgs.ImageIndex = 31;
            chkmtfWarningMsgs.Location = new Point(389, 73);
            chkmtfWarningMsgs.Name = "chkmtfWarningMsgs";
            chkmtfWarningMsgs.Size = new Size(181, 29);
            chkmtfWarningMsgs.TabIndex = 132;
            chkmtfWarningMsgs.Text = "Warning Messages";
            chkmtfWarningMsgs.UseVisualStyleBackColor = true;
            // 
            // chkmtfExceptionHeaders
            // 
            chkmtfExceptionHeaders.Appearance = Appearance.Button;
            chkmtfExceptionHeaders.FlatAppearance.CheckedBackColor = Color.FromArgb(192, 255, 192);
            chkmtfExceptionHeaders.FlatStyle = FlatStyle.Flat;
            chkmtfExceptionHeaders.ImageAlign = ContentAlignment.MiddleRight;
            chkmtfExceptionHeaders.ImageIndex = 20;
            chkmtfExceptionHeaders.Location = new Point(390, 111);
            chkmtfExceptionHeaders.Name = "chkmtfExceptionHeaders";
            chkmtfExceptionHeaders.Size = new Size(181, 29);
            chkmtfExceptionHeaders.TabIndex = 130;
            chkmtfExceptionHeaders.Text = "Exception Headers";
            chkmtfExceptionHeaders.UseVisualStyleBackColor = true;
            // 
            // chkmtfTraceExitMsgs
            // 
            chkmtfTraceExitMsgs.Appearance = Appearance.Button;
            chkmtfTraceExitMsgs.FlatAppearance.CheckedBackColor = Color.FromArgb(192, 255, 192);
            chkmtfTraceExitMsgs.FlatStyle = FlatStyle.Flat;
            chkmtfTraceExitMsgs.ImageAlign = ContentAlignment.MiddleRight;
            chkmtfTraceExitMsgs.ImageIndex = 17;
            chkmtfTraceExitMsgs.Location = new Point(200, 111);
            chkmtfTraceExitMsgs.Name = "chkmtfTraceExitMsgs";
            chkmtfTraceExitMsgs.Size = new Size(183, 29);
            chkmtfTraceExitMsgs.TabIndex = 129;
            chkmtfTraceExitMsgs.Text = "Trace Exit";
            chkmtfTraceExitMsgs.UseVisualStyleBackColor = true;
            // 
            // chkmtfTraceEnterMsgs
            // 
            chkmtfTraceEnterMsgs.Appearance = Appearance.Button;
            chkmtfTraceEnterMsgs.FlatAppearance.CheckedBackColor = Color.FromArgb(192, 255, 192);
            chkmtfTraceEnterMsgs.FlatStyle = FlatStyle.Flat;
            chkmtfTraceEnterMsgs.ImageAlign = ContentAlignment.MiddleRight;
            chkmtfTraceEnterMsgs.ImageIndex = 15;
            chkmtfTraceEnterMsgs.Location = new Point(200, 73);
            chkmtfTraceEnterMsgs.Name = "chkmtfTraceEnterMsgs";
            chkmtfTraceEnterMsgs.Size = new Size(182, 29);
            chkmtfTraceEnterMsgs.TabIndex = 128;
            chkmtfTraceEnterMsgs.Text = "Trace Enter";
            chkmtfTraceEnterMsgs.UseVisualStyleBackColor = true;
            // 
            // chkmtfErrorMsgs
            // 
            chkmtfErrorMsgs.Appearance = Appearance.Button;
            chkmtfErrorMsgs.FlatAppearance.CheckedBackColor = Color.FromArgb(192, 255, 192);
            chkmtfErrorMsgs.FlatStyle = FlatStyle.Flat;
            chkmtfErrorMsgs.ImageAlign = ContentAlignment.MiddleRight;
            chkmtfErrorMsgs.ImageIndex = 30;
            chkmtfErrorMsgs.Location = new Point(389, 36);
            chkmtfErrorMsgs.Name = "chkmtfErrorMsgs";
            chkmtfErrorMsgs.Size = new Size(181, 29);
            chkmtfErrorMsgs.TabIndex = 127;
            chkmtfErrorMsgs.Text = "Error Messages";
            chkmtfErrorMsgs.UseVisualStyleBackColor = true;
            // 
            // chkmtfAssertionMsgs
            // 
            chkmtfAssertionMsgs.Appearance = Appearance.Button;
            chkmtfAssertionMsgs.FlatAppearance.CheckedBackColor = Color.FromArgb(192, 255, 192);
            chkmtfAssertionMsgs.FlatStyle = FlatStyle.Flat;
            chkmtfAssertionMsgs.ImageAlign = ContentAlignment.MiddleRight;
            chkmtfAssertionMsgs.ImageIndex = 11;
            chkmtfAssertionMsgs.Location = new Point(200, 36);
            chkmtfAssertionMsgs.Name = "chkmtfAssertionMsgs";
            chkmtfAssertionMsgs.Size = new Size(182, 29);
            chkmtfAssertionMsgs.TabIndex = 126;
            chkmtfAssertionMsgs.Text = "Assertion Failures";
            chkmtfAssertionMsgs.UseVisualStyleBackColor = true;
            // 
            // chkmtfVerboseLogMessages
            // 
            chkmtfVerboseLogMessages.Appearance = Appearance.Button;
            chkmtfVerboseLogMessages.FlatAppearance.CheckedBackColor = Color.FromArgb(192, 255, 192);
            chkmtfVerboseLogMessages.FlatStyle = FlatStyle.Flat;
            chkmtfVerboseLogMessages.ImageAlign = ContentAlignment.MiddleRight;
            chkmtfVerboseLogMessages.ImageIndex = 27;
            chkmtfVerboseLogMessages.Location = new Point(12, 73);
            chkmtfVerboseLogMessages.Name = "chkmtfVerboseLogMessages";
            chkmtfVerboseLogMessages.Size = new Size(181, 29);
            chkmtfVerboseLogMessages.TabIndex = 125;
            chkmtfVerboseLogMessages.Text = "Verbose Messages";
            chkmtfVerboseLogMessages.UseVisualStyleBackColor = true;
            // 
            // chkmtfIncludeLogs
            // 
            chkmtfIncludeLogs.Appearance = Appearance.Button;
            chkmtfIncludeLogs.FlatAppearance.CheckedBackColor = Color.FromArgb(192, 255, 192);
            chkmtfIncludeLogs.FlatStyle = FlatStyle.Flat;
            chkmtfIncludeLogs.ImageAlign = ContentAlignment.MiddleRight;
            chkmtfIncludeLogs.ImageIndex = 25;
            chkmtfIncludeLogs.Location = new Point(12, 36);
            chkmtfIncludeLogs.Name = "chkmtfIncludeLogs";
            chkmtfIncludeLogs.Size = new Size(181, 29);
            chkmtfIncludeLogs.TabIndex = 124;
            chkmtfIncludeLogs.Text = "Log Messages";
            chkmtfIncludeLogs.UseVisualStyleBackColor = true;
            // 
            // chkCaseSensitive
            // 
            chkCaseSensitive.Location = new Point(4, 299);
            chkCaseSensitive.Name = "chkCaseSensitive";
            chkCaseSensitive.Size = new Size(283, 30);
            chkCaseSensitive.TabIndex = 101;
            chkCaseSensitive.Text = "String matches are case sensitive";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Verdana", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(0, 246);
            label4.Name = "label4";
            label4.Size = new Size(471, 13);
            label4.TabIndex = 99;
            label4.Text = "Exclude any messages which contain one of these strings (; delimited):";
            // 
            // txtFilterExcludeStrings
            // 
            txtFilterExcludeStrings.Location = new Point(4, 266);
            txtFilterExcludeStrings.Name = "txtFilterExcludeStrings";
            txtFilterExcludeStrings.Size = new Size(584, 21);
            txtFilterExcludeStrings.TabIndex = 100;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Verdana", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(-4, 185);
            label5.Name = "label5";
            label5.Size = new Size(472, 13);
            label5.TabIndex = 98;
            label5.Text = "Only include messages which contain one of these strings (; delimited):";
            // 
            // txtFilterIncludeStrings
            // 
            txtFilterIncludeStrings.Location = new Point(4, 204);
            txtFilterIncludeStrings.Name = "txtFilterIncludeStrings";
            txtFilterIncludeStrings.Size = new Size(584, 21);
            txtFilterIncludeStrings.TabIndex = 97;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(pnlDockSplitContainer);
            tabPage2.Controls.Add(btnClearUnmatchedModules);
            tabPage2.Controls.Add(btnClearUnmatchedThreads);
            tabPage2.Location = new Point(4, 22);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(592, 432);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Threads / Modules";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // pnlDockSplitContainer
            // 
            pnlDockSplitContainer.Controls.Add(splitContainer1);
            pnlDockSplitContainer.Dock = DockStyle.Top;
            pnlDockSplitContainer.Location = new Point(3, 3);
            pnlDockSplitContainer.Name = "pnlDockSplitContainer";
            pnlDockSplitContainer.Size = new Size(586, 366);
            pnlDockSplitContainer.TabIndex = 7;
            // 
            // splitContainer1
            // 
            splitContainer1.BackColor = SystemColors.Control;
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(clbModulesFilter);
            splitContainer1.Panel1.Controls.Add(clbThreadsFilter);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(lbxUnmatchedModules);
            splitContainer1.Panel2.Controls.Add(lbxUnmatchedThreads);
            splitContainer1.Size = new Size(586, 366);
            splitContainer1.SplitterDistance = 281;
            splitContainer1.TabIndex = 0;
            // 
            // clbModulesFilter
            // 
            clbModulesFilter.CheckOnClick = true;
            clbModulesFilter.Dock = DockStyle.Fill;
            clbModulesFilter.FormattingEnabled = true;
            clbModulesFilter.Location = new Point(293, 0);
            clbModulesFilter.Name = "clbModulesFilter";
            clbModulesFilter.ScrollAlwaysVisible = true;
            clbModulesFilter.Size = new Size(293, 281);
            clbModulesFilter.TabIndex = 3;
            // 
            // clbThreadsFilter
            // 
            clbThreadsFilter.CheckOnClick = true;
            clbThreadsFilter.ColumnWidth = 110;
            clbThreadsFilter.Dock = DockStyle.Left;
            clbThreadsFilter.FormattingEnabled = true;
            clbThreadsFilter.Location = new Point(0, 0);
            clbThreadsFilter.Name = "clbThreadsFilter";
            clbThreadsFilter.ScrollAlwaysVisible = true;
            clbThreadsFilter.Size = new Size(293, 281);
            clbThreadsFilter.TabIndex = 2;
            // 
            // lbxUnmatchedModules
            // 
            lbxUnmatchedModules.Dock = DockStyle.Fill;
            lbxUnmatchedModules.FormattingEnabled = true;
            lbxUnmatchedModules.ItemHeight = 13;
            lbxUnmatchedModules.Location = new Point(293, 0);
            lbxUnmatchedModules.Name = "lbxUnmatchedModules";
            lbxUnmatchedModules.ScrollAlwaysVisible = true;
            lbxUnmatchedModules.Size = new Size(293, 81);
            lbxUnmatchedModules.TabIndex = 7;
            // 
            // lbxUnmatchedThreads
            // 
            lbxUnmatchedThreads.Dock = DockStyle.Left;
            lbxUnmatchedThreads.FormattingEnabled = true;
            lbxUnmatchedThreads.ItemHeight = 13;
            lbxUnmatchedThreads.Location = new Point(0, 0);
            lbxUnmatchedThreads.Name = "lbxUnmatchedThreads";
            lbxUnmatchedThreads.ScrollAlwaysVisible = true;
            lbxUnmatchedThreads.Size = new Size(293, 81);
            lbxUnmatchedThreads.TabIndex = 6;
            // 
            // btnClearUnmatchedModules
            // 
            btnClearUnmatchedModules.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnClearUnmatchedModules.Location = new Point(367, 391);
            btnClearUnmatchedModules.Name = "btnClearUnmatchedModules";
            btnClearUnmatchedModules.Size = new Size(214, 28);
            btnClearUnmatchedModules.TabIndex = 4;
            btnClearUnmatchedModules.Text = "Remove Unmatched Modules";
            btnClearUnmatchedModules.UseVisualStyleBackColor = true;
            // 
            // btnClearUnmatchedThreads
            // 
            btnClearUnmatchedThreads.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnClearUnmatchedThreads.Location = new Point(163, 387);
            btnClearUnmatchedThreads.Name = "btnClearUnmatchedThreads";
            btnClearUnmatchedThreads.Size = new Size(202, 28);
            btnClearUnmatchedThreads.TabIndex = 3;
            btnClearUnmatchedThreads.Text = "Remove Unmatched Threads";
            btnClearUnmatchedThreads.UseVisualStyleBackColor = true;
            // 
            // tabLocationFiltering
            // 
            tabLocationFiltering.Controls.Add(splitContainer2);
            tabLocationFiltering.Controls.Add(btnClearAdditionalLocFilter);
            tabLocationFiltering.Controls.Add(btnClearLocClassOverflow);
            tabLocationFiltering.Location = new Point(4, 22);
            tabLocationFiltering.Name = "tabLocationFiltering";
            tabLocationFiltering.Padding = new Padding(3);
            tabLocationFiltering.Size = new Size(598, 423);
            tabLocationFiltering.TabIndex = 2;
            tabLocationFiltering.Text = "Locations";
            tabLocationFiltering.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Top;
            splitContainer2.Location = new Point(3, 3);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(clbAdditionalLocationsFilter);
            splitContainer2.Panel1.Controls.Add(clbLocationClassFilter);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(lbxLocAdditionalOverflow);
            splitContainer2.Panel2.Controls.Add(lbxLocClassOverflow);
            splitContainer2.Size = new Size(592, 370);
            splitContainer2.SplitterDistance = 183;
            splitContainer2.TabIndex = 15;
            // 
            // clbAdditionalLocationsFilter
            // 
            clbAdditionalLocationsFilter.CheckOnClick = true;
            clbAdditionalLocationsFilter.Dock = DockStyle.Fill;
            clbAdditionalLocationsFilter.FormattingEnabled = true;
            clbAdditionalLocationsFilter.Location = new Point(258, 0);
            clbAdditionalLocationsFilter.Name = "clbAdditionalLocationsFilter";
            clbAdditionalLocationsFilter.ScrollAlwaysVisible = true;
            clbAdditionalLocationsFilter.Size = new Size(334, 183);
            clbAdditionalLocationsFilter.TabIndex = 11;
            // 
            // clbLocationClassFilter
            // 
            clbLocationClassFilter.CheckOnClick = true;
            clbLocationClassFilter.Dock = DockStyle.Left;
            clbLocationClassFilter.FormattingEnabled = true;
            clbLocationClassFilter.Location = new Point(0, 0);
            clbLocationClassFilter.Name = "clbLocationClassFilter";
            clbLocationClassFilter.ScrollAlwaysVisible = true;
            clbLocationClassFilter.Size = new Size(258, 183);
            clbLocationClassFilter.TabIndex = 12;
            // 
            // lbxLocAdditionalOverflow
            // 
            lbxLocAdditionalOverflow.Dock = DockStyle.Fill;
            lbxLocAdditionalOverflow.Enabled = false;
            lbxLocAdditionalOverflow.FormattingEnabled = true;
            lbxLocAdditionalOverflow.ItemHeight = 13;
            lbxLocAdditionalOverflow.Location = new Point(258, 0);
            lbxLocAdditionalOverflow.Name = "lbxLocAdditionalOverflow";
            lbxLocAdditionalOverflow.ScrollAlwaysVisible = true;
            lbxLocAdditionalOverflow.Size = new Size(334, 183);
            lbxLocAdditionalOverflow.TabIndex = 15;
            // 
            // lbxLocClassOverflow
            // 
            lbxLocClassOverflow.Dock = DockStyle.Left;
            lbxLocClassOverflow.Enabled = false;
            lbxLocClassOverflow.FormattingEnabled = true;
            lbxLocClassOverflow.ItemHeight = 13;
            lbxLocClassOverflow.Location = new Point(0, 0);
            lbxLocClassOverflow.Name = "lbxLocClassOverflow";
            lbxLocClassOverflow.ScrollAlwaysVisible = true;
            lbxLocClassOverflow.Size = new Size(258, 183);
            lbxLocClassOverflow.TabIndex = 14;
            // 
            // btnClearAdditionalLocFilter
            // 
            btnClearAdditionalLocFilter.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnClearAdditionalLocFilter.Location = new Point(383, 380);
            btnClearAdditionalLocFilter.Name = "btnClearAdditionalLocFilter";
            btnClearAdditionalLocFilter.Size = new Size(205, 29);
            btnClearAdditionalLocFilter.TabIndex = 12;
            btnClearAdditionalLocFilter.Text = "Remove Overflow Locations";
            btnClearAdditionalLocFilter.UseVisualStyleBackColor = true;
            // 
            // btnClearLocClassOverflow
            // 
            btnClearLocClassOverflow.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnClearLocClassOverflow.Location = new Point(76, 382);
            btnClearLocClassOverflow.Name = "btnClearLocClassOverflow";
            btnClearLocClassOverflow.Size = new Size(186, 28);
            btnClearLocClassOverflow.TabIndex = 11;
            btnClearLocClassOverflow.Text = "Remove Overflow Classes";
            btnClearLocClassOverflow.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(panel2);
            tabPage3.Controls.Add(label2);
            tabPage3.Controls.Add(txtExcludeAllAboveThisIndex);
            tabPage3.Controls.Add(txtExcludeAllBelowThisIndex);
            tabPage3.Controls.Add(chkEnableIndexFilter);
            tabPage3.Controls.Add(label1);
            tabPage3.Location = new Point(4, 22);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(598, 423);
            tabPage3.TabIndex = 3;
            tabPage3.Text = "Advanced Filters";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.Window;
            panel2.Controls.Add(label7);
            panel2.Controls.Add(chkmtfSessionMsgs);
            panel2.Controls.Add(chkmtfChainMsgs);
            panel2.Controls.Add(chkmtfCommandMsgs);
            panel2.Controls.Add(chkmtfInternalMessages);
            panel2.Controls.Add(chkmtfResourceMessages);
            panel2.Controls.Add(chkmtfExceptionContent);
            panel2.Location = new Point(7, 217);
            panel2.Name = "panel2";
            panel2.Size = new Size(483, 193);
            panel2.TabIndex = 109;
            // 
            // label7
            // 
            label7.Location = new Point(16, 12);
            label7.Name = "label7";
            label7.Size = new Size(464, 46);
            label7.TabIndex = 144;
            label7.Text = "N.B. Ensure you have ticked the option \"Allow Internal Messages To Be Displayed\" in Mex Options before using these filters.";
            // 
            // chkmtfSessionMsgs
            // 
            chkmtfSessionMsgs.Appearance = Appearance.Button;
            chkmtfSessionMsgs.FlatAppearance.CheckedBackColor = Color.FromArgb(192, 255, 192);
            chkmtfSessionMsgs.FlatStyle = FlatStyle.Flat;
            chkmtfSessionMsgs.ImageAlign = ContentAlignment.MiddleRight;
            chkmtfSessionMsgs.ImageIndex = 5;
            chkmtfSessionMsgs.Location = new Point(220, 110);
            chkmtfSessionMsgs.Name = "chkmtfSessionMsgs";
            chkmtfSessionMsgs.Size = new Size(193, 29);
            chkmtfSessionMsgs.TabIndex = 143;
            chkmtfSessionMsgs.Text = "Section Messages";
            chkmtfSessionMsgs.UseVisualStyleBackColor = true;
            // 
            // chkmtfChainMsgs
            // 
            chkmtfChainMsgs.Appearance = Appearance.Button;
            chkmtfChainMsgs.FlatAppearance.CheckedBackColor = Color.FromArgb(192, 255, 192);
            chkmtfChainMsgs.FlatStyle = FlatStyle.Flat;
            chkmtfChainMsgs.ImageAlign = ContentAlignment.MiddleRight;
            chkmtfChainMsgs.ImageIndex = 8;
            chkmtfChainMsgs.Location = new Point(220, 73);
            chkmtfChainMsgs.Name = "chkmtfChainMsgs";
            chkmtfChainMsgs.Size = new Size(193, 29);
            chkmtfChainMsgs.TabIndex = 142;
            chkmtfChainMsgs.Text = "Chain Messages";
            chkmtfChainMsgs.UseVisualStyleBackColor = true;
            // 
            // chkmtfCommandMsgs
            // 
            chkmtfCommandMsgs.Appearance = Appearance.Button;
            chkmtfCommandMsgs.FlatAppearance.CheckedBackColor = Color.FromArgb(192, 255, 192);
            chkmtfCommandMsgs.FlatStyle = FlatStyle.Flat;
            chkmtfCommandMsgs.ImageAlign = ContentAlignment.MiddleRight;
            chkmtfCommandMsgs.ImageIndex = 0;
            chkmtfCommandMsgs.Location = new Point(220, 146);
            chkmtfCommandMsgs.Name = "chkmtfCommandMsgs";
            chkmtfCommandMsgs.Size = new Size(193, 30);
            chkmtfCommandMsgs.TabIndex = 141;
            chkmtfCommandMsgs.Text = "Command Messages";
            chkmtfCommandMsgs.UseVisualStyleBackColor = true;
            // 
            // chkmtfInternalMessages
            // 
            chkmtfInternalMessages.Appearance = Appearance.Button;
            chkmtfInternalMessages.FlatAppearance.CheckedBackColor = Color.FromArgb(192, 255, 192);
            chkmtfInternalMessages.FlatStyle = FlatStyle.Flat;
            chkmtfInternalMessages.ImageAlign = ContentAlignment.MiddleRight;
            chkmtfInternalMessages.ImageIndex = 2;
            chkmtfInternalMessages.Location = new Point(19, 146);
            chkmtfInternalMessages.Name = "chkmtfInternalMessages";
            chkmtfInternalMessages.Size = new Size(193, 30);
            chkmtfInternalMessages.TabIndex = 140;
            chkmtfInternalMessages.Text = "Internal Messages";
            chkmtfInternalMessages.UseVisualStyleBackColor = true;
            // 
            // chkmtfResourceMessages
            // 
            chkmtfResourceMessages.Appearance = Appearance.Button;
            chkmtfResourceMessages.FlatAppearance.CheckedBackColor = Color.FromArgb(192, 255, 192);
            chkmtfResourceMessages.FlatStyle = FlatStyle.Flat;
            chkmtfResourceMessages.ImageAlign = ContentAlignment.MiddleRight;
            chkmtfResourceMessages.ImageIndex = 29;
            chkmtfResourceMessages.Location = new Point(19, 73);
            chkmtfResourceMessages.Name = "chkmtfResourceMessages";
            chkmtfResourceMessages.Size = new Size(193, 29);
            chkmtfResourceMessages.TabIndex = 139;
            chkmtfResourceMessages.Text = "Resource Messages";
            chkmtfResourceMessages.UseVisualStyleBackColor = true;
            // 
            // chkmtfExceptionContent
            // 
            chkmtfExceptionContent.Appearance = Appearance.Button;
            chkmtfExceptionContent.FlatAppearance.CheckedBackColor = Color.FromArgb(192, 255, 192);
            chkmtfExceptionContent.FlatStyle = FlatStyle.Flat;
            chkmtfExceptionContent.ImageAlign = ContentAlignment.MiddleRight;
            chkmtfExceptionContent.ImageIndex = 22;
            chkmtfExceptionContent.Location = new Point(19, 110);
            chkmtfExceptionContent.Name = "chkmtfExceptionContent";
            chkmtfExceptionContent.Size = new Size(193, 29);
            chkmtfExceptionContent.TabIndex = 138;
            chkmtfExceptionContent.Text = "Exception Content";
            chkmtfExceptionContent.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(49, 107);
            label2.Name = "label2";
            label2.Size = new Size(173, 13);
            label2.TabIndex = 107;
            label2.Text = "Exclude higher indexes than:";
            // 
            // txtExcludeAllAboveThisIndex
            // 
            txtExcludeAllAboveThisIndex.Enabled = false;
            txtExcludeAllAboveThisIndex.Location = new Point(258, 103);
            txtExcludeAllAboveThisIndex.Name = "txtExcludeAllAboveThisIndex";
            txtExcludeAllAboveThisIndex.Size = new Size(107, 21);
            txtExcludeAllAboveThisIndex.TabIndex = 106;
            txtExcludeAllAboveThisIndex.TextChanged += ExcludeAllAboveThisIndex_TextChanged;
            // 
            // txtExcludeAllBelowThisIndex
            // 
            txtExcludeAllBelowThisIndex.Enabled = false;
            txtExcludeAllBelowThisIndex.Location = new Point(258, 64);
            txtExcludeAllBelowThisIndex.Name = "txtExcludeAllBelowThisIndex";
            txtExcludeAllBelowThisIndex.Size = new Size(107, 21);
            txtExcludeAllBelowThisIndex.TabIndex = 104;
            txtExcludeAllBelowThisIndex.TextChanged += ExcludeAllBelowThisIndex_TextChanged;
            // 
            // chkEnableIndexFilter
            // 
            chkEnableIndexFilter.AutoSize = true;
            chkEnableIndexFilter.Location = new Point(8, 38);
            chkEnableIndexFilter.Name = "chkEnableIndexFilter";
            chkEnableIndexFilter.Size = new Size(116, 17);
            chkEnableIndexFilter.TabIndex = 108;
            chkEnableIndexFilter.Text = "Use Index Filter";
            chkEnableIndexFilter.UseVisualStyleBackColor = true;
            chkEnableIndexFilter.CheckedChanged += EnableIndexFilter_CheckedChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(49, 68);
            label1.Name = "label1";
            label1.Size = new Size(168, 13);
            label1.TabIndex = 105;
            label1.Text = "Exclude lower indexes than:";
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(txtFilterExtension);
            tabPage4.Controls.Add(chkUseFilterOnLoad);
            tabPage4.Controls.Add(lblSaveData);
            tabPage4.Controls.Add(btnRefreshFilterDir);
            tabPage4.Controls.Add(txtFilterFilename);
            tabPage4.Controls.Add(txtFilterDirectory);
            tabPage4.Controls.Add(groupBox1);
            tabPage4.Controls.Add(lbxAllMatchedFilters);
            tabPage4.Controls.Add(chkAllowOverwrite);
            tabPage4.Controls.Add(btnLoadFilterConfiguration);
            tabPage4.Controls.Add(btnSaveFilterConfiguration);
            tabPage4.Location = new Point(4, 22);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(598, 423);
            tabPage4.TabIndex = 4;
            tabPage4.Text = "Load / Save";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // txtFilterExtension
            // 
            txtFilterExtension.Location = new Point(493, 80);
            txtFilterExtension.Name = "txtFilterExtension";
            txtFilterExtension.ReadOnly = true;
            txtFilterExtension.Size = new Size(79, 21);
            txtFilterExtension.TabIndex = 118;
            // 
            // chkUseFilterOnLoad
            // 
            chkUseFilterOnLoad.AutoSize = true;
            chkUseFilterOnLoad.Location = new Point(8, 261);
            chkUseFilterOnLoad.Name = "chkUseFilterOnLoad";
            chkUseFilterOnLoad.Size = new Size(238, 17);
            chkUseFilterOnLoad.TabIndex = 117;
            chkUseFilterOnLoad.Text = "Attempt to load this filter on start up.";
            chkUseFilterOnLoad.UseVisualStyleBackColor = true;
            chkUseFilterOnLoad.CheckedChanged += UseFilterOnLoad_CheckedChanged;
            // 
            // lblSaveData
            // 
            lblSaveData.AutoSize = true;
            lblSaveData.Location = new Point(7, 55);
            lblSaveData.Name = "lblSaveData";
            lblSaveData.Size = new Size(41, 13);
            lblSaveData.TabIndex = 116;
            lblSaveData.Text = "label6";
            // 
            // btnRefreshFilterDir
            // 
            btnRefreshFilterDir.Location = new Point(368, 118);
            btnRefreshFilterDir.Name = "btnRefreshFilterDir";
            btnRefreshFilterDir.Size = new Size(204, 28);
            btnRefreshFilterDir.TabIndex = 115;
            btnRefreshFilterDir.Text = "Refresh";
            btnRefreshFilterDir.UseVisualStyleBackColor = true;
            btnRefreshFilterDir.Click += Refresh_Click;
            // 
            // txtFilterFilename
            // 
            txtFilterFilename.Location = new Point(368, 80);
            txtFilterFilename.Name = "txtFilterFilename";
            txtFilterFilename.Size = new Size(118, 21);
            txtFilterFilename.TabIndex = 114;
            txtFilterFilename.TextChanged += FilterFilename_TextChanged;
            // 
            // txtFilterDirectory
            // 
            txtFilterDirectory.Location = new Point(7, 26);
            txtFilterDirectory.Name = "txtFilterDirectory";
            txtFilterDirectory.ReadOnly = true;
            txtFilterDirectory.Size = new Size(565, 21);
            txtFilterDirectory.TabIndex = 113;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(chkSaveModules);
            groupBox1.Controls.Add(chkSaveClassLocationInfo);
            groupBox1.Controls.Add(chkSaveLocationInformation);
            groupBox1.Controls.Add(chkSaveIncludeThreads);
            groupBox1.Location = new Point(8, 303);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(564, 91);
            groupBox1.TabIndex = 111;
            groupBox1.TabStop = false;
            groupBox1.Text = "Include this filter information:";
            // 
            // chkSaveModules
            // 
            chkSaveModules.AutoSize = true;
            chkSaveModules.Location = new Point(7, 53);
            chkSaveModules.Name = "chkSaveModules";
            chkSaveModules.Size = new Size(170, 17);
            chkSaveModules.TabIndex = 3;
            chkSaveModules.Text = "Save Module Information";
            chkSaveModules.UseVisualStyleBackColor = true;
            // 
            // chkSaveClassLocationInfo
            // 
            chkSaveClassLocationInfo.AutoSize = true;
            chkSaveClassLocationInfo.Location = new Point(271, 53);
            chkSaveClassLocationInfo.Name = "chkSaveClassLocationInfo";
            chkSaveClassLocationInfo.Size = new Size(212, 17);
            chkSaveClassLocationInfo.TabIndex = 2;
            chkSaveClassLocationInfo.Text = "Save Class Location Information";
            chkSaveClassLocationInfo.UseVisualStyleBackColor = true;
            // 
            // chkSaveLocationInformation
            // 
            chkSaveLocationInformation.AutoSize = true;
            chkSaveLocationInformation.Location = new Point(271, 25);
            chkSaveLocationInformation.Name = "chkSaveLocationInformation";
            chkSaveLocationInformation.Size = new Size(195, 17);
            chkSaveLocationInformation.TabIndex = 1;
            chkSaveLocationInformation.Text = "Save All Location Information";
            chkSaveLocationInformation.UseVisualStyleBackColor = true;
            // 
            // chkSaveIncludeThreads
            // 
            chkSaveIncludeThreads.AutoSize = true;
            chkSaveIncludeThreads.Location = new Point(7, 25);
            chkSaveIncludeThreads.Name = "chkSaveIncludeThreads";
            chkSaveIncludeThreads.Size = new Size(170, 17);
            chkSaveIncludeThreads.TabIndex = 0;
            chkSaveIncludeThreads.Text = "Save Thread Information";
            chkSaveIncludeThreads.UseVisualStyleBackColor = true;
            // 
            // lbxAllMatchedFilters
            // 
            lbxAllMatchedFilters.FormattingEnabled = true;
            lbxAllMatchedFilters.ItemHeight = 13;
            lbxAllMatchedFilters.Location = new Point(7, 79);
            lbxAllMatchedFilters.Name = "lbxAllMatchedFilters";
            lbxAllMatchedFilters.Size = new Size(354, 160);
            lbxAllMatchedFilters.TabIndex = 110;
            lbxAllMatchedFilters.SelectedIndexChanged += AllMatchedFilters_SelectedIndexChanged;
            // 
            // chkAllowOverwrite
            // 
            chkAllowOverwrite.AutoSize = true;
            chkAllowOverwrite.Location = new Point(368, 225);
            chkAllowOverwrite.Name = "chkAllowOverwrite";
            chkAllowOverwrite.Size = new Size(116, 17);
            chkAllowOverwrite.TabIndex = 109;
            chkAllowOverwrite.Text = "Allow Overwrite";
            chkAllowOverwrite.UseVisualStyleBackColor = true;
            // 
            // btnLoadFilterConfiguration
            // 
            btnLoadFilterConfiguration.Location = new Point(368, 190);
            btnLoadFilterConfiguration.Name = "btnLoadFilterConfiguration";
            btnLoadFilterConfiguration.Size = new Size(204, 28);
            btnLoadFilterConfiguration.TabIndex = 108;
            btnLoadFilterConfiguration.Text = "Load";
            btnLoadFilterConfiguration.UseVisualStyleBackColor = true;
            btnLoadFilterConfiguration.Click += LoadFilterConfiguration_Click_1;
            // 
            // btnSaveFilterConfiguration
            // 
            btnSaveFilterConfiguration.Location = new Point(368, 154);
            btnSaveFilterConfiguration.Name = "btnSaveFilterConfiguration";
            btnSaveFilterConfiguration.Size = new Size(204, 28);
            btnSaveFilterConfiguration.TabIndex = 107;
            btnSaveFilterConfiguration.Text = "Save";
            btnSaveFilterConfiguration.UseVisualStyleBackColor = true;
            btnSaveFilterConfiguration.Click += SaveFilterConfiguration_Click;
            // 
            // btnSetDefaultFilter
            // 
            btnSetDefaultFilter.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSetDefaultFilter.Location = new Point(211, 472);
            btnSetDefaultFilter.Name = "btnSetDefaultFilter";
            btnSetDefaultFilter.Size = new Size(87, 28);
            btnSetDefaultFilter.TabIndex = 107;
            btnSetDefaultFilter.Text = "Default";
            btnSetDefaultFilter.UseVisualStyleBackColor = true;
            btnSetDefaultFilter.Click += SetDefaultFilter_Click;
            // 
            // cboQuickLoad
            // 
            cboQuickLoad.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            cboQuickLoad.FormattingEnabled = true;
            cboQuickLoad.Location = new Point(7, 472);
            cboQuickLoad.Name = "cboQuickLoad";
            cboQuickLoad.Size = new Size(187, 23);
            cboQuickLoad.TabIndex = 108;
            cboQuickLoad.SelectedIndexChanged += QuickLoad_SelectedIndexChanged;
            // 
            // frmProcViewFilter
            // 
            AcceptButton = btnOK;
            AutoScaleBaseSize = new Size(6, 16);
            CancelButton = btnCancel;
            ClientSize = new Size(608, 517);
            Controls.Add(cboQuickLoad);
            Controls.Add(btnSetDefaultFilter);
            Controls.Add(tbcFilterDetails);
            Controls.Add(btnLoadMexDefaultFilter);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new Size(1042, 556);
            MinimizeBox = false;
            MinimumSize = new Size(624, 556);
            Name = "frmProcViewFilter";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Filter Options.";
            tbcFilterDetails.ResumeLayout(false);
            tabOptionsEvents.ResumeLayout(false);
            tabOptionsEvents.PerformLayout();
            pnlFilterButtons.ResumeLayout(false);
            pnlFilterButtons.PerformLayout();
            tabPage2.ResumeLayout(false);
            pnlDockSplitContainer.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tabLocationFiltering.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            panel2.ResumeLayout(false);
            tabPage4.ResumeLayout(false);
            tabPage4.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion Windows Form Designer generated code

        // End PopulateAFilterDialogFromStrings

        private void LoadFilterConfiguration_Click_1(object sender, EventArgs e) {
            string filename = Path.Combine(MexCore.TheCore.Options.FilterAndHighlightStoreDirectory, lbxAllMatchedFilters.SelectedItem.ToString() + MexCore.TheCore.Options.FilterExtension);

            //Bilge.Assert(File.Exists(filename), "The filename selected from the list of filteres did not exist.  This should not be possible");

            InitialiseFromExistingFilter(ViewFilter.LoadFilterFromFile(filename));
            tbcFilterDetails.SelectedTab = tabOptionsEvents;
        }

        private void LoadMexDefaultFilter_Click(object sender, EventArgs e) {
            var vf = new ViewFilter();
            InitialiseFromExistingFilter(vf);
            DialogResult = DialogResult.OK;
            FullRefreshRequired = true;
            Close();
        }

        private void OKWithFullRefresh_Click(object sender, System.EventArgs e) {
            FullRefreshRequired = true;
        }

        private void Refresh_Click(object sender, EventArgs e) {
            InitialiseLoadSaveScreen();
        }

        private void SaveFilterConfiguration_Click(object sender, EventArgs e) {
            string fileName = Path.Combine(MexCore.TheCore.Options.FilterAndHighlightStoreDirectory, txtFilterFilename.Text + MexCore.TheCore.Options.FilterExtension);
            var af = GetFilterFromDialog();
            ViewFilter.SaveFilterToFile(fileName, af, chkSaveIncludeThreads.Checked, chkSaveModules.Checked, chkSaveLocationInformation.Checked, chkSaveClassLocationInfo.Checked);
            btnSaveFilterConfiguration.Enabled = false;
            RefreshFiltersList();
        }

        private void SetDefaultFilter_Click(object sender, EventArgs e) {
            var vf = new ViewFilter();
            InitialiseFromExistingFilter(vf);
            tbcFilterDetails.SelectedTab = tabOptionsEvents;
        }

        private void QuickLoad_SelectedIndexChanged(object sender, EventArgs e) {
            string filterName = cboQuickLoad.Text;

            filterName = Path.Combine(MexCore.TheCore.Options.FilterAndHighlightStoreDirectory, filterName + MexCore.TheCore.Options.FilterExtension);

            if (File.Exists(filterName)) {
                var vf = ViewFilter.LoadFilterFromFile(filterName);
                InitialiseFromExistingFilter(vf);
                DialogResult = DialogResult.OK;
                Close();
            } else {
                MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.FilterFileNotFound, UserMessageType.WarningMessage, filterName);
            }
        }

        private void EnableIndexFilter_CheckedChanged(object sender, EventArgs e) {
            txtExcludeAllAboveThisIndex.Enabled = txtExcludeAllBelowThisIndex.Enabled = chkEnableIndexFilter.Checked;
        }

        private void UseFilterOnLoad_CheckedChanged(object sender, EventArgs e) {
            if (!autochanging) {
                // when this is changed if a filter is selected then it should be loaded when Mex starts.
                MexCore.TheCore.Options.FilterFilenameToLoadOnStartup = chkUseFilterOnLoad.Checked
                    ? Path.Combine(MexCore.TheCore.Options.FilterAndHighlightStoreDirectory, txtFilterFilename.Text + MexCore.TheCore.Options.FilterExtension)
                    : "";
            }
        }

        // Defualt false
        private void InitialiseLoadSaveScreen() {
            if (Directory.Exists(MexCore.TheCore.Options.FilterAndHighlightStoreDirectory)) {
                txtFilterFilename.Text = "*";
                txtFilterExtension.Text = MexCore.TheCore.Options.FilterExtension;
                txtFilterDirectory.Text = MexCore.TheCore.Options.FilterAndHighlightStoreDirectory;

                RefreshFiltersList();

                btnSaveFilterConfiguration.Enabled = false;
                btnLoadFilterConfiguration.Enabled = false;
                btnRefreshFilterDir.Enabled = true;
            } else {
                btnSaveFilterConfiguration.Enabled = false;
                btnLoadFilterConfiguration.Enabled = false;
                btnRefreshFilterDir.Enabled = false;
                lblSaveData.Text = "The directory for filter data could not be found.";
            }
        }

        private void AllMatchedFilters_SelectedIndexChanged(object sender, EventArgs e) {
            autochanging = true;
            if (lbxAllMatchedFilters.SelectedItem != null) {
                txtFilterFilename.Text = lbxAllMatchedFilters.SelectedItem.ToString();
                chkUseFilterOnLoad.Enabled = true;
                chkUseFilterOnLoad.Checked = MexCore.TheCore.Options.FilterFilenameToLoadOnStartup == Path.Combine(MexCore.TheCore.Options.FilterAndHighlightStoreDirectory, txtFilterFilename.Text + MexCore.TheCore.Options.FilterExtension);
            } else {
                chkUseFilterOnLoad.Enabled = false;
            }
            autochanging = false;
        }

        private void RefreshFiltersList() {
            string[] matchedFilters = Directory.GetFiles(MexCore.TheCore.Options.FilterAndHighlightStoreDirectory, "*" + MexCore.TheCore.Options.FilterExtension);

            lbxAllMatchedFilters.Items.Clear();
            cboQuickLoad.Items.Clear();

            foreach (string s in matchedFilters) {
                string nextOne = Path.GetFileNameWithoutExtension(s);
                _ = lbxAllMatchedFilters.Items.Add(nextOne);
                _ = cboQuickLoad.Items.Add(nextOne);
            }

            cboQuickLoad.Text = " > Select A Filter To Quick Load <";
            if (matchedFilters.Length == 0) {
                lblSaveData.Text = "No filters found.";
            }
        }

        private void ExcludeAllAboveThisIndex_TextChanged(object sender, EventArgs e) {
            if (!int.TryParse(txtExcludeAllAboveThisIndex.Text, out int idxVal) || (idxVal < 0)) {
                // error.
                txtExcludeAllAboveThisIndex.BackColor = Color.IndianRed;
                higherIndexValue = int.MaxValue;
            } else {
                txtExcludeAllAboveThisIndex.BackColor = Color.FromKnownColor(KnownColor.Window);
                higherIndexValue = idxVal;
            }
        }

        private void ExcludeAllBelowThisIndex_TextChanged(object sender, EventArgs e) {
            if (!int.TryParse(txtExcludeAllBelowThisIndex.Text, out int idxVal) || (idxVal < 0)) {
                // error.
                txtExcludeAllBelowThisIndex.BackColor = Color.IndianRed;
                lowerIndexValue = int.MinValue;
            } else {
                txtExcludeAllBelowThisIndex.BackColor = Color.FromKnownColor(KnownColor.Window);
                lowerIndexValue = idxVal;
            }
        }

        private void FilterFilename_TextChanged(object sender, EventArgs e) {
            string potentialFilenameToSave = txtFilterFilename.Text.Trim();

            if ((potentialFilenameToSave.Length == 0) || (potentialFilenameToSave.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)) {
                btnSaveFilterConfiguration.Enabled = false;
                btnLoadFilterConfiguration.Enabled = false;
                return;
            }

            potentialFilenameToSave = Path.Combine(MexCore.TheCore.Options.FilterAndHighlightStoreDirectory, potentialFilenameToSave + MexCore.TheCore.Options.FilterExtension);

            btnSaveFilterConfiguration.Enabled = true;
            btnLoadFilterConfiguration.Enabled = true;

            if (potentialFilenameToSave.IndexOfAny(Path.GetInvalidPathChars()) > 0) {
                btnSaveFilterConfiguration.Enabled = false;
                lblSaveData.Text = "Unable to save, filename contains invalid characters";
                return;
            }

            if (File.Exists(potentialFilenameToSave)) {
                btnSaveFilterConfiguration.Enabled = chkAllowOverwrite.Checked;
                if (!chkAllowOverwrite.Checked) {
                    lblSaveData.Text = "Unable to save, file exists and allow overwrite not checked";
                }
            } else {
                lblSaveData.Text = "Unable to load, file does not exist";
                btnLoadFilterConfiguration.Enabled = false;
            }
        }

        // End PopulateAFilterDialogFromStrings
        private void UpdateCheckedTypesFromViewFilter(ViewFilter setToThis) {
            chkmtfAssertionMsgs.Checked = setToThis.TraceMessageTypeIncludedByFilter(TraceCommandTypes.AssertionFailed);

            chkmtfExceptionHeaders.Checked = setToThis.TraceMessageTypeIncludedByFilter(TraceCommandTypes.ExceptionBlock);

            chkmtfExceptionContent.Checked = setToThis.TraceMessageTypeIncludedByFilter(TraceCommandTypes.ExceptionData)
                                        & setToThis.TraceMessageTypeIncludedByFilter(TraceCommandTypes.ExcStart)
                                        & setToThis.TraceMessageTypeIncludedByFilter(TraceCommandTypes.ExcEnd);

            chkmtfVerboseLogMessages.Checked = setToThis.TraceMessageTypeIncludedByFilter(TraceCommandTypes.LogMessageVerb);
            chkmtfIncludeLogs.Checked = setToThis.TraceMessageTypeIncludedByFilter(TraceCommandTypes.LogMessage);

            chkmtfResourceMessages.Checked = setToThis.TraceMessageTypeIncludedByFilter(TraceCommandTypes.ResourceCount)
                                    & setToThis.TraceMessageTypeIncludedByFilter(TraceCommandTypes.ResourceEat)
                                    & setToThis.TraceMessageTypeIncludedByFilter(TraceCommandTypes.ResourcePuke);

            chkmtfSessionMsgs.Checked = setToThis.TraceMessageTypeIncludedByFilter(TraceCommandTypes.SectionEnd)
                                    & setToThis.TraceMessageTypeIncludedByFilter(TraceCommandTypes.SectionStart);

            chkmtfTraceEnterMsgs.Checked = setToThis.TraceMessageTypeIncludedByFilter(TraceCommandTypes.TraceMessageIn);
            chkmtfTraceExitMsgs.Checked = setToThis.TraceMessageTypeIncludedByFilter(TraceCommandTypes.TraceMessageOut);

            chkmtfWarningMsgs.Checked = setToThis.TraceMessageTypeIncludedByFilter(TraceCommandTypes.WarningMsg);
            chkmtfErrorMsgs.Checked = setToThis.TraceMessageTypeIncludedByFilter(TraceCommandTypes.ErrorMsg);

            chkmtfChainMsgs.Checked = setToThis.TraceMessageTypeIncludedByFilter(TraceCommandTypes.MoreInfo);

            chkmtfCommandMsgs.Checked = setToThis.TraceMessageTypeIncludedByFilter(TraceCommandTypes.CommandOnly);
            chkmtfInternalMessages.Checked = setToThis.TraceMessageTypeIncludedByFilter(TraceCommandTypes.InternalMsg);
        }

        private void UpdateViewFilterFromCheckedTypes(ViewFilter result) {
            result.SetMessageTypeIncludeByType(TraceCommandTypes.AssertionFailed, chkmtfAssertionMsgs.Checked);

            result.SetMessageTypeIncludeByType(TraceCommandTypes.ExceptionBlock, chkmtfExceptionHeaders.Checked);
            result.SetMessageTypeIncludeByType(TraceCommandTypes.ExceptionData, chkmtfExceptionContent.Checked);
            result.SetMessageTypeIncludeByType(TraceCommandTypes.ExcStart, chkmtfExceptionContent.Checked);
            result.SetMessageTypeIncludeByType(TraceCommandTypes.ExcEnd, chkmtfExceptionContent.Checked);

            result.SetMessageTypeIncludeByType(TraceCommandTypes.LogMessageVerb, chkmtfVerboseLogMessages.Checked);
            result.SetMessageTypeIncludeByType(TraceCommandTypes.LogMessage, chkmtfIncludeLogs.Checked);

            result.SetMessageTypeIncludeByType(TraceCommandTypes.ResourceCount, chkmtfResourceMessages.Checked);
            result.SetMessageTypeIncludeByType(TraceCommandTypes.ResourceEat, chkmtfResourceMessages.Checked);
            result.SetMessageTypeIncludeByType(TraceCommandTypes.ResourcePuke, chkmtfResourceMessages.Checked);

            result.SetMessageTypeIncludeByType(TraceCommandTypes.SectionEnd, chkmtfSessionMsgs.Checked);
            result.SetMessageTypeIncludeByType(TraceCommandTypes.SectionStart, chkmtfSessionMsgs.Checked);

            result.SetMessageTypeIncludeByType(TraceCommandTypes.TraceMessageIn, chkmtfTraceEnterMsgs.Checked);
            result.SetMessageTypeIncludeByType(TraceCommandTypes.TraceMessageOut, chkmtfTraceExitMsgs.Checked);

            result.SetMessageTypeIncludeByType(TraceCommandTypes.WarningMsg, chkmtfWarningMsgs.Checked);
            result.SetMessageTypeIncludeByType(TraceCommandTypes.ErrorMsg, chkmtfErrorMsgs.Checked);

            result.SetMessageTypeIncludeByType(TraceCommandTypes.MoreInfo, chkmtfChainMsgs.Checked);

            result.SetMessageTypeIncludeByType(TraceCommandTypes.CommandOnly, chkmtfCommandMsgs.Checked);
            result.SetMessageTypeIncludeByType(TraceCommandTypes.InternalMsg, chkmtfInternalMessages.Checked);
        }
    }
}