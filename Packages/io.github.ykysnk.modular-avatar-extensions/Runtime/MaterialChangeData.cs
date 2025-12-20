using System;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [Serializable]
    public struct MaterialChangeData : IEquatable<MaterialChangeData>
    {
        public int materialIndex;
        public Material? material;

        public MaterialChangeData(int materialIndex, Material? material)
        {
            this.materialIndex = materialIndex;
            this.material = material;
        }

        public bool Equals(MaterialChangeData other) => materialIndex == other.materialIndex;

        public override bool Equals(object? obj) => obj is MaterialChangeData other && Equals(other);

        public override int GetHashCode() => materialIndex.GetHashCode();
    }
}