using System;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [Serializable]
    public class ShapeKeyData : IEquatable<ShapeKeyData>
    {
        [SerializeField] private GameObject gameObject;
        [SerializeField] private string shapeKeyName;
        [SerializeField] private float value;

        public ShapeKeyData(GameObject gameObject, string shapeKeyName, float value)
        {
            this.gameObject = gameObject;
            this.shapeKeyName = shapeKeyName;
            this.value = value;
        }

        public GameObject GameObject => gameObject;
        public string ShapeKeyName => shapeKeyName;
        public float Value => value;

        public bool Equals(ShapeKeyData other) => gameObject == other.gameObject && shapeKeyName == other.shapeKeyName &&
                                                  value.Equals(other.value);

        public override bool Equals(object? obj) => obj is ShapeKeyData other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(GameObject, ShapeKeyName, Value);
    }
}