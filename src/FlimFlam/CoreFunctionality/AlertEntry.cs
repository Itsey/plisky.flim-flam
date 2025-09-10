namespace Plisky.FlimFlam;
using System;

public class AlertEntry {
    public string Message { get; set; }
    public DateTime OccuredAt { get; set; }
    public string Secondary { get; set; }

    public override string ToString() {
        return Message;
    }
}