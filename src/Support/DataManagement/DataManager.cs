namespace Plisky.FlimFlam {

    using System;
    using System.Collections.Generic;
    using Plisky.Diagnostics.FlimFlam;
    using Plisky.FlimFlam.Interfaces;

    public class DataManager : IRecieveEvents {
        private readonly IMakeEventEntryStores factory;
        private readonly EventEntryStore primary;

        public DataManager(IMakeEventEntryStores eesf) {
            factory = eesf;
            primary = eesf.GetNewEventEntryStore();
        }

        public EventEntryStore GetEventEntries(IFilterProvider filter) {
            if (filter == null) {
                // TODO: Exception Handling
                throw new InvalidOperationException("DEV - a filter must be provided");
            }
            var ees = factory.GetNewEventEntryStore();
            foreach (var v in primary.GetEntries()) {
                if (filter.IncludeEvent(v)) {
                    ees.AddEntry(v);
                }
            }
            return ees;
        }

        #region IRecieveEvents Members

        public void AddEvent(SingleOriginEvent soe) {
            primary.AddEntry(soe);
        }

        public void AddEvent(IEnumerable<SingleOriginEvent> soe) {
            foreach (var e in soe) {
                primary.AddEntry(e);
            }
        }

        #endregion IRecieveEvents Members

        internal SingleOriginEvent GetEntryByOffset(int i) {
            throw new NotImplementedException();
        }

        internal EventViewProvider GetEventViewProvider(IFilterProvider ifp) {
            var ee = new EventEntryStoreFactory().GetNewEventEntryStore();
            foreach (var v in primary.GetEntries()) {
                if (ifp.IncludeEvent(v)) {
                    ee.AddEntry(v);
                }
            }
            return new EventViewProvider(ee);
        }
    }
}