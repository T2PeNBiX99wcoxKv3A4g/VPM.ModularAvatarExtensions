using System;
using System.Collections.Generic;
using nadena.dev.modular_avatar.core;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [Serializable]
    public struct ObjectReferenceData
    {
        public AvatarObjectReference reference;
        public List<ShapeKeyValueReference> shapeKeyValues;

        public ObjectReferenceData(AvatarObjectReference reference, List<ShapeKeyValueReference> shapeKeyValues)
        {
            this.reference = reference;
            this.shapeKeyValues = shapeKeyValues;
        }
    }
}