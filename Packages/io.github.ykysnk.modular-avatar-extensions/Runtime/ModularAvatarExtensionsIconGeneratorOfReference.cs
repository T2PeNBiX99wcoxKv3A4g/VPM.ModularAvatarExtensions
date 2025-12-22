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

        protected override List<GameObject> GetAllObjects() =>
            avatarObjectReferences.ConvertAll(ao => ao.Get(this)).Where(go => go != null).ToList();

        protected override List<ShapeKeyData> GetAllShapeKeyDatas() => new();
    }
}