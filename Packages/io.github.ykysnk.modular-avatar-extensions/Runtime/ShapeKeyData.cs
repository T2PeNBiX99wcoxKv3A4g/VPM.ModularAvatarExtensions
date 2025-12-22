using System;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [Serializable]
    public struct ShapeKeyData : IEquatable<ShapeKeyData>
    {
        public GameObject gameObject;
        public string shapeKeyName;
        public float value;

        public bool Equals(ShapeKeyData other) => gameObject == other.gameObject && shapeKeyName == other.shapeKeyName &&
                                                  Mathf.Approximately(value, other.value);

        public override bool Equals(object? obj) => obj is ShapeKeyData other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(gameObject, shapeKeyName, value);
    }
}