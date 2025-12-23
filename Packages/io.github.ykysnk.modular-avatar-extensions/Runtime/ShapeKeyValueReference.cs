using System;
using nadena.dev.modular_avatar.core;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [Serializable]
    public struct ShapeKeyValueReference
    {
        public AvatarObjectReference reference;
        public string shapeKeyName;
        public float value;
    }
}