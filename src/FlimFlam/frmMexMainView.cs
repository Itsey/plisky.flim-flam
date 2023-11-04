namespace Plisky.FlimFlam {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using OldFlimflam.Screens;
    using Plisky.Diagnostics;
    using Plisky.Diagnostics.FlimFlam;

    public partial class frmMexMainView : Form {
        internal MexStatus ViewerStatus;
        private readonly System.Windows.Forms.Timer tmr = new();

        private const string MEXISO_APP_IDENT = "Mex_2_0";
        private const string MEX_SETTINGSFILENAME = "AppSettings.xml";
        private const string MEX_OPTIONSFILENAME = "AppOptions.xml";
        private static MexCore Core; // Default null

        internal static ImageList AppImages;
        private List<frmViewInAWindow> m_PromotedViews;

        // In order to delegate this we need to make it a member to get at it from the redraw parameter less delegate

        private readonly List<LinkLabel> processLinkLabelCache = new();
        private int processLinkLabelFirstOffset;

        private long currentFindIncrementalIndexPreference = -1;
        private bool refreshNeeded = false;

        public frmMexMainView() {
            InitializeComponent();
            _ = sptSelector.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Select All", null, tsHandler_Click));
            _ = sptSelector.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Select None", null, tsHandler_Click));
            _ = sptSelector.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Select All Starting With", null, tsHandler_Click));
            _ = sptSelector.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Clear then select all starting with:", null, tsHandler_Click));
            _ = sptSelector.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Select All Not Starting With", null, tsHandler_Click));
            _ = sptSelector.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Experimental Purge", null, tsHandler_Click));

            _ = spbtSelectThreads.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Select All", null, tsThreadViewHandler_Click));
            _ = spbtSelectThreads.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Select None", null, tsThreadViewHandler_Click));
            _ = spbtSelectThreads.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Invert Selection", null, tsThreadViewHandler_Click));

            Core = MexCore.TheCore;

            ViewerStatus = new MexStatus(); // MRawEntryParserChain.csexStatus();
            AppImages = imgsMexIcons;
            RefreshQuickIcons();

            tmr.Tick += Tmr_Tick;
            tmr.Interval = 500;
        }

        private void Tmr_Tick(object sender, EventArgs e) {
            if (refreshNeeded) {
                PerformActualAutoRefresh();
            }
        }

        private void sptSelector_Click(object sender, EventArgs e) {
            var tsmi = new ToolStripMenuItem("Select All", null, tsHandler_Click);
            tsHandler_Click(tsmi, null);
        }

        private void tsHandler_Click(object sender, EventArgs e) {
            string sendText = ((ToolStripMenuItem)sender).Text;
            string matchText = txtFilterStartsWith.Text.ToLower();

            switch (sendText) {
                case "Select All":
                    for (int i = 0; i < clbAllKnownProcesses.Items.Count; i++) {
                        clbAllKnownProcesses.SetItemChecked(i, true);
                    }
                    break;

                case "Select None":
                    for (int i = 0; i < clbAllKnownProcesses.Items.Count; i++) {
                        clbAllKnownProcesses.SetItemChecked(i, false);
                    }
                    break;

                case "Select All Starting With":

                    for (int i = 0; i < clbAllKnownProcesses.Items.Count; i++) {
                        string value = clbAllKnownProcesses.Items[i].ToString();
                        value = value.ToLower();
                        if (value.StartsWith(matchText)) {
                            clbAllKnownProcesses.SetItemChecked(i, true);
                        }
                    }
                    break;

                case "Clear then select all starting with:":

                    for (int i = 0; i < clbAllKnownProcesses.Items.Count; i++) {
                        string value = clbAllKnownProcesses.Items[i].ToString();
                        value = value.ToLower();
                        if (value.StartsWith(matchText)) {
                            clbAllKnownProcesses.SetItemChecked(i, true);
                        } else {
                            clbAllKnownProcesses.SetItemChecked(i, false);
                        }
                    }
                    break;

                case "Select All Not Starting With":

                    for (int i = 0; i < clbAllKnownProcesses.Items.Count; i++) {
                        string value = clbAllKnownProcesses.Items[i].ToString();
                        value = value.ToLower();
                        if (!value.StartsWith(matchText)) {
                            clbAllKnownProcesses.SetItemChecked(i, true);
                        }
                    }
                    break;

                case "Experimental Purge":
                    AlterScreenEnableState(ScreenEnabledStates.InvalidProcessSelected);
                    var indexesToPurge = new List<int>();
                    foreach (ProcessSummary cps in clbAllKnownProcesses.CheckedItems) {
                        indexesToPurge.Add(cps.InternalIndex);
                    }
                    MexCore.TheCore.ViewManager.RequestMultiPurge(indexesToPurge);
                    SelectCrossProcessView();

                    break;
            }
            //Bilge.Log("Called");
        }

        private void tsThreadViewHandler_Click(object sender, EventArgs e) {
            string sendText = sender == null ? "Select All" : ((ToolStripMenuItem)sender).Text;
            switch (sendText) {
                case "Select All":
                    for (int i = 0; i < clbThreadList.Items.Count; i++) {
                        clbThreadList.SetItemChecked(i, true);
                    }
                    break;

                case "Select None":
                    for (int i = 0; i < clbThreadList.Items.Count; i++) {
                        clbThreadList.SetItemChecked(i, false);
                    }
                    break;

                case "Invert Selection":
                    for (int i = 0; i < clbThreadList.Items.Count; i++) {
                        clbThreadList.SetItemChecked(i, !clbThreadList.GetItemChecked(i));
                    }
                    break;
            }
        }

        internal static void LoadDefaultFiltersAndHighlights() {
            if (MexCore.TheCore.Options.FilterFilenameToLoadOnStartup != null) {
                //Bilge.Log("Default filter profile has been detected, loading the default profile.");

                string filterName = Path.Combine(MexCore.TheCore.Options.FilterAndHighlightStoreDirectory, MexCore.TheCore.Options.FilterFilenameToLoadOnStartup + MexCore.TheCore.Options.FilterExtension);
                //Bilge.FurtherInfo("Filename for default filter load is " + filterName);

                if (File.Exists(filterName)) {
                    //Bilge.VerboseLog("Loading filter from disk");
                    var vf = ViewFilter.LoadFilterFromFile(filterName);
                    MexCore.TheCore.ViewManager.CurrentFilter = vf;
                    //Bilge.VerboseLog("Filter load completes, filter active");
                } else {
                    //Bilge.Warning("Default filter filename could not be found, ignoring filter load.", "Filename:" + filterName);
                    MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.DefaultFilterFileNotFound, UserMessageType.WarningMessage, filterName);
                }
            }

            if (MexCore.TheCore.Options.HighlightDefaultProfileName != null) {
                //Bilge.Log("Default highlight profile has been detected, loading the default highlight profile.");
                //Bilge.Warning(" DEFAULT HIGLIGHT FILTERING NOT IMPLEMENTED");
            }
        }

        private void SaveViewerConfigurationData() {
            //Bilge.Log("Saving Viewer status:");
            //Bilge.Dump(ViewerStatus);

            try {
                using var isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
                using (var storeStream = new IsolatedStorageFileStream(MEX_SETTINGSFILENAME, FileMode.Create, isoStore)) {
                    ViewerStatus.SaveViewerStatus(storeStream);
                    storeStream.Close();
                } // End using the settings file in the iso store.

                using var storeStream2 = new IsolatedStorageFileStream(MEX_OPTIONSFILENAME, FileMode.Create, isoStore);
                Core.Options.SaveToFile(storeStream2);
                storeStream2.Close();
                // End using the settings file in the iso store.
                //End using the isolated storage
            } catch (Exception) {
                //Bilge.Dump(exx, "Exception trying to save user settings to the isolated storage system");
                throw;
            }
        }

        internal void LoadViewerConfigurationData() {
            bool recover = false;
            try {
                //Bilge.Log("Loading Viewer Status, importing f.ViewerStatus from IsolatedStorage\\" + MEX_SETTINGSFILENAME);
                using (var isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null)) {

                    #region Load the options for the viewer

                    //Bilge.Log("Loading Viewer Status, importing Core.Options from IsolatedStorage\\" + MEX_OPTIONSFILENAME);

                    if (isoStore.GetFileNames(MEX_OPTIONSFILENAME).Length == 1) {
                        using var storeStream = new IsolatedStorageFileStream(MEX_OPTIONSFILENAME, FileMode.Open, isoStore);
                        try {
                            if (storeStream.Length > 0) {
                                var mo = MexOptions.LoadFromFile(storeStream);
                                MexCore.TheCore.Options = new MexOptions(mo);
                                MexCore.TheCore.Options.ApplyOptionsToApplication(mo, true);
                            }
                            storeStream.Close();
                        } catch (NullReferenceException) {
                            //Bilge.Dump(nrx, "Null reference exception when trying to load the application settings");
                            recover = true;
                        } catch (InvalidOperationException) {
                            //Bilge.Dump(ioe, "Error loading application Core.Options from isolated storage, adopting defaults instead.");
                            //Bilge.Warning("RECOVERYMODE >> Isolated Storage Error, deleting options file and loading defaults for Core.Options");
                            recover = true;
                        } // End Try / Catch around the load

                        if (recover) {
                            Core.Options.LoadOptionsDefaults();
                            MexCore.TheCore.Options.ApplyOptionsToApplication(Core.Options, true);
                            try {
                                isoStore.DeleteFile(MEX_OPTIONSFILENAME);
                            } catch (IsolatedStorageException) {
                                //Bilge.Warning("DOUBLEFAULT >> Isolated storage mode tried to recover but the file for Core.Options was locked and therefore recovery failed");
                                //Bilge.Dump(isox);
                            } // End Try / Catch around the delete
                        }  // End if recovery is needed
                           // End using the iso storage stream
                    } else {
                        //Bilge.Log("No application options file was found, loading defaults for the application");
                        Core.Options.LoadOptionsDefaults();
                    }

                    #endregion Load the options for the viewer

                    bool recoversettings = false;

                    #region Load the Settings for the viewer

                    if (isoStore.GetFileNames(MEX_SETTINGSFILENAME).Length == 1) {
                        using var storeStream = new IsolatedStorageFileStream(MEX_SETTINGSFILENAME, FileMode.Open, isoStore);
                        try {
                            if (storeStream.Length > 0) {
                                ViewerStatus.LoadViewerStatus(storeStream);
                                ApplyViewerStatus();
                            }
                            storeStream.Close();
                        } catch (NullReferenceException) {
                            //Bilge.Dump(nrx, "Null reference when trying to load the file");
                            recoversettings = true;
                        } catch (InvalidOperationException) {
                            //Bilge.Dump(ioe, "Error loading application f.ViewerStatus from isolated storage, adopting defaults instead.");
                            //Bilge.Warning("RECOVERYMODE >> Isolated Storage Error, deleting settings file and loading defaults for Viewer f.ViewerStatus");
                            recoversettings = true;
                        }// End Try / Catch around the load

                        if (recoversettings) {
                            ViewerStatus.LoadDefaultStatus();

                            try {
                                isoStore.DeleteFile(MEX_SETTINGSFILENAME);
                            } catch (IsolatedStorageException) {
                                //Bilge.Warning("DOUBLEFAULT >> Isolated storage mode tried to recover but the file for f.ViewerStatus was locked and therefore recovery failed");
                                //Bilge.Dump(isox);
                            } // End Try / Catch around the delete
                        }  // end recover settings
                           // End using the iso storage stream
                    } else {
                        //Bilge.Log("No viewer settings filename was found loading default viwer settings");
                        ViewerStatus.LoadDefaultStatus();
                    }

                    #endregion Load the Settings for the viewer
                }
                // Now apply both sets of settings to the application
                SetRefreshTime(Core.Options.NoSecondsForUIUpdate);
            } catch (IsolatedStorageException) {
                //Bilge.Warning("Failed to import user specified settings for application, exception occured retrieving settings", ise.Message);
                //Bilge.Dump(ise, "There was an error using and loading the settings from isolated storage. User settings not recovered.");
            }
        }

        internal void RefreshQuickFilterList() {
            //Bilge.Assert(MexCore.TheCore.Options.FilterAndHighlightStoreDirectory != null, "The filter and highlight store directory can not be null");
            //Bilge.Assert(Directory.Exists(MexCore.TheCore.Options.FilterAndHighlightStoreDirectory), "The filter and higlight store directory MUST exist");

            string[] matchedFilters = Directory.GetFiles(MexCore.TheCore.Options.FilterAndHighlightStoreDirectory, "*" + MexCore.TheCore.Options.FilterExtension);

            cboMainViewQuickFilterChange.Items.Clear();

            foreach (string s in matchedFilters) {
                _ = cboMainViewQuickFilterChange.Items.Add(Path.GetFileNameWithoutExtension(s));
            }
            _ = cboMainViewQuickFilterChange.Items.Add(" > Refresh List <");
        }

        internal void ApplyViewerStatus() {
            Top = ViewerStatus.YLoc;
            Left = ViewerStatus.XLoc;
            Width = ViewerStatus.Width;
            Height = ViewerStatus.Height;

            lvwProcessView.Columns[0].Width = ViewerStatus.ProcessViewIndexColumnWidth;
            lvwProcessView.Columns[1].Width = ViewerStatus.ProcessViewThreadColumnWidth;
            lvwProcessView.Columns[2].Width = ViewerStatus.ProcessViewLocationColumnWidth;

            lvwProcessView.Columns[3].Width = ViewerStatus.ProcessViewDebugMessageColumnWidth;

            ResizeProcessViewColumns();
        }

        // safsadf
        private void ResizeProcessViewColumns() {
            int theWidth = lvwProcessView.Width;
            theWidth -= lvwProcessView.Columns[0].Width + lvwProcessView.Columns[1].Width + lvwProcessView.Columns[2].Width;

            lvwProcessView.Columns[3].Width = theWidth;
        }

        private void mnuFileOpen_Click(object sender, System.EventArgs e) {
            using var ofd = new frmOpenDialog();
            ofd.InitialiseDialog(ViewerStatus.LastLoadedFile, ViewerStatus.FileMostRecentlyUsedList);
            if (ofd.ShowDialog() == DialogResult.OK) {
                var fim = ofd.GetFileImportMethod();
                bool asynch = ofd.GetAsynchOK();

                MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.ImportFileStarts, UserMessageType.InformationMessage, ofd.Filename);

                var jltf = new Job_LoadTraceFile(ofd.Filename, fim) {
                    AssignIdentToProcess = ofd.GetLabelIdent()
                };

                if (!asynch) {
                    MexCore.TheCore.WorkManager.ProcessJob(jltf);
                } else {
                    MexCore.TheCore.WorkManager.AddJob(jltf);
                }
                ViewerStatus.LastLoadedFile = ofd.Filename;

                if (ViewerStatus.FileMostRecentlyUsedList != null) {
                    foreach (string s in ViewerStatus.FileMostRecentlyUsedList) {
                        if (s == ofd.Filename) { return; }
                    }
                }
                // If we get here its a new MRU file.
                ViewerStatus.AddMostRecentlyUsedFile(ofd.Filename);
            }
            // End using OpenFileDialog ofd
        }

        private void ApplicationShutdown() {
            //Bilge.Log("Mex::UI >> Application Shutdown Called - Shutting down core");

            //Bilge.ResourceSysInfo("Application Shutdown, pre core finalise.");

            //Bilge.Log("Mex::UI >> Removing Notifications ....");

            Core.ViewManager.RegisterForProcessListChanges -= UpdateProcessListsAndMenuCallback;
            Core.ViewManager.RegisterForSelectedProcessChanges -= UpdateFollowingSelectedProcessChange;
            Core.ViewManager.RegisterForSelectedProcessRefreshRequired -= UpdateFollowingNewEventsArrivedForSelectedApp;
            Core.ViewManager.RegisterForCurrentViewChanges -= UpdateCurrentViewAfterRequest;

            //Bilge.Log("Mex::UI >> Stopping Listeners ....");

            Core.ViewManager.RequestChangeODSGathererState(false);
            Core.ViewManager.RequestChangeTCPGathererState(false);

            //Bilge.Log("Mex::UI >> Shutting Down Core ....");

            Core.ShutDownCore();
            //Bilge.ResourceSysInfo("Application Shutdown, post core finalise.");
            //Bilge.Log("Mex::UI >> Shutdown completed. Application closing");
        }

        #region Event handlers for changing process, menu and link label clicks

        private void RespondToSelectedChangeRequest(object sender, System.EventArgs e) {
            //Bilge.Log("Mex::UI >> USer clicked menu item to change process.");
            // TODO : Efficiency, check whether they have clicked the same one that was already selected - if so do nohting.
            int menuItemCount = 0;

            // check that i dont call this method invalidly in the future
            //Bilge.Assert(sender != null, " This method can not be called with a null sender as the tag of the calling object is required");

            int theDesiredIndex;
            if (sender is ToolStripMenuItem senderAsTsMi) {
                senderAsTsMi.Checked = true;
                theDesiredIndex = (int)senderAsTsMi.Tag;
            } else if (sender is LinkLabel senderAsLL) {
                senderAsLL.LinkVisited = true;
                theDesiredIndex = (int)senderAsLL.Tag;
            } else {
                //Bilge.Assert(false, "This method should not be called from anything which is not the menu or a linkLabel");
                throw new InvalidOperationException("This code should not be executed, the processes should be represented either by menu items or by link labels");
            }

            // Check that the code actually found the index.
            //Bilge.Assert(theDesiredIndex != -1, "The index for selecting the application should be set to a valid application when this method is called");

            // Update both the link lables and the process view to ensure that the correctly selected process is ticked / visted respectivly.
            while (mnuViewMenu.DropDownItems[menuItemCount] != mnuViewProcessListDivider) {
                var nextItem = (ToolStripMenuItem)mnuViewMenu.DropDownItems[menuItemCount];
                nextItem.Checked = theDesiredIndex == (int)nextItem.Tag;
                menuItemCount++;
            }

            foreach (var llnext in processLinkLabelCache) {
                llnext.LinkVisited = theDesiredIndex == (int)llnext.Tag;
            }

            // Check consistancy between the two parts of the UI
            //Bilge.Assert(processLinkLabelCache.Count == menuItemCount, "There was a mismatch between the link labels and the processes lsited in the menu");

            ChangeDisplayedProcess(theDesiredIndex);
        }

        #endregion Event handlers for changing process, menu and link label clicks

        private void UpdateMexViewFollowingProcessSelection(ProcessSummary newlySelectedProcess) {
            if (newlySelectedProcess == null) {
                // There was no selected process - probably following a purge all.
                AlterScreenEnableState(ScreenEnabledStates.InvalidProcessSelected);
                txtOverViewSelectedProcName.Text = string.Empty;
            } else {
                bool moreThanOne = Core.ViewManager.KnownProcessCount > 1;
                AlterScreenEnableState(ScreenEnabledStates.ValidProcessSelected);
                AlterScreenEnableState(newlySelectedProcess.hasMoreThanOnethread, newlySelectedProcess.hasResourceData, newlySelectedProcess.hasTimingData, newlySelectedProcess.hasTraceData, moreThanOne);
                txtOverViewSelectedProcName.Text = newlySelectedProcess.Machine + "\\" + newlySelectedProcess.WindowsPid + "    " + newlySelectedProcess.DisplayName;
                txtCurrentProcessLabel.Text = Core.ViewManager.SelectedTracedAppComment;

                // Grow the panel of additional informaiton here.
                lblStatusLine1.Text = newlySelectedProcess.StatusTitle;
                lblStatusText.Text = newlySelectedProcess.StatusText;

                if (MexCore.TheCore.Options.SelectingProcessSelectsProcessView) {
                    SelectProcessView();
                } else {
                    // The second time that the process window is selected is when there is one process and the AutoSelectFirstprocess is
                    // ticked.
                    if ((!moreThanOne) && MexCore.TheCore.Options.AutoSelectFirstProcess) {
                        SelectProcessView();
                    }
                }

                // if the selected screen was a process specific screen we need to cause a screen refresn
                if ((tbcMainView.SelectedTab == tabProcessThreadView) || (tbcMainView.SelectedTab == tabProcessTree)
                    || (tbcMainView.SelectedTab == tabProcessView) || (tbcMainView.SelectedTab == tabTimingsView)
                    || (tbcMainView.SelectedTab == tabResourceView)) {
                    btnRefresh_Click(null, null);
                }
            }
        }

        private void ChangeDisplayedProcess(int theDesiredIndex) {
            if (theDesiredIndex == Core.ViewManager.SelectedTracedAppIdx) {
                if (tbcMainView.SelectedTab != tabProcessView) {
                    tbcMainView.SelectedTab = tabProcessView;
                }
                return;
            }

            Core.ViewManager.SelectedTracedAppIdx = theDesiredIndex;
            // Now that there is a proces selected we have to enable some of the process specific buttons.
            var ps = Core.ViewManager.SelectedTracedAppSummary;
            UpdateMexViewFollowingProcessSelection(ps);
        }

        //Used as the parameter to alterscreenenablestate to tell the method what to do to the state of the screen
        private enum ScreenEnabledStates {
            ApplicationJustStarted,
            ValidProcessSelected,  // Seelcted index set to a valid application
            InvalidProcessSelected, // Seelcted index at -1 or an invalid application
            AbleToPurgeAndClearThis,
            NotAbleToPurgeAndClearThis,
            FindNotSupported,
            FindSupported,
            Unknown,
            AlertArrived
        }

        #region Button changing methods used by AlterScreenEnableState

        private void AlterScreenEnableState(ScreenEnabledStates newState) {
            switch (newState) {
                case ScreenEnabledStates.FindNotSupported: {
                        //Bilge.Log("Mexx::UI::AlterScreenEnableState >> Setting screen state to not support find");
                        mnuFindFind.Enabled = false;
                    }
                    break;

                case ScreenEnabledStates.FindSupported: {
                        //Bilge.Log("Mexx::UI::AlterScreenEnableState >> Setting screen state to not support find");
                        mnuFindFind.Enabled = true;
                    }
                    break;

                case ScreenEnabledStates.ApplicationJustStarted: {
                        //Bilge.Log("Mexx::UI::AlterScreenEnableState >> Setting screen state to initial state for the application");
                        txtSelectedEntryDetails.Text = string.Empty;
                        btnSemiPurgeCurrent.Enabled = false;
                        btnPurgeThis.Enabled = false;
                        btnSetTabProcess.Enabled = false;
                        btnSetTabThreads.Enabled = false;
                        btnSetTabResourceView.Enabled = false;
                        btnSetTabProcessTreeView.Enabled = false;
                        btnSetTabCrossProcessView.Enabled = false;
                        btnSetTabTimings.Enabled = false;
                        btnTransient.Enabled = false;
                        btnAlert.Enabled = true;
                        tbcMainView.SelectedTab = tabODSView;
                    }
                    break;

                case ScreenEnabledStates.ValidProcessSelected: {
                        //Bilge.Log("Mex::UI::AlterScreenEnableState >> Setting screen so you CAN select process specific operations");
                        txtSelectedEntryDetails.Text = string.Empty;
                        btnSemiPurgeCurrent.Enabled = true;
                        btnPurgeThis.Enabled = true;
                        btnSetTabProcess.Enabled = true;
                        btnSetTabThreads.Enabled = false;
                        btnSetTabResourceView.Enabled = false;
                        btnSetTabProcessTreeView.Enabled = false;
                        btnSetTabCrossProcessView.Enabled = false;
                        btnSetTabTimings.Enabled = false;
                        btnTransient.Enabled = true;
                        mnuPurgePurgeAllButThis.Enabled = true;
                        mnuPurgeClearThisProcess.Enabled = true;
                        mnuPurgePurgeThisProcess.Enabled = true;
                    }
                    break;

                case ScreenEnabledStates.InvalidProcessSelected: {
                        //Bilge.Log("Mex::UI::AlterScreenEnableState >> Setting screen so you Can NOT select process specific operations");
                        txtSelectedEntryDetails.Text = string.Empty;
                        btnSemiPurgeCurrent.Enabled = false;
                        btnPurgeThis.Enabled = false;
                        btnSetTabProcess.Enabled = false;
                        btnSetTabThreads.Enabled = false;
                        btnSetTabResourceView.Enabled = false;
                        btnSetTabProcessTreeView.Enabled = false;
                        btnSetTabCrossProcessView.Enabled = false;
                        btnSetTabTimings.Enabled = false;
                        btnTransient.Enabled = false;
                        mnuPurgePurgeAllButThis.Enabled = false;
                        mnuPurgeClearThisProcess.Enabled = false;
                        mnuPurgePurgeThisProcess.Enabled = false;
                    }
                    break;

                case ScreenEnabledStates.AbleToPurgeAndClearThis: {
                        btnSemiPurgeCurrent.Enabled = true;
                        btnPurgeThis.Enabled = true;
                        mnuPurgeClearThisProcess.Enabled = true;
                        mnuPurgePurgeThisProcess.Enabled = true;
                    }
                    break;

                case ScreenEnabledStates.NotAbleToPurgeAndClearThis: {
                        btnSemiPurgeCurrent.Enabled = false;
                        btnPurgeThis.Enabled = false;
                        mnuPurgeClearThisProcess.Enabled = false;
                        mnuPurgePurgeThisProcess.Enabled = false;
                    }
                    break;

                case ScreenEnabledStates.AlertArrived: {
                        btnAlert.Enabled = true;
                    }
                    break;

                default: {
                        //Bilge.Assert(false, "AlterScreenEnableState called with invalid parameter");
                    }
                    break;
            } // End Switch Statement
        }// End AlterScreenEnableState method

        private void AlterScreenEnableState(bool enableThreadView, bool enableResourceView, bool enableTimingView, bool enableTreeView, bool enableCrossProcessView) {
            btnSetTabCrossProcessView.Enabled = enableCrossProcessView;
            btnSetTabThreads.Enabled = enableThreadView;
            btnSetTabProcessTreeView.Enabled = enableTreeView;
            btnSetTabResourceView.Enabled = enableResourceView;
            btnSetTabTimings.Enabled = enableTimingView;
        }

        private void AlterScreenEnableState(bool enableThreadView, bool enableResourceView, bool enableTimingView, bool enableTreeView) {
            btnSetTabThreads.Enabled = enableThreadView;
            btnSetTabProcessTreeView.Enabled = enableTreeView;
            btnSetTabResourceView.Enabled = enableResourceView;
            btnSetTabTimings.Enabled = enableTimingView;
        }

        #endregion Button changing methods used by AlterScreenEnableState

        private void RefreshCurrentView(bool refreshMode) {
            long selectedIndex = 0;

            if (tbcMainView.SelectedTab == tabTimingsView) {
                Core.ViewManager.RefreshView_Timings(tvwTimingInstances, currentAdditionalData);
            }

            if (tbcMainView.SelectedTab == tabTimedView) {
                Core.ViewManager.RefreshView_Timed(lvwTimedView, refreshMode);
            }

            if (tbcMainView.SelectedTab == tabProcessView) {
                bool selfound = false;

                if (lvwProcessView.SelectedItems.Count == 1) {
                    selectedIndex = (long)lvwProcessView.SelectedItems[0].Tag;
                    selfound = true;
                }

                Core.ViewManager.RefreshView_Process(lvwProcessView, refreshMode, selectedIndex);

                if (selfound) {
                    lvwProcessView.Select();
                }
            }

            if (tbcMainView.SelectedTab == tabODSView) {
                Core.ViewManager.RefreshView_ODS(lvwMainODSView, refreshMode);
            }

            if (tbcMainView.SelectedTab == tabProcessThreadView) {
                // Convert the checked threads into a string list
                ExecuteThreadViewRefreshCommand();
            }

            if (tbcMainView.SelectedTab == tabSourceCodeView) {
                ExecuteSourceViewRefreshCommand();
            }

            if (tbcMainView.SelectedTab == tabProcessTree) {
                ExecuteTreeViewRefreshCommand();
            }

            if (tbcMainView.SelectedTab == tabResourceView) {
                Core.ViewManager.RefreshView_Resources(tvwNumericResources, refreshMode);
            }

            if (tbcMainView.SelectedTab == tabCrossProcessView) {
                ExecuteCrossProcessViewRefreshCommand();
            }

            if (tbcMainView.SelectedTab == tabTransient) {
                Core.ViewManager.RefreshView_Transient(txtTransientText, cboCustoms, lbxTransStatus, false);
            }

            if (tbcMainView.SelectedTab == tabAlerts) {
                Core.ViewManager.RefreshView_Alerting(lbgAlertEntries, lblMostRecentText, lblSelectedAlertText, refreshMode);
            }
        }

        private void RefreshCurrentView() {
            RefreshCurrentView(false);
        }

        private void btnRefresh_Click(object sender, System.EventArgs e) {
            RefreshCurrentView();
        }

        private void RefreshViewIncrementallyOnChange() {
            refreshNeeded = true;
            if (!tmr.Enabled) {
                tmr.Enabled = true;
            }
        }

        // This is used as a delegate to be invoked from the update events.
        private void PerformActualAutoRefresh() {
            refreshNeeded = false;

            if (tbcMainView.SelectedTab == tabProcessView) {
                Core.ViewManager.RefreshView_Process(lvwProcessView, true, -1);
                Core.ViewManager.RefreshStatusText(lblStatusLine1, lblStatusText);
            }

            if (tbcMainView.SelectedTab == tabProcessThreadView) {
                ExecuteThreadViewRefreshCommand();
            }

            if (tbcMainView.SelectedTab == tabODSView) {
                Core.ViewManager.RefreshView_ODS(lvwMainODSView, true);
            }

            if (tbcMainView.SelectedTab == tabTransient) {
                Core.ViewManager.RefreshView_Transient(txtTransientText, cboCustoms, lbxTransStatus, true);
            }
        }

        private void RefreshProcessListCache() {
            processLinkLabelCache.Clear();

            var pss = GetProcessSummaries();

            //Bilge.Log("Mex::UI >> UpdateProcessListsAndMenuCallback, callback rebuilding cache of LinkLbls");

            for (int i = 0; i < pss.Length; i++) {
                //Bilge.Assert(pss[i].DisplayName != null, "Mex::UI >> ERROR >> ps.DisplayName should be valid or empty by the time we try adn write it to the screen");

                #region create the link label to be added into the quick list

                // TODO : Dont recreate these each time, its not a big overhead but it is a smidge
                var ll = new System.Windows.Forms.LinkLabel {
                    AutoSize = true,
                    Name = "llquickProc" + i.ToString(),
                    TabStop = false,
                    AutoEllipsis = true
                };
                string llName = pss[i].PreferredRenderName;
                ll.Text = "[ " + llName + " ]";

                // The link label cache is used to hold references to all of the link labels so that we can set the correct
                // one to visited when it is selected.
                processLinkLabelCache.Add(ll);

                ll.Click += new EventHandler(RespondToSelectedChangeRequest);

                #endregion create the link label to be added into the quick list

                string menuItemText;
                menuItemText = pss[i].Machine + "\\" + pss[i].PreferredRenderName + " (" + pss[i].WindowsPid + ")";
                var newProcessMenuItem = new ToolStripMenuItem(menuItemText, null, new EventHandler(RespondToSelectedChangeRequest)) {
                    Tag = pss[i].InternalIndex
                };
                ll.Tag = pss[i].InternalIndex;
                // Now check if it was the selected one.
                if (Core.ViewManager.IsCurrentSelectedAppValid) {
                    if (pss[i].WindowsPid == Core.ViewManager.SelectedTracedAppPid) {
                        ll.LinkVisited = true;
                        newProcessMenuItem.Checked = true;
                    }
                }

                mnuViewMenu.DropDownItems.Insert(0, newProcessMenuItem);
            }
        }

        private bool doingWork;

        private void RedrawProcessLists(/*ProcessSummary[] pss*/) {
            if (doingWork) {
                throw new InvalidOperationException("Reentry not supported, this should not occur.");
            }
            doingWork = true;

            #region constants used to position controls on the screen

            const int LL_TOP = 4;
            const int LL_SPACE = 4;
            const int LL_INITIAL_LEFTPOSITION = 20;
            const int OVERHEAD_EACHEND_DUE_TO_SCROLLERS = 35;

            #endregion constants used to position controls on the screen

            RefreshProcessListCache();
            RemoveLinkLabelsFromPanel();
            llbScrollRight.Enabled = false;

            // processLinkLabelLastOffset = -1;

            if (processLinkLabelCache.Count == 0) {
                doingWork = false;
                return;
            }

            // The autosize only works correctly when the control is added to a container therefore use a temporary panel to
            // force it to update the size so I can determine whether it fits on the screen or not.

            using (var tempWorkingPanel = new Panel()) {
                int totalAvailablePanelWidth = pnlQuickProcessLinkHolder.Width - (OVERHEAD_EACHEND_DUE_TO_SCROLLERS * 2);
                int nextLLXCoordinate = LL_INITIAL_LEFTPOSITION;

                for (int llCount = processLinkLabelFirstOffset; llCount < processLinkLabelCache.Count; llCount++) {
                    tempWorkingPanel.Controls.Add(processLinkLabelCache[llCount]);

                    int nextWidth = processLinkLabelCache[llCount].Width;
                    if (nextLLXCoordinate + nextWidth > totalAvailablePanelWidth) {
                        // More processes we are unable to fit on the panel.
                        //this.processLinkLabelLastOffset = llCount - 1;
                        llbScrollRight.Enabled = true;
                        break;
                    }

                    processLinkLabelCache[llCount].Location = new System.Drawing.Point(nextLLXCoordinate, LL_TOP);
                    pnlQuickProcessLinkHolder.Controls.Add(processLinkLabelCache[llCount]);
                    nextLLXCoordinate += processLinkLabelCache[llCount].Width + LL_SPACE;
                    tempWorkingPanel.Controls.Clear();
                }
            }

            if (!Core.ViewManager.IsCurrentSelectedAppValid) {
                //Bilge.Log("Mex::Ui >> Defaulting the process as we dont have one set at the moment");
                if (mnuViewMenu.DropDownItems[0].Tag != null) {
                    RespondToSelectedChangeRequest(mnuViewMenu.DropDownItems[0], null);
                } else {
                    //Bilge.Assert(mnuViewMenu.DropDownItems[0].Text == "-", "Asserting that the menu list is empty when there is an invalid virtual index in the tag of a selected menu item");
                }
            }

            // TODO : Restore selected one, OR select one if there was no currently selected one
            //Bilge.Log("Mex::UI >> UpdateProcessListsAndMenuCallback, callback COMPLETED");
            doingWork = false;
        }

        private void RemoveLinkLabelsFromPanel() {
            // strip all of the quick labels out, ignoring the two navigation ones
            for (int i = 0; i < pnlQuickProcessLinkHolder.Controls.Count;) {
                var c = pnlQuickProcessLinkHolder.Controls[i];
                if (!c.Name.StartsWith("llbScroll")) {
                    pnlQuickProcessLinkHolder.Controls.Remove(c);
                } else {
                    i++;
                }
            }
        }

        private ProcessSummary[] GetProcessSummaries() {
            ProcessSummary[] result;
#if DEBUG
#endif

            while (mnuViewMenu.DropDownItems[0] is not ToolStripSeparator) {
                mnuViewMenu.DropDownItems.RemoveAt(0);
#if DEBUG
                //Bilge.Assert(debugLoopProtect-- >= 0, "Mex::UI::UpdateProcessListsAndMenuCallback -> Callback trying to update process list in view menu and could not find end of list.  Infinite loop resulted of not finding divider");
#endif
            }

            result = Core.DataManager.GetAllProcessSummaries();
            return result;
        }

        private void UpdateProcessListsAndMenuCallback(ProcessSummary[] pss) {
            //Bilge.Log("Mex::UI >> Callback fired - UpdateProcessListsAndMenuCallback, in response to new application found");

            // TODO: Only do this if the number have actually changed
            //Bilge.Warning("This is innefficient, the datamanager goes to the trouble of telling us just those that have changed, we should use that");

            // TODO : We pass null so that it is forced to do a complete refresh, however i suspect that we should be more efficient about this

            if (InvokeRequired) {
                // TODO : Parameterise this
                _ = Invoke(new voidMethodInvoker(RedrawProcessLists));
            } else {
                RedrawProcessLists(/*pss*/);
            }
        }  // End UpdateProcessListsAndMenuCallback

        #region Delegates used for invokves

        private delegate void voidMethodInvoker();

        private delegate void boolMeth(bool dummy);

        private delegate void stringArrayMethodInvoker(string[] dummy);

        private delegate void procSummaryArrayMethodInvoker(ProcessSummary[] dummy);

        private delegate void procSummaryMethodInvoker(ProcessSummary dummy);

        #endregion Delegates used for invokves

        private void RealUpdateFollowingSelectedProcessChange(ProcessSummary ps) {
            UpdateMexViewFollowingProcessSelection(ps);
        }

        /// <summary>
        /// Method to respond to the fact that the selected process has changed.  If the parameter is null then the selected process is no longer valid
        /// and there is no replacement selected process (eg on purge all).  If the parameter is not null then it contains the details of the new selected
        /// process.  NB this method is called when the name of the currently selected process changes as well as when the selection changes, or when its auto
        /// purged.
        /// </summary>
        /// <param name="ps">The process summary of the new selected process, or null if there is no new process</param>
        private void UpdateFollowingSelectedProcessChange(ProcessSummary ps) {
            //Bilge.Log("UI Recieved update notification of selected process changed ", ps.DisplayName);
            if (InvokeRequired) {
                //Bilge.FurtherInfo("Switching back to the UI thread to perform update following selected process change");
                _ = Invoke(new procSummaryMethodInvoker(RealUpdateFollowingSelectedProcessChange), ps);
            } else {
                RealUpdateFollowingSelectedProcessChange(ps);
            }
        } // end UpdateFollowingSelectedProcessChange method

        private bool timerTriggeredRefreshRequired; // Default false
        private bool triggerRefreshisIncremental; // Default false

        private void UpdateCurrentViewAfterRequest(bool incrementalOK) {
            //Bilge.Log("UI recieved update notification requesting a refresh of the current process", "Incremental value " + incrementalOK.ToString());

            var elapsed = DateTime.Now - m_lastRefreshNotfication;
            if (elapsed.TotalSeconds < MexCore.TheCore.Options.NoSecondsForUIUpdate) {
                //Bilge.VerboseLog("Refresh being ignored because came too frequently, queueing up a refresh request for later");
                // If the messages are coming too frequently we dont refresh, however we put a refresh job on the queue because if this is the
                // last message then we wont get another refresh request and the output will be innaccurate.
                timerTriggeredRefreshRequired = true;
                triggerRefreshisIncremental = incrementalOK;
            } else {
                timerTriggeredRefreshRequired = false;
                m_lastRefreshNotfication = DateTime.Now;

                if (InvokeRequired) {
                    //Bilge.FurtherInfo("Switching back to UI thread to perform refresh");
                    _ = Invoke(new boolMeth(RefreshCurrentView), incrementalOK);
                } else {
                    RefreshCurrentView();
                }
            }
        }

        private void UpdateFollowingNewEventsArrivedForNonTracedApp(bool incrementalOk) {
            if (incrementalOk) {
                //Bilge.Warning("No incremental support on non traced apps");
            }

            if (Core.Options.AutoRefresh) {
                if (InvokeRequired) {
                    _ = Invoke(new voidMethodInvoker(RefreshViewIncrementallyOnChange));
                } else {
                    //Bilge.Warning("Double check why this is being called on the main form thread is that by design??");
                    RefreshViewIncrementallyOnChange();
                }
            }
        }

        private void PositionDialogOverView(Form dialog, bool full) {
            if (full) {
                dialog.Left = Left + tbcMainView.Left + 2;
                dialog.Top = Top + tbcMainView.Top + 2;
                dialog.Width = tbcMainView.Width;
                dialog.Height = tbcMainView.Height;
            } else {
                dialog.Left = Left + 2;
                dialog.Top = Top + 35;
            }
        }

        private void UpdateFollowingOptionsChangeNotification(string[] ListOfFilters) {
            if (InvokeRequired) {
                _ = Invoke(new stringArrayMethodInvoker(RefreshScreenFromOptionsView), ListOfFilters);
            } else {
                //Bilge.Warning("How come this is being called on the UI thread, check how this came about");
                RefreshScreenFromOptionsView(ListOfFilters);
            }
        }

        private DateTime m_lastRefreshNotfication = DateTime.MinValue;

        private void UpdateFollowingNewEventsArrivedForSelectedApp(bool incrementalOk) {
            // a new event has arrived.
            if (Core.Options.AutoRefresh) {
                // If this is the case then we need to refresh the current view when things change, however we dont want to do this too frequently
                // as it causes flicker.  Therefore we check if its appropriate before doing a refresh.
                var elapsed = DateTime.Now - m_lastRefreshNotfication;
                if (elapsed.TotalSeconds < MexCore.TheCore.Options.NoSecondsForUIUpdate) {
                    //Bilge.VerboseLog("Refresh being ignored because came too frequently, queueing up a refresh request for later");
                    // If the messages are coming too frequently we dont refresh, however we put a refresh job on the queue because if this is the
                    // last message then we wont get another refresh request and the output will be innaccurate.
                    MexCore.TheCore.ViewManager.RequestViewRefreshNotification(incrementalOk);
                } else {
                    m_lastRefreshNotfication = DateTime.Now;

                    // The ViewSupportManager might be running on a different thread.  Therefore time to backinvoke to me.
                    if (InvokeRequired) {
                        _ = Invoke(new voidMethodInvoker(RefreshViewIncrementallyOnChange));
                    } else {
                        //Bilge.Warning("Double check why this is being called on the main form thread is that by design??", "CallStack:\n" + InternalUtil.StackToString());
                        RefreshViewIncrementallyOnChange();
                    }
                }
            }
        }

        private void RefreshScreenFromOptionsView(string[] ListOfFilters) {
            cboMainViewQuickFilterChange.Items.Clear();
            cboMainViewQuickFilterChange.Items.AddRange(ListOfFilters);
            cboMainViewQuickFilterChange.Text = " > Quick Load Filter <";
        }

        #region Form Events

        private void Form1_Closed(object sender, System.EventArgs e) {
        }

        private void Form1_Load(object sender, System.EventArgs e) {
            //Bilge.E("Mex::UI >> Form_Load");
            try {
                AlterScreenEnableState(ScreenEnabledStates.ApplicationJustStarted); // Set initial state of the applications UI

                // Register callbacks into the model that we are interested in as the main view.
                Core.ViewManager.RegisterForProcessListChanges += new ViewSupportManager.ProcessListChanged(UpdateProcessListsAndMenuCallback);
                Core.ViewManager.RegisterForSelectedProcessChanges += new ViewSupportManager.SelectedProcessChanged(UpdateFollowingSelectedProcessChange);
                Core.ViewManager.RegisterForSelectedProcessRefreshRequired += new ViewSupportManager.SelectedProcessRefreshRequired(UpdateFollowingNewEventsArrivedForSelectedApp);
                Core.ViewManager.RegisterForCurrentViewChanges += new ViewSupportManager.CurrentViewChanged(UpdateCurrentViewAfterRequest);

                // Start capturing ODS and TCP
                //Bilge.Log("Requesting TCP and ODS Capture is started");
                mnuCaptureToggleOds_Click(null, null);
                mnuCaptureToggleTCP_Click(null, null);
            } finally {
                //Bilge.X();
            }
        }

        #endregion Form Events

        private void btnPurgeCurrent_Click(object sender, System.EventArgs e) {
            RequestPurgeSelectedProcess();
        }

        private void RequestPurgeSelectedProcess() {
            //Bilge.Log("User requests purge of currently selected process");
            AlterScreenEnableState(ScreenEnabledStates.InvalidProcessSelected);
            Core.ViewManager.RequestSelectedAppPurge();
            SetMainViewFollowingPurgeOne();
        }

        private void RequestClearSelectedProcess() {
            //Bilge.Log("User requests purge of currently selected process");
            Core.MessageManager.PurgeAllData();
            Core.ViewManager.RequestSelectedAppClear();
            lvwProcessView.Items.Clear();
        }

        private void lvwProcessView_SelectedIndexChanged(object sender, System.EventArgs e) {
            if (lvwProcessView.SelectedItems.Count == 0) {
                //Bilge.Log("WARNING!!!! --> SelectedIndexChanged when there were no selected items on process view.  More info cant display properly");
                txtSelectedEntryDetails.Text = string.Empty;
                return;
            }

            // They are interacting with the process view and selecting different entries
            // we need to identify the one that they selected and present sensible information in the bottom half of the screen.
            var lvi = lvwProcessView.SelectedItems[0]; // we dont allow multi select therefore this should only be one
                                                       //Bilge.Assert(lvi != null, "This should not happen, selected index changed yet there was nothing selected but the time that we looked");
                                                       //Bilge.Assert(lvi.Tag != null, "The tag should refer to the global index of the list view item, if its null its been incorrectly inserted into the list view.");

            long idx = (long)lvi.Tag;
            currentFindIncrementalIndexPreference = idx;
            string s = Core.ViewManager.GetMoreInfoForEventIndexInSelectedApp(idx);
            txtSelectedEntryDetails.Text = s;
        }

        #region Button Event Handlers for changing the view and View Change Methods

        private void SetToolbarButtonHighlight(Button b) {
            btnSetTabMain.BackColor = Color.FromKnownColor(KnownColor.WhiteSmoke);
            btnSetTabProcess.BackColor = Color.FromKnownColor(KnownColor.WhiteSmoke);
            btnSetTabThreads.BackColor = Color.FromKnownColor(KnownColor.WhiteSmoke);
            btnSetTabResourceView.BackColor = Color.FromKnownColor(KnownColor.WhiteSmoke);
            btnSetTabProcessTreeView.BackColor = Color.FromKnownColor(KnownColor.WhiteSmoke);
            btnSetTabCrossProcessView.BackColor = Color.FromKnownColor(KnownColor.WhiteSmoke);
            btnSetTabTimings.BackColor = Color.FromKnownColor(KnownColor.WhiteSmoke);
            btnTransient.BackColor = Color.FromKnownColor(KnownColor.WhiteSmoke);

            if (b != null) {
                b.BackColor = Color.FromKnownColor(KnownColor.LightCoral);
            }
        }

        private void SelectTreeView() {
            AlterScreenEnableState(ScreenEnabledStates.FindNotSupported);

            if ((!Core.ViewManager.CurrentFilter.TraceMessageTypeIncludedByFilter(TraceCommandTypes.TraceMessageIn)) || (!Core.ViewManager.CurrentFilter.TraceMessageTypeIncludedByFilter(TraceCommandTypes.TraceMessageIn))) {
                if (MessageBox.Show("Your current filter settings remove TraceIn/Out messages.  This will make the tree view pointless, reset this?", "Filter Query", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                    Core.ViewManager.CurrentFilter.BeginFilterUpdate();

                    Core.ViewManager.CurrentFilter.SetMessageTypeIncludeByType(TraceCommandTypes.TraceMessageIn, true);
                    Core.ViewManager.CurrentFilter.SetMessageTypeIncludeByType(TraceCommandTypes.TraceMessageOut, true);

                    Core.ViewManager.CurrentFilter.EndFilterUpdate();
                }
            }

            cboAppThreads.Items.Clear();
            cboAppThreads.Items.AddRange(Core.ViewManager.GetThreadNameListForSelectedApp().ToArray());
            cboAppThreads.SelectedIndex = 0;

            var threadKey = (KeyDisplayRepresentation)cboAppThreads.SelectedItem;
            Core.ViewManager.RefreshView_ProcessTree(tvwProcTraceTree, threadKey);
            SetToolbarButtonHighlight(btnSetTabProcessTreeView);
            tbcMainView.SelectedTab = tabProcessTree;
        }

        private void SelectProcessView() {
            SelectProcessView(-1);
        }

        private void SelectTransientView() {
            AlterScreenEnableState(ScreenEnabledStates.NotAbleToPurgeAndClearThis);
            Core.ViewManager.RefreshView_Transient(txtTransientText, cboCustoms, lbxTransStatus, false);
            SetToolbarButtonHighlight(btnTransient);
            tbcMainView.SelectedTab = tabTransient;
        }

        private void SelectAlertView() {
            AlterScreenEnableState(ScreenEnabledStates.NotAbleToPurgeAndClearThis);
            Core.ViewManager.RefreshView_Alerting(lbgAlertEntries, lblMostRecentText, lblSelectedAlertText, false);
            SetToolbarButtonHighlight(btnAlert);
            tbcMainView.SelectedTab = tabAlerts;
        }

        private void SelectProcessView(long andForceIndexToThis) {
            AlterScreenEnableState(ScreenEnabledStates.AbleToPurgeAndClearThis);
            AlterScreenEnableState(ScreenEnabledStates.FindSupported);
            Core.ViewManager.RefreshView_Process(lvwProcessView, false, andForceIndexToThis);
            SetToolbarButtonHighlight(btnSetTabProcess);
            tbcMainView.SelectedTab = tabProcessView;
            if (andForceIndexToThis >= 0) {
                lvwProcessView.Select();
            }
        }

        private void SelectTimedView() {
            AlterScreenEnableState(ScreenEnabledStates.NotAbleToPurgeAndClearThis);
            AlterScreenEnableState(ScreenEnabledStates.FindNotSupported);
            Core.ViewManager.RefreshView_Timed(lvwTimedView, false);
            SetToolbarButtonHighlight(null);
            tbcMainView.SelectedTab = tabTimedView;
        }

        private void SelectMainView(bool incremental) {
            AlterScreenEnableState(ScreenEnabledStates.FindSupported);
            Core.ViewManager.RefreshView_ODS(lvwMainODSView, incremental);
            SetToolbarButtonHighlight(btnSetTabMain);
            tbcMainView.SelectedTab = tabODSView;
        }

        private void SetMainViewFollowingPurgeAll() {
            AlterScreenEnableState(ScreenEnabledStates.InvalidProcessSelected);
            AlterScreenEnableState(ScreenEnabledStates.FindSupported);
            SetToolbarButtonHighlight(btnSetTabMain);
            lvwMainODSView.Items.Clear();  // frig.
            tbcMainView.SelectedTab = tabODSView;
        }

        private void SetMainViewFollowingPurgeOne() {
            AlterScreenEnableState(ScreenEnabledStates.InvalidProcessSelected);
            AlterScreenEnableState(ScreenEnabledStates.FindSupported);
            SetToolbarButtonHighlight(btnSetTabMain);
            tbcMainView.SelectedTab = tabODSView;
        }

        private void SelectResourceView() {
            AlterScreenEnableState(ScreenEnabledStates.AbleToPurgeAndClearThis);
            AlterScreenEnableState(ScreenEnabledStates.FindNotSupported);
            Core.ViewManager.RefreshView_Resources(tvwNumericResources, false);
            SetToolbarButtonHighlight(btnSetTabResourceView);
            tbcMainView.SelectedTab = tabResourceView;
        }

        private void SelectTimingsView() {
            AlterScreenEnableState(ScreenEnabledStates.NotAbleToPurgeAndClearThis);
            AlterScreenEnableState(ScreenEnabledStates.FindNotSupported);
            Core.ViewManager.RefreshView_Timings(tvwTimingInstances, currentAdditionalData);
            SetToolbarButtonHighlight(btnSetTabTimings);
            tbcMainView.SelectedTab = tabTimingsView;
        }

        private void SelectThreadView() {
            AlterScreenEnableState(ScreenEnabledStates.AbleToPurgeAndClearThis);
            AlterScreenEnableState(ScreenEnabledStates.FindNotSupported);
            ExecuteThreadViewRefreshCommand();
            SetToolbarButtonHighlight(btnSetTabThreads);
            tbcMainView.SelectedTab = tabProcessThreadView;
        }

        private void SelectCrossProcessView() {
            AlterScreenEnableState(ScreenEnabledStates.NotAbleToPurgeAndClearThis);
            AlterScreenEnableState(ScreenEnabledStates.FindNotSupported);
            ExecuteCrossProcessViewRefreshCommand();
            SetToolbarButtonHighlight(btnSetTabCrossProcessView);
            tbcMainView.SelectedTab = tabCrossProcessView;
        }

        private void btnSetTabMain_Click(object sender, System.EventArgs e) {
            AlterScreenEnableState(ScreenEnabledStates.AbleToPurgeAndClearThis);
            SelectMainView(true);
        }

        private void btnSetTabProcess_Click(object sender, System.EventArgs e) {
            //Bilge.Assert(MexCore.TheCore.ViewManager.IsCurrentSelectedAppValid, "The currently selected application is not valid, this will cause problems in a moment.");
            SelectProcessView();
        }

        private void btnSetTabTimings_Click(object sender, System.EventArgs e) {
            SelectTimingsView();
        }

        private void btnResources_Click(object sender, System.EventArgs e) {
            SelectResourceView();
        }

        private void btnSetTabThreads_Click(object sender, System.EventArgs e) {
            SelectThreadView();
        }

        #endregion Button Event Handlers for changing the view and View Change Methods

        private void ExecuteTreeViewRefreshCommand() {
            // Check whether the list of threads is right
            var threadIds = Core.ViewManager.GetThreadNameListForSelectedApp();
            var selectedThread = (KeyDisplayRepresentation)cboAppThreads.SelectedItem;

            cboAppThreads.Items.Clear();
            cboAppThreads.Items.AddRange(threadIds.ToArray());
            if (selectedThread != null) {
                for (int i = 0; i < cboAppThreads.Items.Count; i++) {
                    if (selectedThread.KeyIdentity == ((KeyDisplayRepresentation)cboAppThreads.Items[i]).KeyIdentity) {
                        cboAppThreads.SelectedIndex = i;
                    }
                }
            } else {
                cboAppThreads.SelectedIndex = 0;
            }

            Core.ViewManager.RefreshView_ProcessTree(tvwProcTraceTree, (KeyDisplayRepresentation)cboAppThreads.SelectedItem);
        }

        private void ExecuteCrossProcessViewRefreshCommand() {
            // First find all of the internal indexes of those ones that are already included in the view.
            var checkedIdxs = new List<int>();
            foreach (ProcessSummary cps in clbAllKnownProcesses.CheckedItems) {
                checkedIdxs.Add(cps.InternalIndex);
            }
            clbAllKnownProcesses.Items.Clear();

            var pss = Core.DataManager.GetAllProcessSummaries();

            // Had to put this in as I cant figure out how to check an item in a clb once its been added.  GRR.
            int noToBeChecked = 0;
            foreach (var aps in pss) {
                if (checkedIdxs.Contains(aps.InternalIndex)) {
                    noToBeChecked++;
                    break;
                }
            }
            if (noToBeChecked == 0) {
                // Make sure at least one is ticked
                checkedIdxs.Add(pss[0].InternalIndex);
            }

            // Now add the known processes to the view
            foreach (var ps in pss) {
                _ = clbAllKnownProcesses.Items.Add(ps, checkedIdxs.Contains(ps.InternalIndex));
            }

            //Bilge.Assert(clbAllKnownProcesses.Items.Count > 0, "This screen should not be displayable if there are no currently loaded traced apps");

            RefreshCrossProcessViewItself();
        }

        private void RefreshCrossProcessViewItself() {
            long selectedIndex = 0;

            bool selfound = false;

            if (lvwCrossProcesList.SelectedItems.Count == 1) {
                selectedIndex = (long)lvwCrossProcesList.SelectedItems[0].Tag;
                selfound = true;
            }

            int[] indexesToRefresh = new int[clbAllKnownProcesses.CheckedItems.Count];
            for (int i = 0; i < clbAllKnownProcesses.CheckedItems.Count; i++) {
                indexesToRefresh[i] = ((ProcessSummary)clbAllKnownProcesses.CheckedItems[i]).InternalIndex;
            }

            Core.ViewManager.RefreshView_CrossProcess(lvwCrossProcesList, indexesToRefresh, false, selectedIndex);

            if (selfound) {
                lvwCrossProcesList.Select();
                if (lvwCrossProcesList.SelectedItems.Count != 1) {
                    //Bilge.Warning("Lost selection on the refresh of the cross process view.");
                } else {
                    //Bilge.Assert((long)lvwCrossProcesList.SelectedItems[0].Tag == selectedIndex, "index mismatch on selection");
                }
            }
        }

        private void ExecuteSourceViewRefreshCommand() {
            var threadIds = Core.ViewManager.GetThreadNameListForSelectedApp();
            string lastthread = cboSelectedThread.Text;

            cboSelectedThread.Items.Clear();
            cboSelectedThread.Items.AddRange(threadIds.ToArray());
            cboSelectedThread.SelectedIndex = cboSelectedThread.Items.IndexOf(lastthread) >= 0 ? cboSelectedThread.Items.IndexOf(lastthread) : 0;

            Core.ViewManager.RefreshView_SourceSynch(lvwSourceView, cboSelectedThread.Text);
        }

        private void ExecuteThreadViewRefreshCommand() {
            // Firstly we grab all of the known threads that are in this application, checking them to see
            var threadIds = Core.ViewManager.GetThreadNameListForSelectedApp();

            if (threadIds.Count == 0) {  // There werent any threads to select at all.
                                         // we cant select anything.
                                         //Bilge.Log("WARNING >>Failed to execute a thread view referesh. There were no threads checked therefore the update could not proceeed");
                Core.ViewManager.AddUserNotificationMessageByIndex(UserMessages.NoThreadsSelectedForView, UserMessageType.WarningMessage, null);
                return;
            }

            // Next thing to do is get a separate list of all those thread names that are ticked in the combo box
            // so that we can work out which threads the user wants to display.
            List<KeyDisplayRepresentation> selectedThreads;

            if (clbThreadList.CheckedItems.Count == 0) {
                // Make sure that theres always one thread ticked, otherwise the first run trhough always displays nothing.
                selectedThreads = new List<KeyDisplayRepresentation> {
                    threadIds[0]
                };
            } else {
                // If there are some checked items then we grab these to display the threads properly
                selectedThreads = new List<KeyDisplayRepresentation>();
                foreach (KeyDisplayRepresentation kdr in clbThreadList.CheckedItems) {
                    selectedThreads.Add(kdr);
                }
            }

            // Once we have done this and have grabbed all the ticked threads were safe to repopulate the combo box
            // including any newly discovered thread names, making sure that we tick any threads that were ticked
            // before.
            clbThreadList.Items.Clear();
            foreach (var nextThread in threadIds) {
                // Stick each of the matched threads into the clb, checking it if it was checked before.
                bool nextThreadIsSelected = selectedThreads.Contains(nextThread);
                _ = clbThreadList.Items.Add(nextThread, nextThreadIsSelected);
            }

            // Now we reget the selected threads as there is a possibility that the threads that have been added do not
            // overlap with those that were ticked  - if for example a purge has occured since the last refresh.
            selectedThreads.Clear();
            foreach (KeyDisplayRepresentation kdr in clbThreadList.CheckedItems) {
                selectedThreads.Add(kdr);
            }

            //Bilge.Assert(selectedThreads.Count > 0, "No thred was selected for display.  This shouldnt be possible as we should default to the first thread or return out of the refresh.");
            Core.ViewManager.RefreshView_ProcessThread(lvwThreadView, selectedThreads, chkSynchThreads.Checked);
        }

        private void vscrAllThreadBoxes_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e) {
            try {
                int selectDelta = 0;

                switch (e.Type) {
                    case ScrollEventType.LargeDecrement: selectDelta = -10; break;
                    case ScrollEventType.LargeIncrement: selectDelta = 10; break;
                    case ScrollEventType.SmallDecrement: selectDelta = -1; break;
                    case ScrollEventType.SmallIncrement: selectDelta = 1; break;
                }
                foreach (ListBox lbx in pnlThreadArea.Controls) {
                    lbx.SelectedIndex += selectDelta;
                }
            } catch (ArgumentOutOfRangeException) {
                //Bilge.Dump(aoex, "Argument out of range while using master scroll bar- not really surprising");
            }
        }

        private void btnSetFilter_Click(object sender, System.EventArgs e) {
            //Bilge.EnterSection("Setting Filter Options");
            using var fpvf = new frmProcViewFilter();
            if (Core.ViewManager.SelectedTracedAppIdx != -1) {
                //Bilge.Log("Initialising filter dialog settings from the currently selected application");
                fpvf.InitialiseForTracedApplication();
            }

            if (Core.ViewManager.CurrentFilter != null) {
                //Bilge.Log("Initialising filter dialog settings from current filter.");
                fpvf.InitialiseFromExistingFilter(Core.ViewManager.CurrentFilter);
            }

            if (fpvf.ShowDialog() == DialogResult.OK) {
                Core.ViewManager.CurrentFilter = fpvf.GetFilterFromDialog();
                if (fpvf.FullRefreshRequired) {
                    //Bilge.Log("Filter has caused a full refresh of the current view.");
                    RefreshCurrentView(false);
                }
            }
            //Bilge.LeaveSection();
        }

        private void mnuCaptureToggleOds_Click(object sender, System.EventArgs e) {
            mnuCaptureToggleOds.Checked = !mnuCaptureToggleOds.Checked;
            Core.ViewManager.RequestChangeODSGathererState(mnuCaptureToggleOds.Checked);
        }

        private void mnuCaptureToggleTCP_Click(object sender, EventArgs e) {
            mnuCaptureToggleTCP.Checked = !mnuCaptureToggleTCP.Checked;
            Core.ViewManager.RequestChangeTCPGathererState(mnuCaptureToggleTCP.Checked);
        }

        private void mnuFindFind_Click(object sender, System.EventArgs e) {
            using var ffd = new frmFindDialog();
            PositionDialogOverView(ffd, false);

            if (ffd.ShowDialog() == DialogResult.OK) {
                // they do want us to do a find.
                string theSearchString = ffd.GetFindMatchText();
                bool useCase = false;

                // Decide how they want the string to find to be used
                switch (ffd.GetUsageType()) {
                    case frmFindDialog.FindMatchUsageType.TextMatchCaseSensitive: {
                            useCase = true;
                        }
                        break;

                    case frmFindDialog.FindMatchUsageType.TextMatchNoCase: {
                            useCase = false;
                        }
                        break;

                    default: {
                            throw new NotImplementedException();
                        }
                }

                // now decide where they want to look for thje string
                if (ffd.GetLocationType() == frmFindDialog.FindMatchLocationType.CurrentPhysicalView) {
                    _ = tbcMainView.SelectedTab == tabProcessView
                        ? UIHelperRoutines.FindSelectAndShow(lvwProcessView, theSearchString, useCase)
                        : tbcMainView.SelectedTab == tabODSView
                            ? UIHelperRoutines.FindSelectAndShow(lvwMainODSView, theSearchString, useCase)
                            : throw new NotImplementedException();
                }
            }
        }

        private void btnSetHighlight_Click(object sender, System.EventArgs e) {
            Core.ViewManager.UpdateHighlight();
        }

        private void btnSemiPurgeCurrent_Click(object sender, System.EventArgs e) {
            if (tbcMainView.SelectedTab == tabODSView) {
                Core.DataManager.PurgeUnknownApplications();
                RefreshCurrentView();
                return;
            }

            if (tbcMainView.SelectedTab == tabCrossProcessView) {
                Core.ViewManager.AddUserNotificationMessageByIndex(UserMessages.GeneralUIError, UserMessageType.WarningMessage, "Unable to clear a process from cross process view.");
                return;
            }

            RequestClearSelectedProcess();
        }

        private void lvwProcessView_DoubleClick(object sender, System.EventArgs e) {
            var chosenListView = (ListView)sender;

            if (chosenListView.SelectedItems.Count == 0) {
                // tODO ://Bilge.Error("ERROR Occured when trying to display more details.", "DoubleClick called when no element selected on the process view.  The more details cant be displayed correctly.");
                return;
            }

            var lvi = chosenListView.SelectedItems[0]; // we dont allow multi select therefore this should only be one
                                                       //Bilge.Assert(lvi != null, "This should not happen, selected index changed yet there was nothing selected but the time that we looked");
                                                       //Bilge.Assert(lvi.Tag != null, "The tag should refer to the global index of the item, this entry was not inserted correctly into the grid");

            long idx = (long)lvi.Tag;

            bool FilterChangedDueToDialog = false;

            var msgType = Core.ViewManager.DetermineMessageTypeForView(ref idx);
            switch (msgType) {
                case ViewSupportManager.ExtendedDetailsMode.Exception:
                    using (var fed = new frmExceptionDetails()) {
                        PositionDialogOverView(fed, true);

                        Core.ViewManager.PopulateExceptionDetailsScreen(fed.lbxExceptionHeirachy, fed.txtExceptionSummary, fed.txtExceptionLocationFilename, fed.txtExceptionLocationLineNo, idx);
                        fed.InitialiseOnceDatasIn(idx);
                        FilterChangedDueToDialog = fed.ShowDialog() == DialogResult.Yes;
                    }
                    break;

                case ViewSupportManager.ExtendedDetailsMode.LogMessage:
                    FilterChangedDueToDialog = DisplayLogMessageExtendedDetails(idx, FilterChangedDueToDialog);
                    break;

                case ViewSupportManager.ExtendedDetailsMode.Error:
                    // TODO : Implement this properly.
                    DisplayErrorMessageExtendedDetails(idx);
                    break;

                case ViewSupportManager.ExtendedDetailsMode.AssertionFailure:
                    DisplayAssertionExtendedDetails(idx);
                    break;

                case ViewSupportManager.ExtendedDetailsMode.ImageData:
                    DisplayImageDetails(idx);
                    break;
            }

            if (FilterChangedDueToDialog) {
                // Some of these dialogs let you request to exclude the thing that you just saw or some element of it.  If this is the case then
                // they update the filter directly but depend on us to tell the view to physically refresh itself to take advantage of the
                // new filtering options that are in place.
                RefreshCurrentView(false);
            }
        }

        private void DisplayImageDetails(long idx) {
            using var fri = new frmImageData();
            fri.StartPosition = FormStartPosition.CenterParent;
            var bd = Core.ViewManager.GetBlobData(idx);
            fri.Populate(bd.Item1, bd.Item2);
            _ = fri.ShowDialog();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private void DisplayErrorMessageExtendedDetails(long idx) {
            //AssertionPopulationData apd
            //frmInteractiveAssertionDialog.MexRenderAssertionDialog(apd);
            var edd = Core.ViewManager.GetErrorFurtherInformation(idx);
            if (edd != null) {
                // If it is null then an internal error occured.
                //frmListenerErrorDialog.RenderErrorDialog(edd, false);
            } else {
                Core.ViewManager.AddUserNotificationMessageByIndex(UserMessages.InvalidDataStructureError, UserMessageType.WarningMessage, "Error detail could not be found in selected process.");
            }
        }

        private bool DisplayLogMessageExtendedDetails(long idx, bool FilterChangedDueToDialog) {
            using (var fed = new frmExtendedDetails()) {
                // Restore size settings from last time

                #region size and position the form

                fed.Height = ViewerStatus.MoreDetailsHeight;
                fed.Width = ViewerStatus.MoreDetailsWidth;
                fed.txtDebugEntry.Height = ViewerStatus.MoreDetailsTopPanelHeight;
                fed.txtEntryFurtherDetails.Height = ViewerStatus.MoreDetailsBottomPanelHeight;
                fed.StartPosition = FormStartPosition.CenterParent;

                #endregion size and position the form

                Core.ViewManager.PopulateLogMessageDetailsScreen(idx, fed.txtDebugEntry, fed.txtSecondaryMessage, fed.txtEntryFurtherDetails);
                FilterChangedDueToDialog = fed.ShowDialog() == DialogResult.Yes;

                // Remember end size settings.

                #region store the size information that they finished with

                ViewerStatus.MoreDetailsHeight = fed.Height;
                ViewerStatus.MoreDetailsWidth = fed.Width;
                ViewerStatus.MoreDetailsTopPanelHeight = fed.txtDebugEntry.Height;
                ViewerStatus.MoreDetailsBottomPanelHeight = fed.txtEntryFurtherDetails.Height;

                #endregion store the size information that they finished with
            }
            return FilterChangedDueToDialog;
        }

        private static void DisplayAssertionExtendedDetails(long idx) {
            _ = Core.ViewManager.GetAssertionFurtherInformation(idx);
            //frmInteractiveAssertionDialog.MexRenderAssertionDialog(apd);
        }

        private void RequestClearAllData() {
            //Bilge.Log("User requests a clear of all data in Mex");
            AlterScreenEnableState(ScreenEnabledStates.InvalidProcessSelected);
            Core.ViewManager.RequestPurgeAll();
            SetMainViewFollowingPurgeAll();
        }

        private void btnClearAll_Click(object sender, System.EventArgs e) {
            RequestClearAllData();
        }

        private void lvwMainODSView_DoubleClick(object sender, System.EventArgs e) {
            /*if (lvwMainODSView.SelectedItems.Count == 0) {
               //Bilge.Warning("WARNING!!!! --> SelectedIndexChanged when there were no selected items on main view.  More info cant display properly");
                return;
            }

            ListViewItem lvi = lvwMainODSView.SelectedItems[0]; // we dont allow multi select therefore this should only be one
           //Bilge.Assert(lvi != null, "This should not happen, selected index changed yet there was nothing selected but the time that we looked");

            long idx = long.Parse(lvi.Text);

            // TODO : Fix this.
            using (frmExtendedDetails fed = new frmExtendedDetails()) {
              this.positiondialogoverview()
              fed.PopulateFromIdx(idx);
              fed.ShowDialog();
            }*/
        }

        private enum MexViews {
            MainView, ProcessView, ProcessTreeView, ResourceView, TimedView, Unknown
        }

        private MexViews GetCurrentlySelectedView() {
            if (tbcMainView.SelectedTab == tabODSView) { return MexViews.MainView; }
            if (tbcMainView.SelectedTab == tabProcessView) { return MexViews.ProcessView; }
            if (tbcMainView.SelectedTab == tabProcessTree) { return MexViews.ProcessTreeView; }
            if (tbcMainView.SelectedTab == tabResourceView) { return MexViews.ResourceView; }
            if (tbcMainView.SelectedTab == tabTimedView) { return MexViews.TimedView; }

            // Duh
            //Bilge.Assert(false, "Unknown view has been selected this should not be possible");
            return MexViews.Unknown;
        }

        private void btnDoFind_Click(object sender, EventArgs e) {
            ExecuteFind(false);
        }

        private void ExecuteFind(bool incrementalFind) {
            //Bilge.Warning("non modal find dialog not supported yet.  Should be added in.");

            if ((!incrementalFind) || (MexCore.TheCore.ViewManager.ActiveFind == null)) {
                // We need to create a new active find structure
                using var ffd = new frmFindDialog();
                PositionDialogOverView(ffd, false);
                if (ffd.ShowDialog() == DialogResult.OK) {
                    MexCore.TheCore.ViewManager.ActiveFind = ffd.GetFindStructure();
                } else {
                    // Cancled out of the find dialog, cancel the find.
                    return;
                }
            }

            switch (GetCurrentlySelectedView()) {
                case MexViews.MainView: mnuFindFind_Click(null, null); break;
                case MexViews.ProcessView: MexCore.TheCore.ViewManager.ProcView_Find(lvwProcessView, incrementalFind, currentFindIncrementalIndexPreference); break;
                case MexViews.ProcessTreeView: throw new NotImplementedException();
                case MexViews.ResourceView: throw new NotImplementedException();
                case MexViews.TimedView: throw new NotImplementedException();
                default:
                    //Bilge.Assert(false, "Invalid view selected for find");
                    break;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e) {
            //Bilge.Log("Keydown recieved" + e.KeyCode);

            // Find has been activated.
            if (e.KeyCode == Keys.F3) {
                //Bilge.Log("F3 pressed to start / continue  a search.");
                if (mnuFindFind.Enabled) {
                    ExecuteFind(true);
                }
            }
        }

        private void ShowOptionsScreen() {
            using var fmos = new frmMexOptionsScreen();
            PositionDialogOverView(fmos, false);
            fmos.PopulateOptionsScreenFromOptions(MexCore.TheCore.Options);

            if (fmos.ShowDialog() == DialogResult.OK) {
                MexCore.TheCore.Options.ApplyOptionsToApplication(fmos.GetOptionsFromDialog(), false);
            }
        }

        private void btnShowOptions_Click(object sender, EventArgs e) {
            ShowOptionsScreen();
        }

        private void btnCancelRefresh_Click(object sender, EventArgs e) {
            Core.ViewManager.CancelCurrentViewOperation = true;
        }

        private void btnSetTabProcessTreeView_Click(object sender, EventArgs e) {
            SelectTreeView();
        }

        private void btnRefreshThreadViewNow_Click_1(object sender, EventArgs e) {
            RefreshCurrentView();
        }

        private void btnExpandAll_Click(object sender, EventArgs e) {
            tvwProcTraceTree.ExpandAll();
        }

        private void btnCollapseAll_Click(object sender, EventArgs e) {
            tvwProcTraceTree.CollapseAll();
        }

        private void mnuPurgePurgeAllButThis_Click(object sender, EventArgs e) {
            //Bilge.Log("User Requests PurgeAllButthis from menu.");
            Core.ViewManager.RequestPurgeAllExceptCurrentlySelected();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            //Bilge.Log("FormClosed request, storing data for form");

            //Bilge.E("Mex::UI >> Form_Closed");

            try {
                ApplicationShutdown();

                if (ViewerStatus != null) {
                    ViewerStatus.Height = Height;
                    ViewerStatus.Width = Width;
                    ViewerStatus.XLoc = Left;
                    ViewerStatus.YLoc = Top;

                    // If it has been saved off screen, even partially we reset it next time.  This prevents the obscure case where it was
                    // out of view off screen and can not be brought back on because it remembers it should be off screen.
                    if (ViewerStatus.XLoc < 0) {
                        ViewerStatus.XLoc = 0;
                    }
                    if (ViewerStatus.YLoc < 0) {
                        ViewerStatus.YLoc = 0;
                    }

                    ViewerStatus.ProcessViewThreadColumnWidth = lvwProcessView.Columns[0].Width;
                    ViewerStatus.ProcessViewThreadColumnWidth = lvwProcessView.Columns[1].Width;
                    ViewerStatus.ProcessViewLocationColumnWidth = lvwProcessView.Columns[2].Width;
                    ViewerStatus.ProcessViewDebugMessageColumnWidth = lvwProcessView.Columns[3].Width;

                    //Bilge.Log("Saving Viewer Configuration data");

                    SaveViewerConfigurationData();
                }
            } finally {
                //Bilge.X();
            }
        }

        private void duplicateThisProcessToolStripMenuItem_Click(object sender, EventArgs e) {
            //Bilge.Assert(lvwProcessView.SelectedItems.Count > 1, " Invalid call to set index range");
            var fcd = new frmCreateDuplicate();
            //this.PositionDialogOverView(fcd, true);
            fcd.DescribeWhatsHappening(Core.ViewManager.GetSelectedAppDescription());
            if (fcd.ShowDialog() == DialogResult.OK) {
                Core.ViewManager.CreateDuplicateOfSelectedApplication(fcd.GetNameForDupe());
            }
        }

        private void duplicateToolStripMenuItem_Click(object sender, EventArgs e) {
            //Bilge.Assert(lvwProcessView.SelectedItems.Count > 1, " Invalid call to set index range");

            long currentStartIndex = (long)lvwProcessView.SelectedItems[0].Tag;
            long currentEndIndex = (long)lvwProcessView.SelectedItems[^1].Tag;

            var fcd = new frmCreateDuplicate();
            PositionDialogOverView(fcd, true);

            fcd.DescribeWhatsHappening(Core.ViewManager.GetSelectedAppDescription(), currentStartIndex, currentEndIndex);
            if (fcd.ShowDialog() == DialogResult.OK) {
                Core.ViewManager.CreateDuplicateOfSelectedApplication(fcd.GetNameForDupe(), currentStartIndex, currentEndIndex);
            }
        }

        private void txtCurrentProcessLabel_Leave(object sender, EventArgs e) {
            //Bilge.Log("Viewer setting label for current process to " + txtCurrentProcessLabel.Text);
            Core.ViewManager.SetNameOfCurrentProcess(txtCurrentProcessLabel.Text);
            RedrawProcessLists();
        }

        private void mnuCtxtHideAllEntriesToolStripMenuItem_Click(object sender, EventArgs e) {
            // Hides all of the entries that this application knows of.
            Core.ViewManager.MaskEventEntries();
            RefreshCurrentView(false);
        }

        private void mnuCtxtHideAllEntriesTillNowToolStripMenuItem_Click(object sender, EventArgs e) {
            //Bilge.Assert(lvwProcessView.SelectedItems.Count > 0, " Context menu for hiding the selected items should not be enabled when no items selected");
            if (lvwProcessView.SelectedItems.Count == 0) {
                //Bilge.Warning("User has clicked the context menu and this has taken focus and selection awsay from lvwProcessView, making the click invalid");
                return;
            }
            long nextIdx = (long)lvwProcessView.SelectedItems[0].Tag;
            Core.ViewManager.MaskEventEntriesUpTillGlobalIndex(nextIdx);
            RefreshCurrentView(false);
        }

        private void mnuCtxtHideThisEntryToolStripMenuItem_Click(object sender, EventArgs e) {
            //Bilge.Assert(lvwProcessView.SelectedItems.Count > 0, " Context menu for hiding the selected items should not be enabled when no items selected");
            for (int i = 0; i < lvwProcessView.SelectedItems.Count; i++) {
                long nextIdx = (long)lvwProcessView.SelectedItems[i].Tag;
                Core.ViewManager.MaskEventEntryByGlobalIndex(nextIdx);
            }
            RefreshCurrentView(false);
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e) {
            lvwProcessView_DoubleClick(sender, e);
        }

        #region context menu filters for process view.

        /// <summary>
        /// All of the context menus call this function which will modify the filter according to the options that were chosen.  There is
        /// enough commonality that the routine should exist but its not a very clear one.
        /// </summary>
        /// <param name="nextIdx">The global index of the event entry to parse - must be within the current view.</param>
        /// <param name="include">if set to <c>true</c> to include this text in the filter, <c>false</c> to exlcude.</param>
        /// <param name="doThread">if set to <c>true</c> the thead identity will be used.</param>
        /// <param name="doLocation">if set to <c>true</c> the location data will be used.</param>
        /// <param name="doModule">if set to <c>true</c> the module data will be used.</param>
        /// <param name="doClass">if set to <c>true</c> the class data will be used</param>
        private void ApplySelectionToFilter(long nextIdx, bool include, bool doThread, bool doLocation, bool doModule, bool doClass) {
            var ee = MexCore.TheCore.ViewManager.GetEventEntryFromSelectedAppByIndex(nextIdx);
            var kdr = MexCore.TheCore.ViewManager.SelectedTracedApp.GetThreadDisplayValue(ee);
            _ = new List<KeyDisplayRepresentation>();

            MexCore.TheCore.ViewManager.CurrentFilter.BeginFilterUpdate();
            if (doThread) {
                if (include) {
                    var allThreadValues = Core.ViewManager.GetThreadNameListForSelectedApp();
                    for (int i = 0; i < allThreadValues.Count; i++) {
                        if (allThreadValues[i].KeyIdentity != ee.CurrentThreadKey) {
                            MexCore.TheCore.ViewManager.CurrentFilter.AppendThreadExclusion(allThreadValues[i]);
                        }
                    }
                } else {
                    MexCore.TheCore.ViewManager.CurrentFilter.AppendThreadExclusion(kdr);
                }
            }  // End section for doing threads

            List<string> allValues;
            if (doLocation) {
                if (include) {
                    allValues = Core.ViewManager.GetAllAdditionalLocations();
                    for (int i = 0; i < allValues.Count; i++) {
                        if (allValues[i] != ee.MoreLocationData) {
                            MexCore.TheCore.ViewManager.CurrentFilter.AppendLocationExclusion(allValues[i]);
                        }
                    }
                } else {
                    MexCore.TheCore.ViewManager.CurrentFilter.AppendLocationExclusion(ee.MoreLocationData);
                }
            }

            if (doClass && (ee.MoreLocationData.Length > 0)) {
                int idx = ee.MoreLocationData.IndexOf("::");
                if (idx > 0) {
                    string thisEntrysClass = ee.MoreLocationData[..idx];

                    if (include) {
                        allValues = Core.ViewManager.GetAllAdditionalLocations();
                        allValues = Core.ViewManager.GetAdditionalLocationClassesFromAdditionalLocations(allValues);

                        // Get all of the classes, then loop through them adding them all except the one we want to see.

                        for (int i = 0; i < allValues.Count; i++) {
                            if (allValues[i] != thisEntrysClass) {
                                MexCore.TheCore.ViewManager.CurrentFilter.AppendLocationExclusion(allValues[i]);
                            }
                        } // End for each of the location class info found
                    } else {
                        // Add the one we want to remove from the list
                        MexCore.TheCore.ViewManager.CurrentFilter.AppendLocClassExclusion(thisEntrysClass);
                    }
                }
            }
            if (doModule) {
                if (include) {
                    allValues = Core.ViewManager.GetAllModules();
                    for (int i = 0; i < allValues.Count; i++) {
                        if (allValues[i] != ee.Module) {
                            MexCore.TheCore.ViewManager.CurrentFilter.AppendModuleExclusion(allValues[i]);
                        }
                    }
                } else {
                    MexCore.TheCore.ViewManager.CurrentFilter.AppendModuleExclusion(ee.Module);
                }
            }
            MexCore.TheCore.ViewManager.CurrentFilter.EndFilterUpdate();
            RefreshCurrentView(false);
        } // ApplySelectionToFilter

        private void mnuCtxtFilterThreadsIncludeThis_Click(object sender, EventArgs e) {
            ApplySelectionToFilter((long)lvwProcessView.SelectedItems[0].Tag, true, true, false, false, false);
        }

        private void mnuCtxtFilterThreadsExcludeThis_Click(object sender, EventArgs e) {
            ApplySelectionToFilter((long)lvwProcessView.SelectedItems[0].Tag, false, true, false, false, false);
        }

        private void mnuCtxtFilterModulesIncludeThis_Click(object sender, EventArgs e) {
            ApplySelectionToFilter((long)lvwProcessView.SelectedItems[0].Tag, true, false, false, true, false);
        }

        private void mnuCtxtFilterModulesExcludeThis_Click(object sender, EventArgs e) {
            ApplySelectionToFilter((long)lvwProcessView.SelectedItems[0].Tag, false, false, false, true, false);
        }

        private void mnuCtxtFilterLocationsIncludeThis_Click(object sender, EventArgs e) {
            ApplySelectionToFilter((long)lvwProcessView.SelectedItems[0].Tag, true, false, true, false, false);
        }

        private void mnuCtxtFilterLocationsExcludeThis_Click(object sender, EventArgs e) {
            ApplySelectionToFilter((long)lvwProcessView.SelectedItems[0].Tag, false, false, true, false, false);
        }

        private void showOnlyThisClassToolStripMenuItem_Click(object sender, EventArgs e) {
            ApplySelectionToFilter((long)lvwProcessView.SelectedItems[0].Tag, true, false, false, false, true);
        }

        private void excludeThisClassToolStripMenuItem_Click(object sender, EventArgs e) {
            ApplySelectionToFilter((long)lvwProcessView.SelectedItems[0].Tag, false, false, false, false, true);
        }

        #endregion context menu filters for process view.

        private void mnuViewPromoteCurrent_Click(object sender, EventArgs e) {
            // Takes the current view and extracts it to a view window.
            var fvw = new frmViewInAWindow();
            PositionDialogOverView(fvw, true);
            m_PromotedViews ??= new List<frmViewInAWindow>();

            m_PromotedViews.Add(fvw);

            bool populated = false;

            if ((tbcMainView.SelectedTab == tabProcessView) || (tbcMainView.SelectedTab == tabProcessTree)) {
                string machine = Core.ViewManager.SelectedTracedApp.MachineName;
                string pid = Core.ViewManager.SelectedTracedAppPid.ToString();

                foreach (ListViewItem lvi in lvwProcessView.Items) {
                    fvw.AddEntry(lvi.ImageIndex, (long)lvi.Tag, machine, pid, lvi.SubItems[1].Text, lvi.SubItems[2].Text, lvi.SubItems[3].Text);
                }

                populated = true;
            }

            if (tbcMainView.SelectedTab == tabODSView) {
                foreach (ListViewItem lvi in lvwMainODSView.Items) {
                    // TODO : ODS Image index
                    fvw.AddEntry(1, (long)lvi.Tag, "localhost", lvi.SubItems[1].Text, "", "MainView", lvi.SubItems[2].Text);
                }
                populated = true;
            }

            if (tbcMainView.SelectedTab == tabCrossProcessView) {
                foreach (ListViewItem lvi in lvwCrossProcesList.Items) {
                    //                    string machinePidKey = lvi.SubItems[1].Text;
                    string[] msp = lvi.SubItems[1].Text.Split('\\');

                    fvw.AddEntry(1, (long)lvi.Tag, msp[0], msp[1], lvi.SubItems[2].Text, lvi.SubItems[3].Text, lvi.SubItems[4].Text);
                }

                populated = true;
            }

            if (!populated) {
                //Bilge.Warning("Trying to promote a view that has not yet been implemented to the window will not work.");
                throw new NotImplementedException("The promotion of this view has not been implemented");
            }

            fvw.Show();
        }

        private void btnRefreshCPV_Click(object sender, EventArgs e) {
            RefreshCrossProcessViewItself();
        }

        private void btnSetTabCrossProcessView_Click(object sender, EventArgs e) {
            SelectCrossProcessView();
        }

        private void lvwCrossProcesList_SelectedIndexChanged(object sender, EventArgs e) {
            // This seems to get called even when no items are selected, wierdly.
            if (lvwCrossProcesList.SelectedItems.Count == 0) { return; }

            //Bilge.Assert(lvwCrossProcesList.SelectedItems[0].Tag != null, " The CPV assumes that all list items have a tag value of the gidx associated with the element");

            long gidx = (long)lvwCrossProcesList.SelectedItems[0].Tag;
            txtCpvMoreDetails.Text = Core.ViewManager.GetMoreInfoForEventIndexGlobally(gidx);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e) {
            var clippy = new StringBuilder();

            foreach (ListViewItem lvi in lvwProcessView.SelectedItems) {
                _ = clippy.Append(string.Format("{0},{1},{2},{3},{4}" + Environment.NewLine, lvi.Text, lvi.SubItems[0].Text, lvi.SubItems[1].Text, lvi.SubItems[2].Text, lvi.SubItems[3].Text));
            }

            Clipboard.SetText(clippy.ToString());
        }

        private void mnuPurgePurgeThisProcess_Click(object sender, EventArgs e) {
            PerformSelectedProcessPurge();  // the main view is still valid therefore we switch to it.
        }

        private void PerformSelectedProcessPurge() {
            RequestPurgeSelectedProcess();
            SetMainViewFollowingPurgeOne();
        }

        private void mnuPurgeClearThisProcess_Click(object sender, EventArgs e) {
            RequestClearSelectedProcess();
        }

        private void mnuPurgePurgeAllProcesses_Click(object sender, EventArgs e) {
            RequestClearAllData();
        }

        private void cboMainViewQuickFilterChange_SelectedIndexChanged(object sender, EventArgs e) {
            //Bilge.E();
            try {
                if (cboMainViewQuickFilterChange.Text != " > Refresh List <") {
                    string filterName = Path.Combine(MexCore.TheCore.Options.FilterAndHighlightStoreDirectory, cboMainViewQuickFilterChange.Text + MexCore.TheCore.Options.FilterExtension);

                    if (!File.Exists(filterName)) {
                        //Bilge.Log("Quick filter choice resulted in an invalid filter, refreshing list of filters.", filterName);
                        RefreshQuickFilterList();
                    } else {
                        //Bilge.Log("A filter was found, trying tbo load this filter into the view");

                        var vf = ViewFilter.LoadFilterFromFile(filterName);
                        Core.ViewManager.CurrentFilter = vf;
                        RefreshCurrentView(false);
                    }
                } else {
                    RefreshQuickFilterList();
                }
            } finally {
                //Bilge.X();
            }
        }

        private void highlightThisThreadColor1ToolStripMenuItem_Click(object sender, EventArgs e) {
            int tagval = (int)(sender as ToolStripMenuItem).Tag;

            long nextIdx = (long)lvwProcessView.SelectedItems[0].Tag;

            MexCore.TheCore.ViewManager.CurrentHighlightOptions.DefaultHighlightColoring[tagval].matchCase = MexCore.TheCore.ViewManager.GetEventEntryFromSelectedAppByIndex(nextIdx).ThreadID;
            tagval++;
            tagval %= MexCore.TheCore.ViewManager.CurrentHighlightOptions.DefaultHighlightColoring.Length;
            (sender as ToolStripMenuItem).Tag = tagval;

            if (MexCore.TheCore.ViewManager.ApplyCurrentHighlightOptionsToView()) {
                RefreshCurrentView(false);
            }
        }

        #region hook up the menus that do the same as the buttons

        private void mnuViewOptions_Click(object sender, EventArgs e) {
            btnShowOptions_Click(sender, e);
        }

        private void mnuViewHighlights_Click(object sender, EventArgs e) {
            btnSetHighlight_Click(sender, e);
        }

        private void mnuViewViewsTimedView_Click(object sender, EventArgs e) {
            if (MessageBox.Show("Timed view takes a very long time to display.  Are you sure?", "Perf Warning", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                SelectTimedView();
            }
        }

        private void mnuViewViewsMainView_Click(object sender, EventArgs e) {
            btnSetTabMain_Click(sender, e);
        }

        private void mnuViewViewsProcessView_Click(object sender, EventArgs e) {
            btnSetTabProcess_Click(sender, e);
        }

        private void mnuViewViewsResourceView_Click(object sender, EventArgs e) {
            btnResources_Click(sender, e);
        }

        private void mnuViewViewsTreeView_Click(object sender, EventArgs e) {
            btnSetTabProcessTreeView_Click(sender, e);
        }

        #endregion hook up the menus that do the same as the buttons

        private void mnuViewViewsSetCrossProcessview_Click(object sender, EventArgs e) {
            btnSetTabCrossProcessView_Click(sender, e);
        }

        private void mnuViewViewsSetProcessThreadView_Click(object sender, EventArgs e) {
            btnSetTabThreads_Click(sender, e);
        }

        private void tmrUpdateUI_Tick(object sender, EventArgs e) {
            string s = Core.ViewManager.QueuedMessagesCount;

            tslStatusLabel1.Text = "Q[ " + s + " ]";

            s = Core.ViewManager.LastUserNotificationMessage;
            tsStatusLabelMessageBar.Text = s;

            s = Core.ViewManager.LastIssuedGlobalIndex;
            tsStatusLabelLastIndex.Text = s;

            s = Core.ViewManager.LastMessageRecievedTime;

            tsStatusLabelTime.Text = "at " + s;

            if (timerTriggeredRefreshRequired) {
                var elapsed = DateTime.Now - m_lastRefreshNotfication;

                if (elapsed.TotalSeconds < MexCore.TheCore.Options.NoSecondsForUIUpdate) {
                    // Dont do the refresh this time around.
                    return;
                }

                timerTriggeredRefreshRequired = false;
                MexCore.TheCore.ViewManager.RequestViewRefreshNotification(triggerRefreshisIncremental);
            }
        }

        private void btnUserMessages_Click(object sender, EventArgs e) {
        }

        private void mnuViewFilters_Click(object sender, EventArgs e) {
            btnSetFilter_Click(sender, e);
        }

        private void ctxtPVCtxtMenu_Opening(object sender, CancelEventArgs e) {
            bool enableStateForItemSelected = lvwProcessView.SelectedItems.Count > 0;

            mnuCtxtFilterLocationsExcludeThis.Enabled = enableStateForItemSelected;
            mnuCtxtFilterLocationsIncludeThis.Enabled = enableStateForItemSelected;
            mnuCtxtFilterModulesExcludeThis.Enabled = enableStateForItemSelected;
            mnuCtxtFilterModulesIncludeThis.Enabled = enableStateForItemSelected;
            mnuCtxtHideThisEntryToolStripMenuItem.Enabled = enableStateForItemSelected;
            mnuCtxtHideAllEntriesBeforeThisIndexToolStripMenuItem.Enabled = enableStateForItemSelected;
        }

        private void mnuCtxtUnhideAllToolStripMenuItem_Click(object sender, EventArgs e) {
            ForceFilterUpdateOnDisplay();
        }

        private void ForceFilterUpdateOnDisplay() {
            // The way in which filters work is that they have a unique number internally and that the displays check against
            // the filter number to see if they need to change their display characteristics.  This method artificially updates
            // the filter number and therefore forces a redisplay.
            Core.ViewManager.CurrentFilter.BeginFilterUpdate();
            Core.ViewManager.CurrentFilter.EndFilterUpdate();
            RefreshCurrentView(false);
        }

        private void mnuCtxtResetHighlightToolStripMenuItem_Click(object sender, EventArgs e) {
            Core.ViewManager.ReapplyHighlight();
        }

        private void btnAutoRefreshQuick_Click(object sender, EventArgs e) {
            Core.Options.AutoRefresh = !Core.Options.AutoRefresh;
            RefreshQuickIcons();
        }

        private void btnAutoScrollQuick_Click(object sender, EventArgs e) {
            Core.Options.AutoScroll = !Core.Options.AutoScroll;
            RefreshQuickIcons();
        }

        private void RefreshQuickIcons() {
            btnAutoScrollQuick.ImageIndex = Core.Options.AutoScroll ? 39 : 41;

            btnAutoRefreshQuick.ImageIndex = Core.Options.AutoRefresh ? 40 : 42;
        }

        private void SetRefreshTime(int seconds) {
            if (seconds is 1 or 2 or 5) {
                Core.Options.NoSecondsForUIUpdate = seconds;
            }
            if (seconds == 1) {
                mnuOptionsRefresh1Second.Checked = true;
                mnuOptionsRefresh2Seconds.Checked = false;
                mnuOptionsRefresh5Seconds.Checked = false;
            }
            if (seconds == 2) {
                mnuOptionsRefresh1Second.Checked = false;
                mnuOptionsRefresh2Seconds.Checked = true;
                mnuOptionsRefresh5Seconds.Checked = false;
            }
            if (seconds == 5) {
                mnuOptionsRefresh1Second.Checked = false;
                mnuOptionsRefresh2Seconds.Checked = false;
                mnuOptionsRefresh5Seconds.Checked = true;
            }
        }

        private void mnuOptionsRefresh1Second_Click(object sender, EventArgs e) {
            SetRefreshTime(1);
        }

        private void mnuOptionsRefresh2Seconds_Click(object sender, EventArgs e) {
            SetRefreshTime(2);
        }

        private void mnuOptionsRefresh5Seconds_Click(object sender, EventArgs e) {
            SetRefreshTime(5);
        }

        private void mnuOptionsRefreshPaused_Click(object sender, EventArgs e) {
            btnAutoRefreshQuick_Click(sender, e);
        }

        private AdditionalData currentAdditionalData; // Default null

        private void btnAdditional_Click(object sender, EventArgs e) {
            if (tbcMainView.SelectedTab == tabTimingsView) {
                using var tads = new frmTimingsAdditionalScreen();
                tads.SetScreenFromAddtionalData(currentAdditionalData);
                if (tads.ShowDialog() == DialogResult.OK) {
                    currentAdditionalData = tads.GetAdditionalDataFromScreen();
                }
                return;
            }

            if (tbcMainView.SelectedTab == tabTimedView) {
            }

            if (tbcMainView.SelectedTab == tabProcessView) {
            }

            if (tbcMainView.SelectedTab == tabODSView) {
            }

            if (tbcMainView.SelectedTab == tabProcessThreadView) {
            }

            if (tbcMainView.SelectedTab == tabProcessTree) {
            }

            if (tbcMainView.SelectedTab == tabResourceView) {
            }

            if (tbcMainView.SelectedTab == tabCrossProcessView) {
            }

            using var fmum = new frmMexUserMessages();
            fmum.InitialiseFromMessageStore();
            _ = fmum.ShowDialog();
        }

        private void showOptionsToolStripMenuItem_Click(object sender, EventArgs e) {
            ShowOptionsScreen();
        }

        private void lvwProcessView_Resize(object sender, EventArgs e) {
            ResizeProcessViewColumns();
        }

        private void tbcMainView_SizeChanged(object sender, EventArgs e) {
            // Needed to redraw correctly on resize for the list views.
            if ((tbcMainView.SelectedTab == tabProcessView) || (tbcMainView.SelectedTab == tabProcessThreadView)) {
                btnRefresh_Click(sender, e);
            }
        }

        private void lvwThreadView_SelectedIndexChanged(object sender, EventArgs e) {
            if (lvwThreadView.SelectedItems.Count == 0) {
                //Bilge.Log("WARNING!!!! --> SelectedIndexChanged when there were no selected items on lvwThreadView view.  More info cant display properly");
                txtThreadViewDetails.Text = string.Empty;
                return;
            }

            // They are interacting with the process view and selecting different entries
            // we need to identify the one that they selected and present sensible information in the bottom half of the screen.
            var lvi = lvwThreadView.SelectedItems[0]; // we dont allow multi select therefore this should only be one
                                                      //Bilge.Assert(lvi != null, "This should not happen, selected index changed yet there was nothing selected but the time that we looked");
                                                      //Bilge.Assert(lvi.Tag != null, "The tag should refer to the global index of the list view item, if its null its been incorrectly inserted into the list view.");

            long idx = (long)lvi.Tag;
            currentFindIncrementalIndexPreference = idx;
            string s = Core.ViewManager.GetMoreInfoForEventIndexInSelectedApp(idx);
            txtThreadViewDetails.Text = s;
        }

        private void btnRefreshTreeView_Click(object sender, EventArgs e) {
            ExecuteTreeViewRefreshCommand();
        }

        private void lvwCrossProcesList_DoubleClick(object sender, EventArgs e) {
            if (lvwCrossProcesList.SelectedItems.Count == 1) {
                long gIdx = (long)lvwCrossProcesList.SelectedItems[0].Tag;

                int desiredTA = Core.DataManager.FindTracedAppIndexThatContainsGlobalIndex(gIdx);
                if (Core.ViewManager.SelectedTracedApp.VirtualIndex != desiredTA) {
                    ChangeDisplayedProcess(desiredTA);
                }
                SelectProcessView(gIdx);
            }
        }

        private void llbScrollRight_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            llbScrollLeft.Enabled = true;
            processLinkLabelFirstOffset++;
            RedrawProcessLists();
        }

        private void llbScrollLeft_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            //Bilge.Assert(processLinkLabelFirstOffset > 0, "The label offset is in an invalid state for this control to be enabled");

            llbScrollRight.Enabled = true;

            processLinkLabelFirstOffset--;
            if (processLinkLabelFirstOffset == 0) {
                llbScrollLeft.Enabled = false;
            }
            RedrawProcessLists();
        }

        private void mnuViewRefreshProcesses_Click(object sender, EventArgs e) {
            RedrawProcessLists();
        }

        private void mnuViewAbout_Click(object sender, EventArgs e) {
            using var fmum = new frmMexUserMessages();
            fmum.InitialiseFromMessageStore();
            _ = fmum.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e) {
            using var fds = new frmDiagnosticsScreen();
            _ = fds.ShowDialog();
        }

        private void btnPurgeThis_Click(object sender, EventArgs e) {
            PerformSelectedProcessPurge();
        }

        private void spbtSelectThreads_Click(object sender, EventArgs e) {
            tsThreadViewHandler_Click(null, null);
        }

        private void BtnTransient_Click(object sender, EventArgs e) {
            SelectTransientView();
        }

        private void btnAlert_Click(object sender, EventArgs e) {
            SelectAlertView();
        }

        private void CboCustoms_SelectedIndexChanged(object sender, EventArgs e) {
            if (cboCustoms.SelectedItem != null) {
                var elmt = (TransientDisplay)cboCustoms.SelectedItem;
                // Get a Custom Event Entry.

                txtTransientText.Text = elmt.Text;
            } else {
                txtTransientText.Text = string.Empty;
            }
        }

        private void BtnReplay_Click(object sender, EventArgs e) {
            for (int i = 0; i < cboCustoms.Items.Count; i++) {
                cboCustoms.SelectedIndex = i;
                Application.DoEvents();
                Thread.Sleep(50);
            }
        }

        private void btnConnectToURI_Click(object sender, EventArgs e) {
            using var f = new frmConnectToEndpoint();
            if (f.ShowDialog() == DialogResult.OK) {
            }
        }
    }
}