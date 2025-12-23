using System;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [Serializable]
    public struct ShapeKeyValueReference : IReferenceExtraData
    {
        public string shapeKeyName;
        public float value;
    }
}