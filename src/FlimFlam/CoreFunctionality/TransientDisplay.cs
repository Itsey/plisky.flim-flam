//using Plisky.Diagnostics;
using OldFlimflam;

namespace Plisky.FlimFlam;

internal class TransientDisplay {

    public TransientDisplay() {
        Text = Title = string.Empty;
        TType = TransientType.Undefined;
    }

    public string Text { get; internal set; }
    public string Title { get; internal set; }
    public TransientType TType { get; internal set; }

    public override string ToString() {
        return Title;
    }
}