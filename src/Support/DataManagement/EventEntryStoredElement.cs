namespace Plisky.FlimFlam {
    using Plisky.Diagnostics.FlimFlam;

    public class EventEntryStoredElement {

        public EventEntryStoredElement(SingleOriginEvent ee, EventEntryStoredElement next, EventEntryStoredElement previous) {
            Navigation = new EventEntryNavigator(next, previous);
            Entry = ee;
        }

        public SingleOriginEvent Entry { get; set; }
        public EventEntryNavigator Navigation { get; set; }
    }
}