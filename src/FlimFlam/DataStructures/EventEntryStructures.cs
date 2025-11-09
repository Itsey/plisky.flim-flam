using System;
using System.Collections.Generic;
using System.Diagnostics;
using Plisky.Diagnostics;
using Plisky.Diagnostics.FlimFlam;

namespace Plisky.FlimFlam;

/// <summary>
/// Each processes debug output consists of a series of event entries.  These event entries are what make up the growing logs
/// from the program.  Each event entry is stored in its own class - although much of the information is stripped and only the
/// event entry specific stuff remains - such as the thread ID
/// </summary>
[DebuggerDisplay("{Vsnetdbgvw}")]
public class EventEntry {
    internal TraceCommandTypes cmdType;

    internal string debugMessage;

    internal string lineNumber;

    // The type of this event
    internal string module;

    internal string moreLocationData;

    internal string secondaryMessage;

    // The more info part of the message
    internal string threadID;

    internal string threadNetId;

    // Additional location data sent
    // This is the result of that last filters visit and will be used again if filter.idx==lastvisitedfilter
    internal ViewSpecificData viewData;

    public EventEntry(SingleOriginEvent copyMe) {
        this.cmdType = copyMe.Type; //Refactoring_TraceCommandTypes.MessageTypeToTraceCommandType(copyMe.Type);
        SetDebugMessage(copyMe.Text);

        this.lineNumber = copyMe.LineNumber;
        this.module = copyMe.MethodName;
        this.moreLocationData = copyMe.MoreLocInfo;

        this.threadID = copyMe.ThreadId;
        this.threadNetId = copyMe.NetThreadId;
        this.viewData = new ViewSpecificData();
        this.GlobalIndex = copyMe.Id;
        this.HasMoreInfo = false;

        if (copyMe.Details != null) {
            this.secondaryMessage += copyMe.Details;
        }

        this.Tags = copyMe.Tags;

        //copyMe.HasMoreInfo;
        //this.SecondaryMessage = copyMe.;
        //this.LastVisitedFilter = copyMe.LastVisitedFilter;
        //this.LastVisitedFilterResult = copyMe.LastVisitedFilterResult;
    }

    // The time that Mex recieved the message
    internal EventEntry(long gIndex) {
        GlobalIndex = gIndex;
    }

    internal EventEntry(EventEntry copyMe) {
        this.cmdType = copyMe.cmdType;
        this.debugMessage = copyMe.debugMessage;
        this.HasMoreInfo = copyMe.HasMoreInfo;
        this.LastVisitedFilter = copyMe.LastVisitedFilter;
        this.LastVisitedFilterResult = copyMe.LastVisitedFilterResult;
        this.lineNumber = copyMe.lineNumber;
        this.module = copyMe.module;
        this.moreLocationData = copyMe.moreLocationData;
        this.secondaryMessage = copyMe.secondaryMessage;
        this.threadID = copyMe.threadID;
        this.threadNetId = copyMe.threadNetId;
        this.viewData = new ViewSpecificData();
    }

    internal EventEntry(long newGlobalIndex, string newModule, string newLineNo, string newThreadId, string newDebugMessage, string moreLocData, string threadnetidentity)
        : this(newGlobalIndex) {
        GlobalIndex = newGlobalIndex;
        threadID = newThreadId;
        debugMessage = newDebugMessage;
        moreLocationData = moreLocData;
        threadNetId = threadnetidentity;
        module = newModule;
        lineNumber = newLineNo;
        cmdType = TraceCommandTypes.Unknown;
        viewData.isValid = false;
    }

    internal string CurrentThreadKey {
        get {
            return this.threadNetId + "_" + this.threadID;
        }
    }

    internal long GlobalIndex { get; set; }

    // Globally assigned index.
    internal bool HasMoreInfo { get; set; }

    internal uint LastVisitedFilter { get; set; }

    // Filters dont change much, this is the id of the last filter used
    internal bool LastVisitedFilterResult { get; set; }

    internal SupportingMessageData SupportingData { get; set; }
    internal Dictionary<string, string> Tags { get; set; } = new Dictionary<string, string>();
    internal DateTime TimeMessageRecieved { get; set; }
    // OS ID of thread that caused message

    // .net ID of the thread that caused the message

    // This is used by the views to store info such as highlighting and find matches.

    private string Vsnetdbgvw {
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
        var ee = (EventEntry)obj;

        bool result = true;

        if (this.debugMessage != ee.debugMessage) {
            result = false;
        }
        if (this.cmdType != ee.cmdType) {
            result = false;
        }
        if (this.CurrentThreadKey != ee.CurrentThreadKey) {
            result = false;
        }
        if ((this.lineNumber != ee.lineNumber) || (this.module != ee.module) || (this.moreLocationData != ee.moreLocationData)) {
            result = false;
        }
        if (this.secondaryMessage != ee.secondaryMessage) {
            result = false;
        }
        return result;
    }

    public override int GetHashCode() {
        return base.GetHashCode();
    }

    internal static EventEntry CreatePseudoEE(long newGlobalIndex, string debugMessage) {
        var result = new EventEntry(newGlobalIndex, "Unknown", "00", "-1", debugMessage, "XRef::PidMatch", string.Empty);
        result.cmdType = TraceCommandTypes.LogMessage;
        result.viewData.isValid = false;
        return result;
    }

    internal string GetDiagnosticStringData() {
        string result = "Event Entry: \r\n" + " Msg(" + debugMessage + ")\r\n";
        result += "Index : " + GlobalIndex + "\r\n";
        return result;
    }

    internal void SetDebugMessage(string msg) {
        int markerPoint = msg.IndexOf("~~#~~");

        if (markerPoint >= 0) {
            // This debug message has attached to it a secondary message.
            secondaryMessage = msg.Substring(markerPoint + 5);
            debugMessage = msg.Substring(0, markerPoint);
        } else {
            debugMessage = msg;
            secondaryMessage = string.Empty;
        }
    }
} // end EventEntry class def