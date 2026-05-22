using System.Collections.Generic;
using System.Linq;
using io.github.ykysnk.ModularAvatarExtensions.ExtraData;
using nadena.dev.modular_avatar.core;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [AddComponentMenu("Modular Avatar EX/MAEX Icon Generator Of Reference")]
    [DisallowMultipleComponent]
    public class ModularAvatarExtensionsIconGeneratorOfReference : ModularAvatarExtensionsIconGeneratorBase
    {
        [SerializeField] private List<AvatarObjectReference> avatarObjectReferences = new();

        protected override List<GameObject> GetAllObjects() =>
            avatarObjectReferences.ConvertAll(ao => ao.Get(this)).Where(go => go != null).ToList();

        protected override List<ShapeKeyData> GetAllShapeKeyDatas()
        {
            if (!TryGetComponent<ExtraDataWithShapeKey>(out var iconGeneratorOfReferenceWithShapeKey))
                return new();
            var gameObjects = avatarObjectReferences.ConvertAll(ao => ao.Get(this)).Where(go => go != null);
            return iconGeneratorOfReferenceWithShapeKey.ShapeKeyValues.Select(x =>
                (gameObject: x.reference.Get(this), x.shapeKeyName, x.value)).Where(x =>
            {
                if (x.gameObject == null || !gameObjects.Contains(x.gameObject)) return false;
                if (!x.gameObject.TryGetComponent<SkinnedMeshRenderer>(out var skinnedMeshRenderer)) return false;
                var mesh = skinnedMeshRenderer.sharedMesh;
                var shapeNames = Enumerable.Range(0, mesh.blendShapeCount)
                    .Select(y => mesh.GetBlendShapeName(y))
                    .ToList();
                return shapeNames.Contains(x.shapeKeyName);
            }).Select(x => new ShapeKeyData(x.gameObject, x.shapeKeyName, x.value)).ToList();
        }
    }
}