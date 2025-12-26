using System.Collections.Generic;
using System.Linq;
using nadena.dev.modular_avatar.core;
using nadena.dev.ndmf.runtime;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [AddComponentMenu("Modular Avatar EX/MAEX Icon Generator")]
    [DisallowMultipleComponent]
    public class ModularAvatarExtensionsIconGenerator : ModularAvatarExtensionsIconGeneratorBase
    {
        [SerializeField] private ModularAvatarObjectToggle? modularAvatarObjectToggle;
        [SerializeField] private ModularAvatarShapeChanger? modularAvatarShapeChanger;

        protected override void Check()
        {
            if (modularAvatarObjectToggle != null)
            {
                var objectsHash2 = GetListHash(modularAvatarObjectToggle.Objects.Select(to =>
                    RuntimeUtil.AvatarRootPath(to.Object.Get(this))));
                if (objectsHash2 != objectsHash) OnChange();
            }

            base.Check();
        }

        protected override void OnChange()
        {
            modularAvatarObjectToggle = GetComponent<ModularAvatarObjectToggle>();
            modularAvatarShapeChanger = GetComponent<ModularAvatarShapeChanger>();
            base.OnChange();
        }

        protected override List<GameObject> GetAllObjects()
        {
            if (modularAvatarMenuItem == null) return new();
            if (modularAvatarMenuItem.PortableControl.Type != PortableControlType.SubMenu)
                return modularAvatarObjectToggle == null
                    ? new()
                    : modularAvatarObjectToggle.Objects.Select(to => to.Object.Get(this)).Where(go => go != null)
                        .ToList();

            var allSubModularAvatarObjectToggles =
                modularAvatarMenuItem.GetComponentsInChildren<ModularAvatarObjectToggle>(true);
            return allSubModularAvatarObjectToggles
                .SelectMany(toggle => toggle.Objects.Select(to => to.Object.Get(this)).Where(go => go != null)).ToList();
        }

        protected override List<ShapeKeyData> GetAllShapeKeyDatas()
        {
            if (modularAvatarShapeChanger == null) return new();
            return modularAvatarShapeChanger.Shapes.Where(x => x.Object.Get(this) != null)
                .Select(x =>
                {
                    var go = x.Object.Get(this);
                    return new ShapeKeyData
                    {
                        gameObject = go,
                        shapeKeyName = x.ShapeName,
                        value = x.ChangeType == ShapeChangeType.Set ? x.Value : 100f
                    };
                }).ToList();
        }
    }
}