using System;
using System.Linq;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor.Animation
{
    internal class ParamPassData : IEquatable<ParamPassData>
    {
        internal readonly ParamData[] ParamDatas;
        internal readonly bool Reverse;

        internal ParamPassData(ParamData[] paramDatas, bool reverse)
        {
            ParamDatas = paramDatas;
            Reverse = reverse;
        }

        public bool Equals(ParamPassData other) =>
            ParamDatas.SequenceEqual(other.ParamDatas) && Reverse == other.Reverse;

        public override bool Equals(object? obj) => obj is ParamPassData other && Equals(other);

        public override int GetHashCode() =>
            HashCode.Combine(ParamDatas.Select(x => x.GetHashCode()).Aggregate((x, y) => x ^ y), Reverse);
    }
}