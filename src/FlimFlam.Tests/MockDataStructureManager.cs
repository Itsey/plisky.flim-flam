using Plisky.FlimFlam;

namespace Plisky.FlimFlam.Tests; 
internal class MockDataStructureManager : DataStructureManager {

    public int AlertRecordsRecieved { get; set; } = 0;
    public int EventEntriesRecieved { get; internal set; }

    public override int PlaceAlertInoDataStructure(AlertEntry aa) {
        AlertRecordsRecieved++;
        return AlertRecordsRecieved;
    }

    public override int PlaceNewEventIntoDataStructure(EventEntry newEvt, int thePid, string eventMachineName) {
        EventEntriesRecieved++;
        return EventEntriesRecieved;
    }
}