using System.Collections.Generic;
using System.Linq;
using io.github.ykysnk.utils.Extensions;
using io.github.ykysnk.utils.NonUdon;
using nadena.dev.modular_avatar.core;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [AddComponentMenu("Modular Avatar EX/MAEX Icon Generator")]
    public class ModularAvatarExtensionsIconGenerator : ModularAvatarExtensionsIconGeneratorBase
    {
        [SerializeField] private ModularAvatarObjectToggle? modularAvatarObjectToggle;

        protected override void LateUpdate()
        {
            if (modularAvatarObjectToggle == null) return;
            var objectsHash2 =
                HashUtils.ComputeHash(
                    string.Join("|", modularAvatarObjectToggle.Objects.Select(to => to.Object.Get(this).FullName())),
                    HashUtils.HashType.SHA1);
            if (objectsHash2 != objectsHash) OnChange();
            base.LateUpdate();
        }

        protected override void OnChange()
        {
            modularAvatarObjectToggle = GetComponent<ModularAvatarObjectToggle>();
            base.OnChange();
        }

        protected override List<GameObject> GetAllObjects()
        {
            if (modularAvatarMenuItem == null) return new();
            if (modularAvatarMenuItem.PortableControl.Type != PortableControlType.SubMenu)
                return modularAvatarObjectToggle == null
                    ? new()
                    : modularAvatarObjectToggle.Objects.Select(to => to.Object.Get(this)).ToList();

            var allSubModularAvatarObjectToggles =
                modularAvatarMenuItem.GetComponentsInChildren<ModularAvatarObjectToggle>(true);
            return allSubModularAvatarObjectToggles.SelectMany(toggle => toggle.Objects.Select(to => to.Object.Get(this)))
                .ToList();
        }
    }
}