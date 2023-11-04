using System;
using System.Collections.Generic;
using System.Diagnostics;
using Plisky.Diagnostics;
using Plisky.Diagnostics.FlimFlam;

namespace Plisky.FlimFlam { 

    /// <summary>
    /// Each processes debug output consists of a series of event entries.  These event entries are what make up the growing logs
    /// from the program.  Each event entry is stored in its own class - although much of the information is stripped and only the
    /// event entry specific stuff remains - such as the thread ID
    /// </summary>
    [DebuggerDisplay("{m_vsnetdbgvw}")]
    internal class EventEntry {
        internal TraceCommandTypes cmdType;
        internal string DebugMessage;
        internal long GlobalIndex;

        // Globally assigned index.
        internal bool HasMoreInfo;

        internal uint LastVisitedFilter;

        // Filters dont change much, this is the id of the last filter used
        internal bool LastVisitedFilterResult;

        internal string LineNumber;
        internal DateTime m_timeMessageRecieved;

        // The type of this event
        internal string Module;

        internal string MoreLocationData;
        internal string SecondaryMessage;
        internal SupportingMessageData SupportingData;

        internal Dictionary<string, string> Tags = new Dictionary<string, string>();

        // The more info part of the message
        internal string ThreadID;       // OS ID of thread that caused message

        internal string ThreadNetId;  // .net ID of the thread that caused the message

        // Additional location data sent
        // This is the result of that last filters visit and will be used again if filter.idx==lastvisitedfilter
        internal ViewSpecificData ViewData;  // This is used by the views to store info such as highlighting and find matches.

        // The time that Mex recieved the message

        public EventEntry(SingleOriginEvent copyMe) {
            this.cmdType = copyMe.Type; //Refactoring_TraceCommandTypes.MessageTypeToTraceCommandType(copyMe.Type);
            SetDebugMessage(copyMe.Text);

            this.LineNumber = copyMe.LineNumber;
            this.Module = copyMe.MethodName;
            this.MoreLocationData = copyMe.MoreLocInfo;

            this.ThreadID = copyMe.ThreadId;
            this.ThreadNetId = copyMe.NetThreadId;
            this.ViewData = new ViewSpecificData();
            this.GlobalIndex = copyMe.Id;
            this.HasMoreInfo = false;

            if (copyMe.Details != null) {
                this.SecondaryMessage += copyMe.Details;
            }

            this.Tags = copyMe.Tags;

            //copyMe.HasMoreInfo;
            //this.SecondaryMessage = copyMe.;
            //this.LastVisitedFilter = copyMe.LastVisitedFilter;
            //this.LastVisitedFilterResult = copyMe.LastVisitedFilterResult;
        }

        internal EventEntry(long gIndex) {
            GlobalIndex = gIndex;
        }

        internal EventEntry(EventEntry copyMe) {
            this.cmdType = copyMe.cmdType;
            this.DebugMessage = copyMe.DebugMessage;
            this.HasMoreInfo = copyMe.HasMoreInfo;
            this.LastVisitedFilter = copyMe.LastVisitedFilter;
            this.LastVisitedFilterResult = copyMe.LastVisitedFilterResult;
            this.LineNumber = copyMe.LineNumber;
            this.Module = copyMe.Module;
            this.MoreLocationData = copyMe.MoreLocationData;
            this.SecondaryMessage = copyMe.SecondaryMessage;
            this.ThreadID = copyMe.ThreadID;
            this.ThreadNetId = copyMe.ThreadNetId;
            this.ViewData = new ViewSpecificData();
        }

        internal EventEntry(long newGlobalIndex, string newModule, string newLineNo, string newThreadId, string newDebugMessage, string moreLocData, string threadnetidentity)
            : this(newGlobalIndex) {
            GlobalIndex = newGlobalIndex;
            ThreadID = newThreadId;
            DebugMessage = newDebugMessage;
            MoreLocationData = moreLocData;
            ThreadNetId = threadnetidentity;
            Module = newModule;
            LineNumber = newLineNo;
            cmdType = TraceCommandTypes.Unknown;
            ViewData.isValid = false;
        }

        internal string CurrentThreadKey {
            get {
                return this.ThreadNetId + "_" + this.ThreadID;
            }
        }

        private string m_vsnetdbgvw {
            get { return cmdType.ToString() + " idx : " + GlobalIndex.ToString(); }
        }

        /// <summary>
        /// Determines if this event entry and the one passed contain the same physical data.
        /// </summary>
        /// <remarks> This ignores times and global indexes</remarks>
        /// <param name="obj">An event entry to compare against</param>
        /// <returns>True if the data is the same</returns>
        public override bool Equals(object obj) {
            if (obj.GetType() == typeof(DBNull)) {
                return false;
            }
            if ((obj.GetType() != typeof(EventEntry))) {
                throw new InvalidCastException("You can not compare an EventEntry with anything that is not an EventEntry");
            }
            EventEntry ee = (EventEntry)obj;

            bool result = true;

            if (this.DebugMessage != ee.DebugMessage) {
                result = false;
            }
            if (this.cmdType != ee.cmdType) {
                result = false;
            }
            if (this.CurrentThreadKey != ee.CurrentThreadKey) {
                result = false;
            }
            if ((this.LineNumber != ee.LineNumber) || (this.Module != ee.Module) || (this.MoreLocationData != ee.MoreLocationData)) {
                result = false;
            }
            if (this.SecondaryMessage != ee.SecondaryMessage) {
                result = false;
            }
            return result;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        internal static EventEntry CreatePseudoEE(long newGlobalIndex, string debugMessage) {
            EventEntry result = new EventEntry(newGlobalIndex, "Unknown", "00", "-1", debugMessage, "XRef::PidMatch", string.Empty);
            result.cmdType = TraceCommandTypes.LogMessage;
            result.ViewData.isValid = false;
            return result;
        }

        internal string GetDiagnosticStringData() {
            string result = "Event Entry: \r\n" + " Msg(" + DebugMessage + ")\r\n";
            result += "Index : " + GlobalIndex + "\r\n";
            return result;
        }

        internal void SetDebugMessage(string msg) {
            int markerPoint = msg.IndexOf("~~#~~");

            if (markerPoint >= 0) {
                // This debug message has attached to it a secondary message.
                SecondaryMessage = msg.Substring(markerPoint + 5);
                DebugMessage = msg.Substring(0, markerPoint);
            } else {
                DebugMessage = msg;
                SecondaryMessage = string.Empty;
            }
        }
    } // end EventEntry class def
}