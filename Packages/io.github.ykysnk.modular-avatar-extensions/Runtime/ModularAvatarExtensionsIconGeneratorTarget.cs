using System.Collections;
using io.github.ykysnk.utils;
using nadena.dev.modular_avatar.core;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

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
#if UNITY_EDITOR
            Undo.RecordObject(modularAvatarMenuItem, "Change Icon");
#endif
            modularAvatarMenuItem.PortableControl.Icon = iconGenerator.IconTexture;
        }

        private IEnumerator CheckLoop()
        {
            while (enabled && gameObject.activeSelf)
            {
                if (gameObject.scene.IsValid() && !Utils.IsInPrefab())
                    Check();
                yield return new WaitForSeconds(2f);
            }
        }

        protected override void OnChange()
        {
            if (!gameObject.activeSelf || !gameObject.scene.IsValid() || Utils.IsInPrefab()) return;
            modularAvatarMenuItem = GetComponent<ModularAvatarMenuItem>();
        }
    }
}