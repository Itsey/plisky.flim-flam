using Plisky.Diagnostics.FlimFlam;

namespace Plisky.FlimFlam.Interfaces {

    public interface IParseData {

        SingleOriginEvent AddRawEvent(RawApplicationEvent rae);

        SingleOriginEvent[] AddRawEvent(RawApplicationEvent[] rae);
    }
}