////using Plisky.Plumbing.Legacy;
using System;
using System.Collections;
using Plisky.Plumbing;

namespace Plisky.FlimFlam;

/// <summary>
/// Summary description for CacheSupportManager.
/// </summary>
public class CacheSupportManager {
    private string thisMachineNameCache = Environment.MachineName;

    public CacheSupportManager() {
        //Bilge.Log("MexViewer::CacheSupportManager --> Create () ");
        truncateCache = new Hashtable();
    }

    internal string MexRunningOnMachineName {
        get { return thisMachineNameCache; }
    }

    internal string DiagnosticsText() {
        return "Cached Machine Name:" + thisMachineNameCache;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    internal void Shutdown() {
        //Bilge.Log("MexViewer::CacheSupportManager --> Shutdown requested.");
    }

    #region Truncation Cache System

    private Hashtable truncateCache;

    internal void CacheAdd_EventEntryExpectingTruncate(EventEntry ee, int pid, string machineName, string joinIdentifier) {
        string keyForAdd = pid.ToString() + machineName + joinIdentifier;
        if (!truncateCache.Contains(keyForAdd)) {
            truncateCache.Add(keyForAdd, ee);
        } else {
            //Bilge.Log("CacheSupportManager::CacheAdd_EventEntryExpectingTruncate --> Entry already cached");
        }
    }

    internal EventEntry CacheGet_EventEntryExpectingTruncate(int pid, string machineName, string joinIdentifier) {
        return CacheGet_EventEntryExpectingTruncate(pid, machineName, joinIdentifier, false);
    }

    internal EventEntry CacheGet_EventEntryExpectingTruncate(int pid, string machineName, string joinIdentifier, bool expire) {

        #region entry code

        //Bilge.Assert(pid >= 0, "Invalid pid passed to CacheGet_EventEntryExpectingTruncate");
        if (machineName == null) {
            //Bilge.Log("CacheSupportManager::CacheGet_EventEntryExpectingTruncate --> WARNING no machine name passed, will try to process");
            // TODO: FIX THIS!!!
            machineName = Environment.MachineName;
        }

        #endregion entry code

        string searchKey = pid.ToString() + machineName + joinIdentifier;
        EventEntry result = (EventEntry)truncateCache[searchKey];

        if (result == null) {
            // Its not cached, this should not actually be possible
            //Bilge.Log("CacheSupportManager::CacheGet_EventEntryExpectingTruncate WARNING--> Cache Miss on truncate should not be possible");
            //Bilge.Log("CacheSupportManager::CacheGet_EventEntryExpectingTruncate CORRECTION CODE ENGAGED --> Looking for truncation");
            TracedApplication ta = MexCore.TheCore.DataManager.GetKnownApplicationByPid(pid, machineName);
            if (ta != null) {
                // TA can be null if we really unluckily intercepted the tail half of a truncation.
                for (int i = ta.EventEntries.Count - 1; i >= 0; i--) {
                    if (ta.EventEntries[i].debugMessage.EndsWith(FlimFlamConstants.MESSAGETRUNCATE)) {
                        result = ta.EventEntries[i];
                        break;
                    }
                }
            }
        } else {
            // it was found in the cache, check whether were expiring it.
            if (expire) {
                truncateCache.Remove(searchKey);
            }
        }

        // Hmm it doesent exist at all, this also should not be possible.
        return result;
    }

    #endregion Truncation Cache System
}