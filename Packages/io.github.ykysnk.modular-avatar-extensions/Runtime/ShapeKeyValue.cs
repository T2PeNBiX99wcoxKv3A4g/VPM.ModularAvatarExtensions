using System;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    public readonly struct ShapeKeyValue : IEquatable<ShapeKeyValue>
    {
        public readonly int ShapeKeyIndex;
        public readonly float Value;

        public ShapeKeyValue(GameObject gameObject, string shapeKeyName, float value)
        {
            if (gameObject.TryGetComponent<SkinnedMeshRenderer>(out var skinnedMeshRenderer))
                ShapeKeyIndex = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(shapeKeyName);
            else
                ShapeKeyIndex = -1;
            Value = value;
        }

        public bool Equals(ShapeKeyValue other) => ShapeKeyIndex == other.ShapeKeyIndex && Value.Equals(other.Value);

        public override bool Equals(object? obj) => obj is ShapeKeyValue other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(ShapeKeyIndex, Value);
    }
}