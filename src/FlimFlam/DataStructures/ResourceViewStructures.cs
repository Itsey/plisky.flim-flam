namespace Plisky.FlimFlam { 

    internal struct ResourceProfile {
        internal ValueIdxPair[] Consumption;
        internal long HighestValue;
        internal long LowestValue;
        internal string Name;
    }

    internal struct ValueIdxPair {
        internal long GlobalIndex;
        internal long Value;

        internal ValueIdxPair(long idx, long val) {
            GlobalIndex = idx;
            Value = val;
        }
    }
}