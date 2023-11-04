using System;

namespace Plisky.FlimFlam { 

    /// <summary>
    /// ProcessSummary contains the basic details about a known process ready for display.  This covers the process id, and name as well
    /// as its internal index.   The process summary struct is used for passing process details around the application.
    /// </summary>
    internal class ProcessSummary {
        internal string FullTextDescription = string.Empty;
        internal bool hasMoreThanOnethread;
        internal bool hasResourceData;
        internal bool hasTimingData;
        internal bool hasTraceData;
        internal int InternalIndex;
        internal string Machine;
        internal string ProcName = string.Empty;
        internal string settings = string.Empty;
        internal int WindowsPid;
        private string displayNameValue = string.Empty;
        private string labelGivenToProcess = string.Empty;
        private string preferredRenderName;

        /// <summary>
        /// ProcessSummary constructor to intiialise all values.
        /// </summary>
        /// <param name="machine">The name of the machine that was running this process</param>
        /// <param name="name">The friendly name of the process</param>
        /// <param name="id">The windows process id of the process</param>
        /// <param name="idx">The internal index of the process</param>
        internal ProcessSummary(string machine, string name, int id, int idx, string callingInfo, string execInfo) {
            DisplayName = name;
            WindowsPid = id;
            InternalIndex = idx;
            Machine = machine;
            StatusTitle = callingInfo;
            StatusText = execInfo;
            hasMoreThanOnethread = hasResourceData = hasTimingData = hasTraceData = false;
        }

        internal ProcessSummary(TracedApplication tap) {
            if (tap == null) {
                throw new InvalidOperationException("The traced application passed to ProcessSummary(TracedApplication) was null, the summary can not be created");
            }

            if (string.IsNullOrEmpty(tap.InitialiseName)) {
                DisplayName = tap.ProcessName;
            } else {
                DisplayName = tap.InitialiseName;
            }

            if (tap.ProcessLabel.Length > 0) {
                DisplayName += " ( " + tap.ProcessLabel + " )";
            }

            if (tap.ContainsMoreThanOneThread) {
                settings += "Multithreaded;";
            }
            if (tap.ContainsResourceData) {
                settings += "ResourceInfo;";
            }
            if (tap.ContainsTimingData) {
                settings += "Timed;";
            }

            if (tap.ProcessLabel.Length > 0) {
                tap.PreferredDisplayName = tap.ProcessLabel;
            } else {
                if (tap.PreferredDisplayName.Length > 0) {
                    this.PreferredRenderName = tap.PreferredDisplayName;
                } else {
                    tap.PreferredDisplayName = DisplayName;
                }
            }

            FullTextDescription = "(" + this.ProcLabel + ")(" + this.ProcName + ")(" + tap.PreferredDisplayName + ")(" + tap.ProcessIdAsString + ")";

            ProcLabel = tap.ProcessLabel;
            ProcName = tap.ProcessName;

            WindowsPid = tap.ProcessIdNo;
            InternalIndex = tap.VirtualIndex;
            Machine = tap.MachineName;
            if (tap.StatusText == string.Empty) {
                StatusTitle = tap.CallingAssemblyInfo;
                StatusText = tap.ExecutingAssemblyInfo;
            } else {
                StatusTitle = tap.StatusText;
                StatusText = tap.StatusContents;
            }
            hasMoreThanOnethread = tap.ContainsMoreThanOneThread;
            hasResourceData = tap.ContainsResourceData;
            hasTimingData = tap.ContainsTimingData;
            hasTraceData = tap.ContainsTraceData;
        }

        public string StatusText { get; set; }
        public string StatusTitle { get; set; }

        internal string DisplayName {
            get { return displayNameValue; }
            set { displayNameValue = value; }
        }

        internal string PreferredRenderName {
            get {
                if ((MexCore.TheCore.Options.UsePreferredNameInsteadOfProcessId) && (preferredRenderName != null) && (preferredRenderName.Length > 0)) {
                    return preferredRenderName;
                } else {
                    return WindowsPid.ToString();
                }
            }

            set {
                preferredRenderName = value;
            }
        }

        internal string ProcLabel {
            get { return labelGivenToProcess; }
            set {
                if (value != null) {
                    labelGivenToProcess = value;
                    preferredRenderName = value;
                }
            }
        }

        public override string ToString() {
            return Machine + "\\" + WindowsPid + " (" + DisplayName + ")";
        }
    }
}