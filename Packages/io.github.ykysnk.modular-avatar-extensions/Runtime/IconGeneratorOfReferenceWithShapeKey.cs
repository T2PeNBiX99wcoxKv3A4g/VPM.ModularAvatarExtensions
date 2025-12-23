using System.Collections.Generic;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Modular Avatar EX/Reference Extra Datas/MAEX Extra Data - With Shape Key")]
    public class IconGeneratorOfReferenceWithShapeKey : AvatarMaexComponent, IReferenceExtraData
    {
        [SerializeField] private List<ShapeKeyValueReference> shapeKeyValues = new();

        public List<ShapeKeyValueReference> ShapeKeyValues => shapeKeyValues;
    }
}