using System;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [Serializable]
    public class MaterialChangeData : IEquatable<MaterialChangeData>
    {
        [SerializeField] private int materialIndex;
        [SerializeField] private Material? material;

        public MaterialChangeData(int materialIndex, Material? material)
        {
            this.materialIndex = materialIndex;
            this.material = material;
        }

        public int MaterialIndex => materialIndex;
        public Material? Material => material;

        public bool Equals(MaterialChangeData other) => materialIndex == other.materialIndex;

        public override bool Equals(object? obj) => obj is MaterialChangeData other && Equals(other);

        public override int GetHashCode() => MaterialIndex.GetHashCode();
    }
}