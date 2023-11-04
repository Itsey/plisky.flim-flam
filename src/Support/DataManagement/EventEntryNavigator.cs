namespace Plisky.FlimFlam {

    public class EventEntryNavigator {

        public EventEntryNavigator(EventEntryStoredElement next, EventEntryStoredElement previous) {
            Next = next;
            Previous = previous;
        }

        public EventEntryStoredElement Next { get; set; }
        public EventEntryStoredElement Previous { get; set; }
    }
}