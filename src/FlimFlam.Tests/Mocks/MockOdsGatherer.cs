using Plisky.FlimFlam;

namespace Plisky.FlimFlam.Tests; 
internal class MockOdsGatherer : OdsProcessGatherer {
   public bool AllowStart { get; set; }
    public bool IsActive { get; set; } = false;


    public MockOdsGatherer(bool allowStart = true) { 
        AllowStart = allowStart;
    }

    public override bool StartOdsGathererProcess() {
        if (AllowStart) {
            IsActive = true;
            return true;
        } else {
            return false;
        }
    }

    public override bool StopOdsGathererProcess() {
        if (!IsActive) { return false; }

        IsActive = false;
        return true;
    }

}