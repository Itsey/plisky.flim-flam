namespace Plisky.FlimFlam {

    internal class EventViewProvider : ViewProvider {
        private EventEntryStore ees;

        public EventViewProvider(EventEntryStore ees) {
            // TODO: Complete member initialization
            this.ees = ees;
        }

        public long TotalEvents { get; set; }
    }
}