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
        internal bool FullRefreshRequired;

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
        private System.ComponentModel.IContainer components;
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
                        threadExcludes = new List<KeyDisplayRepresentation>();
                        foreach (KeyDisplayRepresentation s in clbThreadsFilter.Items) {
                            if (!clbThreadsFilter.CheckedItems.Contains(s)) {
                                threadExcludes.Add(s);
                            }
                        }
                    } // End if we are filtering by thread ID.

                    if (clbModulesFilter.CheckedItems.Count != clbModulesFilter.Items.Count) {
                        //Bilge.VerboseLog("Looking at the modules filter that was ticked, clbModulesFilter.Checked.Count > 0");
                        moduleExcludes = new List<string>();

                        foreach (string s in clbModulesFilter.Items) {
                            if (!clbModulesFilter.CheckedItems.Contains(s)) {
                                moduleExcludes.Add(s);
                            }
                        }
                    }

                    if (clbAdditionalLocationsFilter.CheckedItems.Count != clbAdditionalLocationsFilter.Items.Count) {
                        //Bilge.VerboseLog("Looking at the additional locations that were ticked, AdditionalLocationsFilter.Checked.Count >0");

                        additionalLocationExcludes = new List<string>();

                        foreach (string s in clbAdditionalLocationsFilter.Items) {
                            if (!clbAdditionalLocationsFilter.CheckedItems.Contains(s)) {
                                additionalLocationExcludes.Add(s);
                            }
                        }
                    }

                    if (clbLocationClassFilter.CheckedItems.Count != clbLocationClassFilter.Items.Count) {
                        //Bilge.VerboseLog("Looking at the class location filter, clbLocationClassFilter.Checked.Count > 0");
                        additionalLocationClassExcludes = new List<string>();
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
            List<KeyDisplayRepresentation> threadDisplayNames = MexCore.TheCore.ViewManager.GetThreadNameListForSelectedApp();
            List<string> moduleDisplayNames = MexCore.TheCore.ViewManager.GetAllModules();
            List<string> additionalLocationNames;
            List<string> additionalLocationClasses;

            MexCore.TheCore.ViewManager.GetAdditionalLocationsAndClasses(out additionalLocationNames, out additionalLocationClasses);

            clbThreadsFilter.Items.Clear();
            clbModulesFilter.Items.Clear();
            clbThreadsFilter.Items.AddRange(threadDisplayNames.ToArray());
            clbModulesFilter.Items.AddRange(moduleDisplayNames.ToArray());

            clbAdditionalLocationsFilter.Items.AddRange(threadDisplayNames.ToArray());
            clbLocationClassFilter.Items.AddRange(additionalLocationClasses.ToArray());
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
                    List<string> additionalLocations = MexCore.TheCore.ViewManager.GetAllAdditionalLocations();
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

                string belowidx;
                string aboveidx;

                if (currentFilter.GetIndexFilters(out belowidx, out aboveidx)) {
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        internal void PolpulateCheckedListBoxFromKeyDisplay(CheckedListBox target, ListBox overflow, List<KeyDisplayRepresentation> completeListOfAllItems, List<KeyDisplayRepresentation> thoseItemsToBeTicked) {
            target.Items.Clear();
            overflow.Items.Clear();

            if (overflow != null) {
                foreach (KeyDisplayRepresentation s in thoseItemsToBeTicked) {
                    if (!completeListOfAllItems.Contains(s)) {
                        overflow.Items.Add(s);
                    }
                }
            }

            foreach (KeyDisplayRepresentation s in completeListOfAllItems) {
                if (thoseItemsToBeTicked.Contains(s)) {
                    target.Items.Add(s, false);
                } else {
                    target.Items.Add(s, true);
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        internal void PolpulateCheckedListBoxFromStringLists(CheckedListBox target, ListBox overflow, List<string> completeListOfAllItems, List<string> thoseItemsToBeTicked) {
            target.Items.Clear();
            overflow.Items.Clear();

            if (overflow != null) {
                foreach (string s in thoseItemsToBeTicked) {
                    if (!completeListOfAllItems.Contains(s)) {
                        overflow.Items.Add(s);
                    }
                }
            }

            foreach (string s in completeListOfAllItems) {
                if (thoseItemsToBeTicked.Contains(s)) {
                    target.Items.Add(s, false);
                } else {
                    target.Items.Add(s, true);
                }
            }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProcViewFilter));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnLoadMexDefaultFilter = new System.Windows.Forms.Button();
            this.tbcFilterDetails = new System.Windows.Forms.TabControl();
            this.tabOptionsEvents = new System.Windows.Forms.TabPage();
            this.pnlFilterButtons = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.chkmtfWarningMsgs = new System.Windows.Forms.CheckBox();
            this.chkmtfExceptionHeaders = new System.Windows.Forms.CheckBox();
            this.chkmtfTraceExitMsgs = new System.Windows.Forms.CheckBox();
            this.chkmtfTraceEnterMsgs = new System.Windows.Forms.CheckBox();
            this.chkmtfErrorMsgs = new System.Windows.Forms.CheckBox();
            this.chkmtfAssertionMsgs = new System.Windows.Forms.CheckBox();
            this.chkmtfVerboseLogMessages = new System.Windows.Forms.CheckBox();
            this.chkmtfIncludeLogs = new System.Windows.Forms.CheckBox();
            this.chkCaseSensitive = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFilterExcludeStrings = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtFilterIncludeStrings = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.pnlDockSplitContainer = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.clbModulesFilter = new System.Windows.Forms.CheckedListBox();
            this.clbThreadsFilter = new System.Windows.Forms.CheckedListBox();
            this.lbxUnmatchedModules = new System.Windows.Forms.ListBox();
            this.lbxUnmatchedThreads = new System.Windows.Forms.ListBox();
            this.btnClearUnmatchedModules = new System.Windows.Forms.Button();
            this.btnClearUnmatchedThreads = new System.Windows.Forms.Button();
            this.tabLocationFiltering = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.clbAdditionalLocationsFilter = new System.Windows.Forms.CheckedListBox();
            this.clbLocationClassFilter = new System.Windows.Forms.CheckedListBox();
            this.lbxLocAdditionalOverflow = new System.Windows.Forms.ListBox();
            this.lbxLocClassOverflow = new System.Windows.Forms.ListBox();
            this.btnClearAdditionalLocFilter = new System.Windows.Forms.Button();
            this.btnClearLocClassOverflow = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.chkmtfSessionMsgs = new System.Windows.Forms.CheckBox();
            this.chkmtfChainMsgs = new System.Windows.Forms.CheckBox();
            this.chkmtfCommandMsgs = new System.Windows.Forms.CheckBox();
            this.chkmtfInternalMessages = new System.Windows.Forms.CheckBox();
            this.chkmtfResourceMessages = new System.Windows.Forms.CheckBox();
            this.chkmtfExceptionContent = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtExcludeAllAboveThisIndex = new System.Windows.Forms.TextBox();
            this.txtExcludeAllBelowThisIndex = new System.Windows.Forms.TextBox();
            this.chkEnableIndexFilter = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.txtFilterExtension = new System.Windows.Forms.TextBox();
            this.chkUseFilterOnLoad = new System.Windows.Forms.CheckBox();
            this.lblSaveData = new System.Windows.Forms.Label();
            this.btnRefreshFilterDir = new System.Windows.Forms.Button();
            this.txtFilterFilename = new System.Windows.Forms.TextBox();
            this.txtFilterDirectory = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkSaveModules = new System.Windows.Forms.CheckBox();
            this.chkSaveClassLocationInfo = new System.Windows.Forms.CheckBox();
            this.chkSaveLocationInformation = new System.Windows.Forms.CheckBox();
            this.chkSaveIncludeThreads = new System.Windows.Forms.CheckBox();
            this.lbxAllMatchedFilters = new System.Windows.Forms.ListBox();
            this.chkAllowOverwrite = new System.Windows.Forms.CheckBox();
            this.btnLoadFilterConfiguration = new System.Windows.Forms.Button();
            this.btnSaveFilterConfiguration = new System.Windows.Forms.Button();
            this.btnSetDefaultFilter = new System.Windows.Forms.Button();
            this.cboQuickLoad = new System.Windows.Forms.ComboBox();
            this.tbcFilterDetails.SuspendLayout();
            this.tabOptionsEvents.SuspendLayout();
            this.pnlFilterButtons.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.pnlDockSplitContainer.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabLocationFiltering.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            //
            // btnOK
            //
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(426, 376);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(74, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOKWithFullRefresh_Click);
            //
            // btnCancel
            //
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(345, 376);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            //
            // btnLoadMexDefaultFilter
            //
            this.btnLoadMexDefaultFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadMexDefaultFilter.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnLoadMexDefaultFilter.Location = new System.Drawing.Point(259, 376);
            this.btnLoadMexDefaultFilter.Name = "btnLoadMexDefaultFilter";
            this.btnLoadMexDefaultFilter.Size = new System.Drawing.Size(74, 23);
            this.btnLoadMexDefaultFilter.TabIndex = 99;
            this.btnLoadMexDefaultFilter.Text = "Use Default";
            this.btnLoadMexDefaultFilter.Click += new System.EventHandler(this.btnLoadMexDefaultFilter_Click);
            //
            // tbcFilterDetails
            //
            this.tbcFilterDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbcFilterDetails.Controls.Add(this.tabOptionsEvents);
            this.tbcFilterDetails.Controls.Add(this.tabPage2);
            this.tbcFilterDetails.Controls.Add(this.tabLocationFiltering);
            this.tbcFilterDetails.Controls.Add(this.tabPage3);
            this.tbcFilterDetails.Controls.Add(this.tabPage4);
            this.tbcFilterDetails.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbcFilterDetails.Location = new System.Drawing.Point(2, 3);
            this.tbcFilterDetails.Name = "tbcFilterDetails";
            this.tbcFilterDetails.SelectedIndex = 0;
            this.tbcFilterDetails.Size = new System.Drawing.Size(505, 365);
            this.tbcFilterDetails.TabIndex = 106;
            //
            // tabOptionsEvents
            //
            this.tabOptionsEvents.Controls.Add(this.pnlFilterButtons);
            this.tabOptionsEvents.Controls.Add(this.chkCaseSensitive);
            this.tabOptionsEvents.Controls.Add(this.label4);
            this.tabOptionsEvents.Controls.Add(this.txtFilterExcludeStrings);
            this.tabOptionsEvents.Controls.Add(this.label5);
            this.tabOptionsEvents.Controls.Add(this.txtFilterIncludeStrings);
            this.tabOptionsEvents.Location = new System.Drawing.Point(4, 22);
            this.tabOptionsEvents.Name = "tabOptionsEvents";
            this.tabOptionsEvents.Padding = new System.Windows.Forms.Padding(3);
            this.tabOptionsEvents.Size = new System.Drawing.Size(497, 339);
            this.tabOptionsEvents.TabIndex = 0;
            this.tabOptionsEvents.Text = "Text / Type";
            this.tabOptionsEvents.UseVisualStyleBackColor = true;
            //
            // pnlFilterButtons
            //
            this.pnlFilterButtons.BackColor = System.Drawing.SystemColors.Window;
            this.pnlFilterButtons.Controls.Add(this.label8);
            this.pnlFilterButtons.Controls.Add(this.chkmtfWarningMsgs);
            this.pnlFilterButtons.Controls.Add(this.chkmtfExceptionHeaders);
            this.pnlFilterButtons.Controls.Add(this.chkmtfTraceExitMsgs);
            this.pnlFilterButtons.Controls.Add(this.chkmtfTraceEnterMsgs);
            this.pnlFilterButtons.Controls.Add(this.chkmtfErrorMsgs);
            this.pnlFilterButtons.Controls.Add(this.chkmtfAssertionMsgs);
            this.pnlFilterButtons.Controls.Add(this.chkmtfVerboseLogMessages);
            this.pnlFilterButtons.Controls.Add(this.chkmtfIncludeLogs);
            this.pnlFilterButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilterButtons.Location = new System.Drawing.Point(3, 3);
            this.pnlFilterButtons.Name = "pnlFilterButtons";
            this.pnlFilterButtons.Size = new System.Drawing.Size(491, 132);
            this.pnlFilterButtons.TabIndex = 124;
            //
            // label8
            //
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(7, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(354, 13);
            this.label8.TabIndex = 139;
            this.label8.Text = "Include / Exclude messages based on message type:";
            //
            // chkmtfWarningMsgs
            //
            this.chkmtfWarningMsgs.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkmtfWarningMsgs.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.chkmtfWarningMsgs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkmtfWarningMsgs.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkmtfWarningMsgs.ImageIndex = 31;
            this.chkmtfWarningMsgs.Location = new System.Drawing.Point(324, 59);
            this.chkmtfWarningMsgs.Name = "chkmtfWarningMsgs";
            this.chkmtfWarningMsgs.Size = new System.Drawing.Size(151, 24);
            this.chkmtfWarningMsgs.TabIndex = 132;
            this.chkmtfWarningMsgs.Text = "Warning Messages";
            this.chkmtfWarningMsgs.UseVisualStyleBackColor = true;
            //
            // chkmtfExceptionHeaders
            //
            this.chkmtfExceptionHeaders.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkmtfExceptionHeaders.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.chkmtfExceptionHeaders.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkmtfExceptionHeaders.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkmtfExceptionHeaders.ImageIndex = 20;
            this.chkmtfExceptionHeaders.Location = new System.Drawing.Point(325, 90);
            this.chkmtfExceptionHeaders.Name = "chkmtfExceptionHeaders";
            this.chkmtfExceptionHeaders.Size = new System.Drawing.Size(151, 24);
            this.chkmtfExceptionHeaders.TabIndex = 130;
            this.chkmtfExceptionHeaders.Text = "Exception Headers";
            this.chkmtfExceptionHeaders.UseVisualStyleBackColor = true;
            //
            // chkmtfTraceExitMsgs
            //
            this.chkmtfTraceExitMsgs.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkmtfTraceExitMsgs.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.chkmtfTraceExitMsgs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkmtfTraceExitMsgs.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkmtfTraceExitMsgs.ImageIndex = 17;
            this.chkmtfTraceExitMsgs.Location = new System.Drawing.Point(167, 90);
            this.chkmtfTraceExitMsgs.Name = "chkmtfTraceExitMsgs";
            this.chkmtfTraceExitMsgs.Size = new System.Drawing.Size(152, 24);
            this.chkmtfTraceExitMsgs.TabIndex = 129;
            this.chkmtfTraceExitMsgs.Text = "Trace Exit";
            this.chkmtfTraceExitMsgs.UseVisualStyleBackColor = true;
            //
            // chkmtfTraceEnterMsgs
            //
            this.chkmtfTraceEnterMsgs.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkmtfTraceEnterMsgs.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.chkmtfTraceEnterMsgs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkmtfTraceEnterMsgs.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkmtfTraceEnterMsgs.ImageIndex = 15;
            this.chkmtfTraceEnterMsgs.Location = new System.Drawing.Point(167, 59);
            this.chkmtfTraceEnterMsgs.Name = "chkmtfTraceEnterMsgs";
            this.chkmtfTraceEnterMsgs.Size = new System.Drawing.Size(151, 24);
            this.chkmtfTraceEnterMsgs.TabIndex = 128;
            this.chkmtfTraceEnterMsgs.Text = "Trace Enter";
            this.chkmtfTraceEnterMsgs.UseVisualStyleBackColor = true;
            //
            // chkmtfErrorMsgs
            //
            this.chkmtfErrorMsgs.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkmtfErrorMsgs.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.chkmtfErrorMsgs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkmtfErrorMsgs.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkmtfErrorMsgs.ImageIndex = 30;
            this.chkmtfErrorMsgs.Location = new System.Drawing.Point(324, 29);
            this.chkmtfErrorMsgs.Name = "chkmtfErrorMsgs";
            this.chkmtfErrorMsgs.Size = new System.Drawing.Size(151, 24);
            this.chkmtfErrorMsgs.TabIndex = 127;
            this.chkmtfErrorMsgs.Text = "Error Messages";
            this.chkmtfErrorMsgs.UseVisualStyleBackColor = true;
            //
            // chkmtfAssertionMsgs
            //
            this.chkmtfAssertionMsgs.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkmtfAssertionMsgs.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.chkmtfAssertionMsgs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkmtfAssertionMsgs.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkmtfAssertionMsgs.ImageIndex = 11;
            this.chkmtfAssertionMsgs.Location = new System.Drawing.Point(167, 29);
            this.chkmtfAssertionMsgs.Name = "chkmtfAssertionMsgs";
            this.chkmtfAssertionMsgs.Size = new System.Drawing.Size(151, 24);
            this.chkmtfAssertionMsgs.TabIndex = 126;
            this.chkmtfAssertionMsgs.Text = "Assertion Failures";
            this.chkmtfAssertionMsgs.UseVisualStyleBackColor = true;
            //
            // chkmtfVerboseLogMessages
            //
            this.chkmtfVerboseLogMessages.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkmtfVerboseLogMessages.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.chkmtfVerboseLogMessages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkmtfVerboseLogMessages.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkmtfVerboseLogMessages.ImageIndex = 27;
            this.chkmtfVerboseLogMessages.Location = new System.Drawing.Point(10, 59);
            this.chkmtfVerboseLogMessages.Name = "chkmtfVerboseLogMessages";
            this.chkmtfVerboseLogMessages.Size = new System.Drawing.Size(151, 24);
            this.chkmtfVerboseLogMessages.TabIndex = 125;
            this.chkmtfVerboseLogMessages.Text = "Verbose Messages";
            this.chkmtfVerboseLogMessages.UseVisualStyleBackColor = true;
            //
            // chkmtfIncludeLogs
            //
            this.chkmtfIncludeLogs.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkmtfIncludeLogs.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.chkmtfIncludeLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkmtfIncludeLogs.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkmtfIncludeLogs.ImageIndex = 25;
            this.chkmtfIncludeLogs.Location = new System.Drawing.Point(10, 29);
            this.chkmtfIncludeLogs.Name = "chkmtfIncludeLogs";
            this.chkmtfIncludeLogs.Size = new System.Drawing.Size(151, 24);
            this.chkmtfIncludeLogs.TabIndex = 124;
            this.chkmtfIncludeLogs.Text = "Log Messages";
            this.chkmtfIncludeLogs.UseVisualStyleBackColor = true;
            //
            // chkCaseSensitive
            //
            this.chkCaseSensitive.Location = new System.Drawing.Point(3, 243);
            this.chkCaseSensitive.Name = "chkCaseSensitive";
            this.chkCaseSensitive.Size = new System.Drawing.Size(236, 24);
            this.chkCaseSensitive.TabIndex = 101;
            this.chkCaseSensitive.Text = "String matches are case sensitive";
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(0, 200);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(471, 13);
            this.label4.TabIndex = 99;
            this.label4.Text = "Exclude any messages which contain one of these strings (; delimited):";
            //
            // txtFilterExcludeStrings
            //
            this.txtFilterExcludeStrings.Location = new System.Drawing.Point(3, 216);
            this.txtFilterExcludeStrings.Name = "txtFilterExcludeStrings";
            this.txtFilterExcludeStrings.Size = new System.Drawing.Size(487, 21);
            this.txtFilterExcludeStrings.TabIndex = 100;
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(-3, 150);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(472, 13);
            this.label5.TabIndex = 98;
            this.label5.Text = "Only include messages which contain one of these strings (; delimited):";
            //
            // txtFilterIncludeStrings
            //
            this.txtFilterIncludeStrings.Location = new System.Drawing.Point(3, 166);
            this.txtFilterIncludeStrings.Name = "txtFilterIncludeStrings";
            this.txtFilterIncludeStrings.Size = new System.Drawing.Size(487, 21);
            this.txtFilterIncludeStrings.TabIndex = 97;
            //
            // tabPage2
            //
            this.tabPage2.Controls.Add(this.pnlDockSplitContainer);
            this.tabPage2.Controls.Add(this.btnClearUnmatchedModules);
            this.tabPage2.Controls.Add(this.btnClearUnmatchedThreads);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(497, 339);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Threads / Modules";
            this.tabPage2.UseVisualStyleBackColor = true;
            //
            // pnlDockSplitContainer
            //
            this.pnlDockSplitContainer.Controls.Add(this.splitContainer1);
            this.pnlDockSplitContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDockSplitContainer.Location = new System.Drawing.Point(3, 3);
            this.pnlDockSplitContainer.Name = "pnlDockSplitContainer";
            this.pnlDockSplitContainer.Size = new System.Drawing.Size(491, 298);
            this.pnlDockSplitContainer.TabIndex = 7;
            //
            // splitContainer1
            //
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            //
            // splitContainer1.Panel1
            //
            this.splitContainer1.Panel1.Controls.Add(this.clbModulesFilter);
            this.splitContainer1.Panel1.Controls.Add(this.clbThreadsFilter);
            //
            // splitContainer1.Panel2
            //
            this.splitContainer1.Panel2.Controls.Add(this.lbxUnmatchedModules);
            this.splitContainer1.Panel2.Controls.Add(this.lbxUnmatchedThreads);
            this.splitContainer1.Size = new System.Drawing.Size(491, 298);
            this.splitContainer1.SplitterDistance = 229;
            this.splitContainer1.TabIndex = 0;
            //
            // clbModulesFilter
            //
            this.clbModulesFilter.CheckOnClick = true;
            this.clbModulesFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbModulesFilter.FormattingEnabled = true;
            this.clbModulesFilter.Location = new System.Drawing.Point(244, 0);
            this.clbModulesFilter.Name = "clbModulesFilter";
            this.clbModulesFilter.ScrollAlwaysVisible = true;
            this.clbModulesFilter.Size = new System.Drawing.Size(247, 228);
            this.clbModulesFilter.TabIndex = 3;
            //
            // clbThreadsFilter
            //
            this.clbThreadsFilter.CheckOnClick = true;
            this.clbThreadsFilter.ColumnWidth = 110;
            this.clbThreadsFilter.Dock = System.Windows.Forms.DockStyle.Left;
            this.clbThreadsFilter.FormattingEnabled = true;
            this.clbThreadsFilter.Location = new System.Drawing.Point(0, 0);
            this.clbThreadsFilter.Name = "clbThreadsFilter";
            this.clbThreadsFilter.ScrollAlwaysVisible = true;
            this.clbThreadsFilter.Size = new System.Drawing.Size(244, 228);
            this.clbThreadsFilter.TabIndex = 2;
            //
            // lbxUnmatchedModules
            //
            this.lbxUnmatchedModules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbxUnmatchedModules.FormattingEnabled = true;
            this.lbxUnmatchedModules.Location = new System.Drawing.Point(244, 0);
            this.lbxUnmatchedModules.Name = "lbxUnmatchedModules";
            this.lbxUnmatchedModules.ScrollAlwaysVisible = true;
            this.lbxUnmatchedModules.Size = new System.Drawing.Size(247, 56);
            this.lbxUnmatchedModules.TabIndex = 7;
            //
            // lbxUnmatchedThreads
            //
            this.lbxUnmatchedThreads.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbxUnmatchedThreads.FormattingEnabled = true;
            this.lbxUnmatchedThreads.Location = new System.Drawing.Point(0, 0);
            this.lbxUnmatchedThreads.Name = "lbxUnmatchedThreads";
            this.lbxUnmatchedThreads.ScrollAlwaysVisible = true;
            this.lbxUnmatchedThreads.Size = new System.Drawing.Size(244, 56);
            this.lbxUnmatchedThreads.TabIndex = 6;
            //
            // btnClearUnmatchedModules
            //
            this.btnClearUnmatchedModules.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearUnmatchedModules.Location = new System.Drawing.Point(311, 310);
            this.btnClearUnmatchedModules.Name = "btnClearUnmatchedModules";
            this.btnClearUnmatchedModules.Size = new System.Drawing.Size(178, 23);
            this.btnClearUnmatchedModules.TabIndex = 4;
            this.btnClearUnmatchedModules.Text = "Remove Unmatched Modules";
            this.btnClearUnmatchedModules.UseVisualStyleBackColor = true;
            //
            // btnClearUnmatchedThreads
            //
            this.btnClearUnmatchedThreads.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearUnmatchedThreads.Location = new System.Drawing.Point(136, 307);
            this.btnClearUnmatchedThreads.Name = "btnClearUnmatchedThreads";
            this.btnClearUnmatchedThreads.Size = new System.Drawing.Size(168, 23);
            this.btnClearUnmatchedThreads.TabIndex = 3;
            this.btnClearUnmatchedThreads.Text = "Remove Unmatched Threads";
            this.btnClearUnmatchedThreads.UseVisualStyleBackColor = true;
            //
            // tabLocationFiltering
            //
            this.tabLocationFiltering.Controls.Add(this.splitContainer2);
            this.tabLocationFiltering.Controls.Add(this.btnClearAdditionalLocFilter);
            this.tabLocationFiltering.Controls.Add(this.btnClearLocClassOverflow);
            this.tabLocationFiltering.Location = new System.Drawing.Point(4, 22);
            this.tabLocationFiltering.Name = "tabLocationFiltering";
            this.tabLocationFiltering.Padding = new System.Windows.Forms.Padding(3);
            this.tabLocationFiltering.Size = new System.Drawing.Size(497, 339);
            this.tabLocationFiltering.TabIndex = 2;
            this.tabLocationFiltering.Text = "Locations";
            this.tabLocationFiltering.UseVisualStyleBackColor = true;
            //
            // splitContainer2
            //
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            //
            // splitContainer2.Panel1
            //
            this.splitContainer2.Panel1.Controls.Add(this.clbAdditionalLocationsFilter);
            this.splitContainer2.Panel1.Controls.Add(this.clbLocationClassFilter);
            //
            // splitContainer2.Panel2
            //
            this.splitContainer2.Panel2.Controls.Add(this.lbxLocAdditionalOverflow);
            this.splitContainer2.Panel2.Controls.Add(this.lbxLocClassOverflow);
            this.splitContainer2.Size = new System.Drawing.Size(491, 301);
            this.splitContainer2.SplitterDistance = 149;
            this.splitContainer2.TabIndex = 15;
            //
            // clbAdditionalLocationsFilter
            //
            this.clbAdditionalLocationsFilter.CheckOnClick = true;
            this.clbAdditionalLocationsFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbAdditionalLocationsFilter.FormattingEnabled = true;
            this.clbAdditionalLocationsFilter.Location = new System.Drawing.Point(215, 0);
            this.clbAdditionalLocationsFilter.Name = "clbAdditionalLocationsFilter";
            this.clbAdditionalLocationsFilter.ScrollAlwaysVisible = true;
            this.clbAdditionalLocationsFilter.Size = new System.Drawing.Size(276, 148);
            this.clbAdditionalLocationsFilter.TabIndex = 11;
            //
            // clbLocationClassFilter
            //
            this.clbLocationClassFilter.CheckOnClick = true;
            this.clbLocationClassFilter.Dock = System.Windows.Forms.DockStyle.Left;
            this.clbLocationClassFilter.FormattingEnabled = true;
            this.clbLocationClassFilter.Location = new System.Drawing.Point(0, 0);
            this.clbLocationClassFilter.Name = "clbLocationClassFilter";
            this.clbLocationClassFilter.ScrollAlwaysVisible = true;
            this.clbLocationClassFilter.Size = new System.Drawing.Size(215, 148);
            this.clbLocationClassFilter.TabIndex = 12;
            //
            // lbxLocAdditionalOverflow
            //
            this.lbxLocAdditionalOverflow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbxLocAdditionalOverflow.Enabled = false;
            this.lbxLocAdditionalOverflow.FormattingEnabled = true;
            this.lbxLocAdditionalOverflow.Location = new System.Drawing.Point(215, 0);
            this.lbxLocAdditionalOverflow.Name = "lbxLocAdditionalOverflow";
            this.lbxLocAdditionalOverflow.ScrollAlwaysVisible = true;
            this.lbxLocAdditionalOverflow.Size = new System.Drawing.Size(276, 147);
            this.lbxLocAdditionalOverflow.TabIndex = 15;
            //
            // lbxLocClassOverflow
            //
            this.lbxLocClassOverflow.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbxLocClassOverflow.Enabled = false;
            this.lbxLocClassOverflow.FormattingEnabled = true;
            this.lbxLocClassOverflow.Location = new System.Drawing.Point(0, 0);
            this.lbxLocClassOverflow.Name = "lbxLocClassOverflow";
            this.lbxLocClassOverflow.ScrollAlwaysVisible = true;
            this.lbxLocClassOverflow.Size = new System.Drawing.Size(215, 147);
            this.lbxLocClassOverflow.TabIndex = 14;
            //
            // btnClearAdditionalLocFilter
            //
            this.btnClearAdditionalLocFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearAdditionalLocFilter.Location = new System.Drawing.Point(319, 309);
            this.btnClearAdditionalLocFilter.Name = "btnClearAdditionalLocFilter";
            this.btnClearAdditionalLocFilter.Size = new System.Drawing.Size(171, 23);
            this.btnClearAdditionalLocFilter.TabIndex = 12;
            this.btnClearAdditionalLocFilter.Text = "Remove Overflow Locations";
            this.btnClearAdditionalLocFilter.UseVisualStyleBackColor = true;
            //
            // btnClearLocClassOverflow
            //
            this.btnClearLocClassOverflow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearLocClassOverflow.Location = new System.Drawing.Point(63, 310);
            this.btnClearLocClassOverflow.Name = "btnClearLocClassOverflow";
            this.btnClearLocClassOverflow.Size = new System.Drawing.Size(155, 23);
            this.btnClearLocClassOverflow.TabIndex = 11;
            this.btnClearLocClassOverflow.Text = "Remove Overflow Classes";
            this.btnClearLocClassOverflow.UseVisualStyleBackColor = true;
            //
            // tabPage3
            //
            this.tabPage3.Controls.Add(this.panel2);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.txtExcludeAllAboveThisIndex);
            this.tabPage3.Controls.Add(this.txtExcludeAllBelowThisIndex);
            this.tabPage3.Controls.Add(this.chkEnableIndexFilter);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(497, 339);
            this.tabPage3.TabIndex = 3;
            this.tabPage3.Text = "Advanced Filters";
            this.tabPage3.UseVisualStyleBackColor = true;
            //
            // panel2
            //
            this.panel2.BackColor = System.Drawing.SystemColors.Window;
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.chkmtfSessionMsgs);
            this.panel2.Controls.Add(this.chkmtfChainMsgs);
            this.panel2.Controls.Add(this.chkmtfCommandMsgs);
            this.panel2.Controls.Add(this.chkmtfInternalMessages);
            this.panel2.Controls.Add(this.chkmtfResourceMessages);
            this.panel2.Controls.Add(this.chkmtfExceptionContent);
            this.panel2.Location = new System.Drawing.Point(6, 176);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(402, 157);
            this.panel2.TabIndex = 109;
            //
            // label7
            //
            this.label7.Location = new System.Drawing.Point(13, 10);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(387, 37);
            this.label7.TabIndex = 144;
            this.label7.Text = "N.B. Ensure you have ticked the option \"Allow Internal Messages To Be Displayed\" " +
                "in Mex Options before using these filters.";
            //
            // chkmtfSessionMsgs
            //
            this.chkmtfSessionMsgs.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkmtfSessionMsgs.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.chkmtfSessionMsgs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkmtfSessionMsgs.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkmtfSessionMsgs.ImageIndex = 5;
            this.chkmtfSessionMsgs.Location = new System.Drawing.Point(183, 89);
            this.chkmtfSessionMsgs.Name = "chkmtfSessionMsgs";
            this.chkmtfSessionMsgs.Size = new System.Drawing.Size(161, 24);
            this.chkmtfSessionMsgs.TabIndex = 143;
            this.chkmtfSessionMsgs.Text = "Section Messages";
            this.chkmtfSessionMsgs.UseVisualStyleBackColor = true;
            //
            // chkmtfChainMsgs
            //
            this.chkmtfChainMsgs.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkmtfChainMsgs.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.chkmtfChainMsgs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkmtfChainMsgs.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkmtfChainMsgs.ImageIndex = 8;
            this.chkmtfChainMsgs.Location = new System.Drawing.Point(183, 59);
            this.chkmtfChainMsgs.Name = "chkmtfChainMsgs";
            this.chkmtfChainMsgs.Size = new System.Drawing.Size(161, 24);
            this.chkmtfChainMsgs.TabIndex = 142;
            this.chkmtfChainMsgs.Text = "Chain Messages";
            this.chkmtfChainMsgs.UseVisualStyleBackColor = true;
            //
            // chkmtfCommandMsgs
            //
            this.chkmtfCommandMsgs.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkmtfCommandMsgs.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.chkmtfCommandMsgs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkmtfCommandMsgs.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkmtfCommandMsgs.ImageIndex = 0;
            this.chkmtfCommandMsgs.Location = new System.Drawing.Point(183, 119);
            this.chkmtfCommandMsgs.Name = "chkmtfCommandMsgs";
            this.chkmtfCommandMsgs.Size = new System.Drawing.Size(161, 24);
            this.chkmtfCommandMsgs.TabIndex = 141;
            this.chkmtfCommandMsgs.Text = "Command Messages";
            this.chkmtfCommandMsgs.UseVisualStyleBackColor = true;
            //
            // chkmtfInternalMessages
            //
            this.chkmtfInternalMessages.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkmtfInternalMessages.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.chkmtfInternalMessages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkmtfInternalMessages.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkmtfInternalMessages.ImageIndex = 2;
            this.chkmtfInternalMessages.Location = new System.Drawing.Point(16, 119);
            this.chkmtfInternalMessages.Name = "chkmtfInternalMessages";
            this.chkmtfInternalMessages.Size = new System.Drawing.Size(161, 24);
            this.chkmtfInternalMessages.TabIndex = 140;
            this.chkmtfInternalMessages.Text = "Internal Messages";
            this.chkmtfInternalMessages.UseVisualStyleBackColor = true;
            //
            // chkmtfResourceMessages
            //
            this.chkmtfResourceMessages.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkmtfResourceMessages.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.chkmtfResourceMessages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkmtfResourceMessages.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkmtfResourceMessages.ImageIndex = 29;
            this.chkmtfResourceMessages.Location = new System.Drawing.Point(16, 59);
            this.chkmtfResourceMessages.Name = "chkmtfResourceMessages";
            this.chkmtfResourceMessages.Size = new System.Drawing.Size(161, 24);
            this.chkmtfResourceMessages.TabIndex = 139;
            this.chkmtfResourceMessages.Text = "Resource Messages";
            this.chkmtfResourceMessages.UseVisualStyleBackColor = true;
            //
            // chkmtfExceptionContent
            //
            this.chkmtfExceptionContent.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkmtfExceptionContent.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.chkmtfExceptionContent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkmtfExceptionContent.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkmtfExceptionContent.ImageIndex = 22;
            this.chkmtfExceptionContent.Location = new System.Drawing.Point(16, 89);
            this.chkmtfExceptionContent.Name = "chkmtfExceptionContent";
            this.chkmtfExceptionContent.Size = new System.Drawing.Size(161, 24);
            this.chkmtfExceptionContent.TabIndex = 138;
            this.chkmtfExceptionContent.Text = "Exception Content";
            this.chkmtfExceptionContent.UseVisualStyleBackColor = true;
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(41, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(173, 13);
            this.label2.TabIndex = 107;
            this.label2.Text = "Exclude higher indexes than:";
            //
            // txtExcludeAllAboveThisIndex
            //
            this.txtExcludeAllAboveThisIndex.Enabled = false;
            this.txtExcludeAllAboveThisIndex.Location = new System.Drawing.Point(215, 84);
            this.txtExcludeAllAboveThisIndex.Name = "txtExcludeAllAboveThisIndex";
            this.txtExcludeAllAboveThisIndex.Size = new System.Drawing.Size(89, 21);
            this.txtExcludeAllAboveThisIndex.TabIndex = 106;
            this.txtExcludeAllAboveThisIndex.TextChanged += new System.EventHandler(this.txtExcludeAllAboveThisIndex_TextChanged);
            //
            // txtExcludeAllBelowThisIndex
            //
            this.txtExcludeAllBelowThisIndex.Enabled = false;
            this.txtExcludeAllBelowThisIndex.Location = new System.Drawing.Point(215, 52);
            this.txtExcludeAllBelowThisIndex.Name = "txtExcludeAllBelowThisIndex";
            this.txtExcludeAllBelowThisIndex.Size = new System.Drawing.Size(89, 21);
            this.txtExcludeAllBelowThisIndex.TabIndex = 104;
            this.txtExcludeAllBelowThisIndex.TextChanged += new System.EventHandler(this.txtExcludeAllBelowThisIndex_TextChanged);
            //
            // chkEnableIndexFilter
            //
            this.chkEnableIndexFilter.AutoSize = true;
            this.chkEnableIndexFilter.Location = new System.Drawing.Point(7, 31);
            this.chkEnableIndexFilter.Name = "chkEnableIndexFilter";
            this.chkEnableIndexFilter.Size = new System.Drawing.Size(116, 17);
            this.chkEnableIndexFilter.TabIndex = 108;
            this.chkEnableIndexFilter.Text = "Use Index Filter";
            this.chkEnableIndexFilter.UseVisualStyleBackColor = true;
            this.chkEnableIndexFilter.CheckedChanged += new System.EventHandler(this.chkEnableIndexFilter_CheckedChanged);
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(168, 13);
            this.label1.TabIndex = 105;
            this.label1.Text = "Exclude lower indexes than:";
            //
            // tabPage4
            //
            this.tabPage4.Controls.Add(this.txtFilterExtension);
            this.tabPage4.Controls.Add(this.chkUseFilterOnLoad);
            this.tabPage4.Controls.Add(this.lblSaveData);
            this.tabPage4.Controls.Add(this.btnRefreshFilterDir);
            this.tabPage4.Controls.Add(this.txtFilterFilename);
            this.tabPage4.Controls.Add(this.txtFilterDirectory);
            this.tabPage4.Controls.Add(this.groupBox1);
            this.tabPage4.Controls.Add(this.lbxAllMatchedFilters);
            this.tabPage4.Controls.Add(this.chkAllowOverwrite);
            this.tabPage4.Controls.Add(this.btnLoadFilterConfiguration);
            this.tabPage4.Controls.Add(this.btnSaveFilterConfiguration);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(497, 339);
            this.tabPage4.TabIndex = 4;
            this.tabPage4.Text = "Load / Save";
            this.tabPage4.UseVisualStyleBackColor = true;
            //
            // txtFilterExtension
            //
            this.txtFilterExtension.Location = new System.Drawing.Point(411, 65);
            this.txtFilterExtension.Name = "txtFilterExtension";
            this.txtFilterExtension.ReadOnly = true;
            this.txtFilterExtension.Size = new System.Drawing.Size(66, 21);
            this.txtFilterExtension.TabIndex = 118;
            //
            // chkUseFilterOnLoad
            //
            this.chkUseFilterOnLoad.AutoSize = true;
            this.chkUseFilterOnLoad.Location = new System.Drawing.Point(7, 212);
            this.chkUseFilterOnLoad.Name = "chkUseFilterOnLoad";
            this.chkUseFilterOnLoad.Size = new System.Drawing.Size(238, 17);
            this.chkUseFilterOnLoad.TabIndex = 117;
            this.chkUseFilterOnLoad.Text = "Attempt to load this filter on start up.";
            this.chkUseFilterOnLoad.UseVisualStyleBackColor = true;
            this.chkUseFilterOnLoad.CheckedChanged += new System.EventHandler(this.chkUseFilterOnLoad_CheckedChanged);
            //
            // lblSaveData
            //
            this.lblSaveData.AutoSize = true;
            this.lblSaveData.Location = new System.Drawing.Point(6, 45);
            this.lblSaveData.Name = "lblSaveData";
            this.lblSaveData.Size = new System.Drawing.Size(41, 13);
            this.lblSaveData.TabIndex = 116;
            this.lblSaveData.Text = "label6";
            //
            // btnRefreshFilterDir
            //
            this.btnRefreshFilterDir.Location = new System.Drawing.Point(307, 96);
            this.btnRefreshFilterDir.Name = "btnRefreshFilterDir";
            this.btnRefreshFilterDir.Size = new System.Drawing.Size(170, 23);
            this.btnRefreshFilterDir.TabIndex = 115;
            this.btnRefreshFilterDir.Text = "Refresh";
            this.btnRefreshFilterDir.UseVisualStyleBackColor = true;
            this.btnRefreshFilterDir.Click += new System.EventHandler(this.btnRefresh_Click);
            //
            // txtFilterFilename
            //
            this.txtFilterFilename.Location = new System.Drawing.Point(307, 65);
            this.txtFilterFilename.Name = "txtFilterFilename";
            this.txtFilterFilename.Size = new System.Drawing.Size(98, 21);
            this.txtFilterFilename.TabIndex = 114;
            this.txtFilterFilename.TextChanged += new System.EventHandler(this.txtFilterFilename_TextChanged);
            //
            // txtFilterDirectory
            //
            this.txtFilterDirectory.Location = new System.Drawing.Point(6, 21);
            this.txtFilterDirectory.Name = "txtFilterDirectory";
            this.txtFilterDirectory.ReadOnly = true;
            this.txtFilterDirectory.Size = new System.Drawing.Size(471, 21);
            this.txtFilterDirectory.TabIndex = 113;
            //
            // groupBox1
            //
            this.groupBox1.Controls.Add(this.chkSaveModules);
            this.groupBox1.Controls.Add(this.chkSaveClassLocationInfo);
            this.groupBox1.Controls.Add(this.chkSaveLocationInformation);
            this.groupBox1.Controls.Add(this.chkSaveIncludeThreads);
            this.groupBox1.Location = new System.Drawing.Point(7, 246);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(470, 74);
            this.groupBox1.TabIndex = 111;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Include this filter information:";
            //
            // chkSaveModules
            //
            this.chkSaveModules.AutoSize = true;
            this.chkSaveModules.Location = new System.Drawing.Point(6, 43);
            this.chkSaveModules.Name = "chkSaveModules";
            this.chkSaveModules.Size = new System.Drawing.Size(170, 17);
            this.chkSaveModules.TabIndex = 3;
            this.chkSaveModules.Text = "Save Module Information";
            this.chkSaveModules.UseVisualStyleBackColor = true;
            //
            // chkSaveClassLocationInfo
            //
            this.chkSaveClassLocationInfo.AutoSize = true;
            this.chkSaveClassLocationInfo.Location = new System.Drawing.Point(226, 43);
            this.chkSaveClassLocationInfo.Name = "chkSaveClassLocationInfo";
            this.chkSaveClassLocationInfo.Size = new System.Drawing.Size(212, 17);
            this.chkSaveClassLocationInfo.TabIndex = 2;
            this.chkSaveClassLocationInfo.Text = "Save Class Location Information";
            this.chkSaveClassLocationInfo.UseVisualStyleBackColor = true;
            //
            // chkSaveLocationInformation
            //
            this.chkSaveLocationInformation.AutoSize = true;
            this.chkSaveLocationInformation.Location = new System.Drawing.Point(226, 20);
            this.chkSaveLocationInformation.Name = "chkSaveLocationInformation";
            this.chkSaveLocationInformation.Size = new System.Drawing.Size(195, 17);
            this.chkSaveLocationInformation.TabIndex = 1;
            this.chkSaveLocationInformation.Text = "Save All Location Information";
            this.chkSaveLocationInformation.UseVisualStyleBackColor = true;
            //
            // chkSaveIncludeThreads
            //
            this.chkSaveIncludeThreads.AutoSize = true;
            this.chkSaveIncludeThreads.Location = new System.Drawing.Point(6, 20);
            this.chkSaveIncludeThreads.Name = "chkSaveIncludeThreads";
            this.chkSaveIncludeThreads.Size = new System.Drawing.Size(170, 17);
            this.chkSaveIncludeThreads.TabIndex = 0;
            this.chkSaveIncludeThreads.Text = "Save Thread Information";
            this.chkSaveIncludeThreads.UseVisualStyleBackColor = true;
            //
            // lbxAllMatchedFilters
            //
            this.lbxAllMatchedFilters.FormattingEnabled = true;
            this.lbxAllMatchedFilters.Location = new System.Drawing.Point(6, 64);
            this.lbxAllMatchedFilters.Name = "lbxAllMatchedFilters";
            this.lbxAllMatchedFilters.Size = new System.Drawing.Size(295, 134);
            this.lbxAllMatchedFilters.TabIndex = 110;
            this.lbxAllMatchedFilters.SelectedIndexChanged += new System.EventHandler(this.lbxAllMatchedFilters_SelectedIndexChanged);
            //
            // chkAllowOverwrite
            //
            this.chkAllowOverwrite.AutoSize = true;
            this.chkAllowOverwrite.Location = new System.Drawing.Point(307, 183);
            this.chkAllowOverwrite.Name = "chkAllowOverwrite";
            this.chkAllowOverwrite.Size = new System.Drawing.Size(116, 17);
            this.chkAllowOverwrite.TabIndex = 109;
            this.chkAllowOverwrite.Text = "Allow Overwrite";
            this.chkAllowOverwrite.UseVisualStyleBackColor = true;
            //
            // btnLoadFilterConfiguration
            //
            this.btnLoadFilterConfiguration.Location = new System.Drawing.Point(307, 154);
            this.btnLoadFilterConfiguration.Name = "btnLoadFilterConfiguration";
            this.btnLoadFilterConfiguration.Size = new System.Drawing.Size(170, 23);
            this.btnLoadFilterConfiguration.TabIndex = 108;
            this.btnLoadFilterConfiguration.Text = "Load";
            this.btnLoadFilterConfiguration.UseVisualStyleBackColor = true;
            this.btnLoadFilterConfiguration.Click += new System.EventHandler(this.btnLoadFilterConfiguration_Click_1);
            //
            // btnSaveFilterConfiguration
            //
            this.btnSaveFilterConfiguration.Location = new System.Drawing.Point(307, 125);
            this.btnSaveFilterConfiguration.Name = "btnSaveFilterConfiguration";
            this.btnSaveFilterConfiguration.Size = new System.Drawing.Size(170, 23);
            this.btnSaveFilterConfiguration.TabIndex = 107;
            this.btnSaveFilterConfiguration.Text = "Save";
            this.btnSaveFilterConfiguration.UseVisualStyleBackColor = true;
            this.btnSaveFilterConfiguration.Click += new System.EventHandler(this.btnSaveFilterConfiguration_Click);
            //
            // btnSetDefaultFilter
            //
            this.btnSetDefaultFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetDefaultFilter.Location = new System.Drawing.Point(181, 376);
            this.btnSetDefaultFilter.Name = "btnSetDefaultFilter";
            this.btnSetDefaultFilter.Size = new System.Drawing.Size(72, 23);
            this.btnSetDefaultFilter.TabIndex = 107;
            this.btnSetDefaultFilter.Text = "Default";
            this.btnSetDefaultFilter.UseVisualStyleBackColor = true;
            this.btnSetDefaultFilter.Click += new System.EventHandler(this.btnSetDefaultFilter_Click);
            //
            // cboQuickLoad
            //
            this.cboQuickLoad.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboQuickLoad.FormattingEnabled = true;
            this.cboQuickLoad.Location = new System.Drawing.Point(6, 376);
            this.cboQuickLoad.Name = "cboQuickLoad";
            this.cboQuickLoad.Size = new System.Drawing.Size(161, 21);
            this.cboQuickLoad.TabIndex = 108;
            this.cboQuickLoad.SelectedIndexChanged += new System.EventHandler(this.cboQuickLoad_SelectedIndexChanged);
            //
            // frmProcViewFilter
            //
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(512, 425);
            this.Controls.Add(this.cboQuickLoad);
            this.Controls.Add(this.btnSetDefaultFilter);
            this.Controls.Add(this.tbcFilterDetails);
            this.Controls.Add(this.btnLoadMexDefaultFilter);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(868, 452);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(520, 452);
            this.Name = "frmProcViewFilter";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Filter Options.";
            this.tbcFilterDetails.ResumeLayout(false);
            this.tabOptionsEvents.ResumeLayout(false);
            this.tabOptionsEvents.PerformLayout();
            this.pnlFilterButtons.ResumeLayout(false);
            this.pnlFilterButtons.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.pnlDockSplitContainer.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tabLocationFiltering.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion Windows Form Designer generated code

        // End PopulateAFilterDialogFromStrings

        private void btnLoadFilterConfiguration_Click_1(object sender, EventArgs e) {
            string filename = Path.Combine(MexCore.TheCore.Options.FilterAndHighlightStoreDirectory, lbxAllMatchedFilters.SelectedItem.ToString() + MexCore.TheCore.Options.FilterExtension);

            //Bilge.Assert(File.Exists(filename), "The filename selected from the list of filteres did not exist.  This should not be possible");

            this.InitialiseFromExistingFilter(ViewFilter.LoadFilterFromFile(filename));
            tbcFilterDetails.SelectedTab = tabOptionsEvents;
        }

        private void btnLoadMexDefaultFilter_Click(object sender, EventArgs e) {
            ViewFilter vf = new ViewFilter();
            this.InitialiseFromExistingFilter(vf);
            this.DialogResult = DialogResult.OK;
            this.FullRefreshRequired = true;
            Close();
        }

        private void btnOKWithFullRefresh_Click(object sender, System.EventArgs e) {
            FullRefreshRequired = true;
        }

        private void btnRefresh_Click(object sender, EventArgs e) {
            InitialiseLoadSaveScreen();
        }

        private void btnSaveFilterConfiguration_Click(object sender, EventArgs e) {
            string fileName = Path.Combine(MexCore.TheCore.Options.FilterAndHighlightStoreDirectory, txtFilterFilename.Text + MexCore.TheCore.Options.FilterExtension);
            ViewFilter af = GetFilterFromDialog();
            ViewFilter.SaveFilterToFile(fileName, af, chkSaveIncludeThreads.Checked, chkSaveModules.Checked, chkSaveLocationInformation.Checked, chkSaveClassLocationInfo.Checked);
            btnSaveFilterConfiguration.Enabled = false;
            RefreshFiltersList();
        }

        private void btnSetDefaultFilter_Click(object sender, EventArgs e) {
            ViewFilter vf = new ViewFilter();
            this.InitialiseFromExistingFilter(vf);
            tbcFilterDetails.SelectedTab = tabOptionsEvents;
        }

        private void cboQuickLoad_SelectedIndexChanged(object sender, EventArgs e) {
            string filterName = cboQuickLoad.Text;

            filterName = Path.Combine(MexCore.TheCore.Options.FilterAndHighlightStoreDirectory, filterName + MexCore.TheCore.Options.FilterExtension);

            if (File.Exists(filterName)) {
                ViewFilter vf = ViewFilter.LoadFilterFromFile(filterName);
                this.InitialiseFromExistingFilter(vf);
                this.DialogResult = DialogResult.OK;
                this.Close();
            } else {
                MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.FilterFileNotFound, UserMessageType.WarningMessage, filterName);
            }
        }

        private void chkEnableIndexFilter_CheckedChanged(object sender, EventArgs e) {
            txtExcludeAllAboveThisIndex.Enabled = txtExcludeAllBelowThisIndex.Enabled = chkEnableIndexFilter.Checked;
        }

        private void chkUseFilterOnLoad_CheckedChanged(object sender, EventArgs e) {
            if (!autochanging) {
                // when this is changed if a filter is selected then it should be loaded when Mex starts.
                if (chkUseFilterOnLoad.Checked) {
                    MexCore.TheCore.Options.FilterFilenameToLoadOnStartup = Path.Combine(MexCore.TheCore.Options.FilterAndHighlightStoreDirectory, txtFilterFilename.Text + MexCore.TheCore.Options.FilterExtension);
                } else {
                    MexCore.TheCore.Options.FilterFilenameToLoadOnStartup = "";
                }
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

        private void lbxAllMatchedFilters_SelectedIndexChanged(object sender, EventArgs e) {
            autochanging = true;
            if (lbxAllMatchedFilters.SelectedItem != null) {
                txtFilterFilename.Text = lbxAllMatchedFilters.SelectedItem.ToString();
                chkUseFilterOnLoad.Enabled = true;
                chkUseFilterOnLoad.Checked = (MexCore.TheCore.Options.FilterFilenameToLoadOnStartup == Path.Combine(MexCore.TheCore.Options.FilterAndHighlightStoreDirectory, txtFilterFilename.Text + MexCore.TheCore.Options.FilterExtension));
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
                lbxAllMatchedFilters.Items.Add(nextOne);
                cboQuickLoad.Items.Add(nextOne);
            }

            cboQuickLoad.Text = " > Select A Filter To Quick Load <";
            if (matchedFilters.Length == 0) {
                lblSaveData.Text = "No filters found.";
            }
        }

        private void txtExcludeAllAboveThisIndex_TextChanged(object sender, EventArgs e) {
            int idxVal;
            if (!int.TryParse(txtExcludeAllAboveThisIndex.Text, out idxVal) || (idxVal < 0)) {
                // error.
                txtExcludeAllAboveThisIndex.BackColor = Color.IndianRed;
                higherIndexValue = int.MaxValue;
            } else {
                txtExcludeAllAboveThisIndex.BackColor = Color.FromKnownColor(KnownColor.Window);
                higherIndexValue = idxVal;
            }
        }

        private void txtExcludeAllBelowThisIndex_TextChanged(object sender, EventArgs e) {
            int idxVal;
            if (!int.TryParse(txtExcludeAllBelowThisIndex.Text, out idxVal) || (idxVal < 0)) {
                // error.
                txtExcludeAllBelowThisIndex.BackColor = Color.IndianRed;
                lowerIndexValue = int.MinValue;
            } else {
                txtExcludeAllBelowThisIndex.BackColor = Color.FromKnownColor(KnownColor.Window);
                lowerIndexValue = idxVal;
            }
        }

        private void txtFilterFilename_TextChanged(object sender, EventArgs e) {
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