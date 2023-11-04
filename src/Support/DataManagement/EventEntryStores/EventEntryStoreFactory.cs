using Plisky.FlimFlam.Interfaces;

namespace Plisky.FlimFlam {

    public class EventEntryStoreFactory : IMakeEventEntryStores {

        #region IMakeEventEntryStores Members

        public EventEntryStore GetNewEventEntryStore() {
            return new EESimpleImpl();
        }

        #endregion IMakeEventEntryStores Members
    }
}