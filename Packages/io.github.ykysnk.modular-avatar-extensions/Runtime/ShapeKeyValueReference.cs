using System;
using nadena.dev.modular_avatar.core;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [Serializable]
    public class ShapeKeyValueReference
    {
        public AvatarObjectReference reference = new();
        public string shapeKeyName = "";
        public float value;
    }
}