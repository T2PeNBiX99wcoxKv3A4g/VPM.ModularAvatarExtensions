using System.Collections.Generic;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions.ExtraData
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Modular Avatar EX/Reference Extra Datas/MAEX Extra Data - With Shape Key")]
    public class ExtraDataWithShapeKey : AvatarMaexComponent, IExtraData
    {
        [SerializeField] private List<ShapeKeyValueReference> shapeKeyValues = new();

        public List<ShapeKeyValueReference> ShapeKeyValues => shapeKeyValues;
    }
}