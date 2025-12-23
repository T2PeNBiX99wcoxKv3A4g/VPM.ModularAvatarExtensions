using System.Collections.Generic;
using System.Linq;
using nadena.dev.modular_avatar.core;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [AddComponentMenu("Modular Avatar EX/MAEX Icon Generator Of Referance")]
    public class ModularAvatarExtensionsIconGeneratorOfReference : ModularAvatarExtensionsIconGeneratorBase
    {
        [SerializeField] private List<AvatarObjectReference> avatarObjectReferences = new();
        [SerializeField] private List<ObjectReferenceData> objectReferenceDatas = new();

        protected override void OnChange()
        {
            if (avatarObjectReferences.Count < 1) return;
            objectReferenceDatas = avatarObjectReferences.Select(x => new ObjectReferenceData(x, new())).ToList();
            avatarObjectReferences.Clear();
        }

        protected override List<GameObject> GetAllObjects() =>
            objectReferenceDatas.ConvertAll(ao => ao.reference.Get(this)).Where(go => go != null).ToList();

        protected override List<ShapeKeyData> GetAllShapeKeyDatas() =>
            (from referenceData in objectReferenceDatas
            from shapeKeyValue in referenceData.shapeKeyValues
            select new ShapeKeyData
            {
                gameObject = referenceData.reference.Get(this),
                shapeKeyName = shapeKeyValue.shapeKeyName,
                value = shapeKeyValue.value
            }).ToList();
    }
}