using System.Collections;
using nadena.dev.modular_avatar.core;
using UnityEditor;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [AddComponentMenu("Modular Avatar EX/MAEX Icon Generator Target")]
    public class ModularAvatarExtensionsIconGeneratorTarget : AvatarMaexComponent
    {
        [SerializeField] private ModularAvatarMenuItem? modularAvatarMenuItem;
        [SerializeField] private ModularAvatarExtensionsIconGeneratorBase? iconGenerator;

        private void OnEnable() => StartCoroutine(CheckLoop());

        private void Check()
        {
            if (iconGenerator == null || modularAvatarMenuItem == null) return;
            if (iconGenerator.IconTexture == null ||
                modularAvatarMenuItem.PortableControl.Icon == iconGenerator.IconTexture) return;
            Undo.RecordObject(modularAvatarMenuItem, "Change Icon");
            modularAvatarMenuItem.PortableControl.Icon = iconGenerator.IconTexture;
        }

        private IEnumerator CheckLoop()
        {
            while (enabled && gameObject.activeSelf)
            {
                Check();
                yield return new WaitForSeconds(2f);
            }
        }

        protected override void OnChange()
        {
            modularAvatarMenuItem = GetComponent<ModularAvatarMenuItem>();
        }
    }
}