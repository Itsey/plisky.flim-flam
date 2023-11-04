using Plisky.Diagnostics.FlimFlam;

namespace Plisky.FlimFlam {

    /// <summary>
    /// Responsible for the interface that describes a filter.  An ability to include or exclude events based on certain
    /// criterial.
    /// </summary>
    public interface IFilterProvider {

        bool IncludeEvent(SingleOriginEvent evt);

        bool IncludeProcess(int originId);
    }
}