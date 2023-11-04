using Plisky.Diagnostics.FlimFlam;

namespace Plisky.FilmFlam.Interfaces {

    /// <summary>
    /// Responsible for representing importable data to the importer.  The primary importer calls through this interface to get data from the importer.
    /// </summary>
    public interface ISupportImporting {
        bool HasData { get; set; }

        RawApplicationEvent GetNextEvent();
    }
}