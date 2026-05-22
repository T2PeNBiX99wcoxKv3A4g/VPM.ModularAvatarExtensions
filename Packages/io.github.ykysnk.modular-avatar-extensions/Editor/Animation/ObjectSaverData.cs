using System;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor.Animation
{
    internal class ObjectSaverData : IEquatable<ObjectSaverData>
    {
        internal readonly GameObject GameObject;
        internal readonly string Path;

        internal ObjectSaverData(GameObject gameObject, string path)
        {
            GameObject = gameObject;
            Path = path;
        }

        public bool Equals(ObjectSaverData other) => GameObject == other.GameObject;

        public override bool Equals(object? obj) => obj is ObjectSaverData other && Equals(other);

        public override int GetHashCode() => GameObject.GetHashCode();
    }
}