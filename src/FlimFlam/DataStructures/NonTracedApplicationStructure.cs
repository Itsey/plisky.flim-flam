using System;

namespace Plisky.FlimFlam; 

/// <summary>
/// Summary description for NonTracedApplicationStructure.
/// </summary>
internal class NonTracedApplicationEntry {
    internal long assignedIndex;

    internal string DebugEntry { get; set; }

    internal int Pid { get; set; }

    internal ViewSpecificData viewData;

    internal NonTracedApplicationEntry() {
        Pid = -1;
        assignedIndex = -1;
    }

    internal NonTracedApplicationEntry(int incommingPid, string incommingDebugEntry, long gIndex) {
        if (incommingPid == -1) {
            // This indicates that they did not know the pid at the time that this message was placed into the structure.
            // sometiems this means that the pid is stored as the first part of the string, with a comma separating that and the text
            // this occurs for example on file imports.
            if (incommingDebugEntry.IndexOf(',') > 0) {
                try {
                    string possiblePid = incommingDebugEntry.Substring(0, incommingDebugEntry.IndexOf(','));
                    incommingDebugEntry = incommingDebugEntry.Substring(incommingDebugEntry.IndexOf(',') + 1);  // Get rid of ####,
                    Pid = int.Parse(possiblePid);
                } catch (FormatException) {
                    //Bilge.Dump(ex, "This MUST be a invalidformatException");
                    // ok forget it
                    Pid = incommingPid;
                }
            } else {
                // Ok we couldnt find a comma therefore its unlikely to be in the format ####,log
                Pid = incommingPid;
            }
        } else { // End if incomming Pid ==-1, else they passed a valid pid so we just use that
            Pid = incommingPid;
        }
        // Now do the other two parts
        DebugEntry = incommingDebugEntry; assignedIndex = gIndex;
    }

    internal string GetDiagnosticStringData() {
        string result = "NTA event for PID: " + Pid.ToString() + "\r\n";
        result += "Entry: " + DebugEntry + "\r\n";
        result += "Index: " + assignedIndex.ToString() + "\r\n";
        result += viewData.GetDiagnosticStringData();
        return result;
    }


}