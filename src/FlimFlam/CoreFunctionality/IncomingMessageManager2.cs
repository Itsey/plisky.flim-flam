
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plisky.Diagnostics;
using Plisky.Diagnostics.FlimFlam;
using Plisky.Plumbing;

namespace Plisky.FlimFlam;

public class IncomingMessageManager2 : IncomingMessageManager {
    protected Bilge b = new Bilge("core-messagemgr");
    protected Hub h;
    protected Feature odsFeature;
    protected Feature importerFeature;
    protected OdsProcessGatherer odsProcessGatherer;
    protected SystemdataMessage sysdata = new SystemdataMessage();
    protected DataStructureManager dsm;

    /// <summary>
    /// This is configured from the mex options.  If it is true the duplicate strings from the same 
    /// source are deleted.
    /// </summary>
    public bool RemoveDupesOnImport { get; set; } = true;
    // TODO : Hook into options
    // TODO : Consider dupe count on the remaining one

    public IncomingMessageManager2(Hub hb, OdsProcessGatherer pg, DataStructureManager dm) : base() {
        odsFeature = Feature.GetFeatureByName("Bilge-OdsOOP") ?? new Feature("Bilge-OdsOOP", false);
        importerFeature = Feature.GetFeatureByName("FF-NewImport") ?? new Feature("FF-NewImport", false);
        odsProcessGatherer = pg;
        h = hb;
        dsm = dm;
        h.Launch(sysdata);
        if (!sysdata.WasHandled) {
            b.Warning.Log("System data message not handled, machinename wrong");
        }
        PrepareNewImportStructure();
    }

    private void PrepareNewImportStructure() {
        if (importerFeature.Active) {
            newImporter ??= new ImportParser(store);
            dp ??= new DataParser(store, newImporter, comparer);
        }
    }

    public override void ActivateODSGatherer() {
        if (odsFeature.Active) {
            if (odsProcessGatherer.StartOdsGathererProcess()) {
                h.Launch(new UserNotificationMessage() {
                    Message = UserMessages.ODSListenerTurnedOn,
                    MessageType = UserMessageType.InformationMessage
                });
            } else {
                h.Launch(new UserNotificationMessage() {
                    Message = UserMessages.ODSListenerTurnedOn,
                    MessageType = UserMessageType.WarningMessage,
                    Parameter = "ODS Failed to start, possibly already active?"
                });
            }

        } else {
            base.ActivateODSGatherer();
        }
    }

    public override void DeactivateODSGatherer() {
        if (odsFeature.Active) {
            odsProcessGatherer.StopOdsGathererProcess();
        } else {
            base.DeactivateODSGatherer();
        }
    }

    public virtual void ProcessNextStoredMessage(bool reporting) {
        var msgParser = new ImportParser(ois);


        do {
            if (shutdownRequested) { break; }

            var nextEvent = GetNextEvent();
            if (nextEvent == null) {
                continue;
            }

            // DISABLED FOR UNIT TESTS >> MexCore.TheCore.LogEveryting("IncommingMessageManager", nextEvent.messageString);

            RemoveDuplicatesOnImport2(nextEvent);


            var rae = new RawApplicationEvent() {
                ArrivalTime = nextEvent.timeRecieved,
                OriginId = store.GetOriginIdentity(nextEvent.machineName, nextEvent.pid.ToString()),
                Machine = nextEvent.machineName,
                Process = nextEvent.pid.ToString(),
                Text = nextEvent.messageString
            };

            var parsed = msgParser.Parse(rae);
            if (parsed ==null) {
                // TODO Error Handling, via hub?
            }

            b.Assert.NotNull(parsed, "Can not process a failed message, code should not have been allowed to fall through here.");


            dsm.PlaceNewEventIntoDataStructure(new EventEntry(1), nextEvent.pid, sysdata.MachineName);

        } while (incommingMsgQueue.Count > 0);
    }


    /// <summary>
    /// Using options to determine whether this behaviour is desired remove duplicate entries by throwing them away before
    /// importing the current entry. Note this can be used when two import methods are generating the same messages.
    /// </summary>
    /// <param name="nextEvent">Entry to use to look for duplicates.</param>
    protected virtual void RemoveDuplicatesOnImport2(IncomingEventStore nextEvent) {

        if (RemoveDupesOnImport) {
            bool dupeFound = true;
            while (dupeFound && (incommingMsgQueue.Count > 0)) {
                var potentialNext = (IncomingEventStore)incommingMsgQueue.Peek();
                if (potentialNext != null) {
                    dupeFound = potentialNext.Equals(nextEvent);
                    if (dupeFound) {
                        if (incommingMsgQueue.Count > 0) {
                            incommingMsgQueue.Dequeue();
                        }
                    }
                }
            }
        }
    }

    private IncomingEventStore? GetNextEvent() {

        if (incommingMsgQueue.Count == 0) {
            return null;
        }

        var nextEvent = (IncomingEventStore)incommingMsgQueue.Dequeue();
        if (string.IsNullOrEmpty(nextEvent.messageString)) {
            return null;
        }
        return nextEvent;
    }

    public override void ProcessNextStoredMessage(bool doAll, bool reporting) {
        // COMPAT MODE - not required. 
        // doAll no longer required it was always true.

        if (!importerFeature.Active) {
            base.ProcessNextStoredMessage(doAll, reporting);
        } else {
            ProcessNextStoredMessage(reporting);
        }


    }


    /// <summary>
    /// This method is called to queue a new debug string message ready for processing.  This method should return very rapidly so as not to
    /// hold up any of the gatherer methods and is the preferred way of adding data entries into the mex data structures.
    /// </summary>
    /// <param name="msg">The text component of the message to be added</param>
    /// <param name="pid">The pid if the pid is supported by this gatherer, otherwise set this value to -1</param>
    public override void AddIncomingMessage(InternalSource source, string msg, int processId) {
        incommingMsgQueue.Enqueue(new IncomingEventStore(source, sysdata.MachineName, msg, processId, dsm.GetNextGlobalIndex()));
    }
}

