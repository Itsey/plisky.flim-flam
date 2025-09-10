namespace Plisky.FlimFlam;

/// <summary>
/// Holds additional data available for each screen to allow screen specific customisations.
/// </summary>
internal class AdditionalData {
    private double excludeTimingsGreaterThan;
    private double excludeTimingsLessThan;
    private bool useExclusionFilters;

    public double ExcludeTimingsGreaterThan {
        get { return this.excludeTimingsGreaterThan; }
        set { this.excludeTimingsGreaterThan = value; }
    }

    public double ExcludeTimingsLessThan {
        get { return this.excludeTimingsLessThan; }
        set { this.excludeTimingsLessThan = value; }
    }

    public bool UseExclusionFilters {
        get { return this.useExclusionFilters; }
        set { this.useExclusionFilters = value; }
    }
}