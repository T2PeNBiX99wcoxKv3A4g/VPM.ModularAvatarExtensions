using System;
using System.Linq;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    internal readonly struct ParamPassData : IEquatable<ParamPassData>
    {
        internal readonly ParamData[] ParamDatas;
        internal readonly bool Reverse;
        internal readonly bool HighPriority;

        internal ParamPassData(ParamData[] paramDatas, bool reverse, bool highPriority)
        {
            ParamDatas = paramDatas;
            Reverse = reverse;
            HighPriority = highPriority;
        }

        public bool Equals(ParamPassData other) =>
            ParamDatas.SequenceEqual(other.ParamDatas) && Reverse == other.Reverse && HighPriority == other.HighPriority;

        public override bool Equals(object? obj) => obj is ParamPassData other && Equals(other);

        public override int GetHashCode() =>
            HashCode.Combine(ParamDatas.Select(x => x.GetHashCode()).Aggregate((x, y) => x ^ y), Reverse, HighPriority);
    }
}