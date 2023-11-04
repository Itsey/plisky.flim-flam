namespace Plisky.FlimFlam.Interfaces {

    public interface IProvideEntryStores {

        EventEntryStore GetEventEntryStore(IFilterProvider ifp);
    }
}