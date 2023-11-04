using System;

namespace Plisky.FlimFlam { 

    internal class TimeInstanceView {
        internal DateTime EnterTime = DateTime.MaxValue;
        internal DateTime ExitTime = DateTime.MaxValue;
        internal string InstanceName;
        internal string SinkName;

        internal TimeSpan Elaspsed {
            get {
                if (!this.Valid) {
                    return TimeSpan.MaxValue;
                }
                return ExitTime - EnterTime;
            }
        }

        internal bool Valid {
            get {
                return ((EnterTime != DateTime.MaxValue) && (ExitTime != DateTime.MaxValue));
            }
        }
    }
}