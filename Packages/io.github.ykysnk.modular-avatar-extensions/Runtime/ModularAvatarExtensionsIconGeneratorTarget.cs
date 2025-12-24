using System.Collections;
using io.github.ykysnk.utils;
using io.github.ykysnk.utils.Extensions;
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
#if UNITY_EDITOR
            if (iconGenerator == null || modularAvatarMenuItem == null) return;
            if (iconGenerator.IconTexture == null ||
                modularAvatarMenuItem.PortableControl.Icon == iconGenerator.IconTexture) return;

            Undo.RecordObject(modularAvatarMenuItem, "Change Icon");
            modularAvatarMenuItem.PortableControl.Icon = iconGenerator.IconTexture;
            EditorUtility.SetDirty(modularAvatarMenuItem);
#endif
        }

        private IEnumerator CheckLoop()
        {
            while (enabled && gameObject.activeSelf)
            {
                if (gameObject.IsSceneObject() && !Utils.IsPlaying)
                    Check();
                yield return new WaitForSeconds(2f);
            }
        }

        protected override void OnChange()
        {
            if (!gameObject.activeSelf || !gameObject.IsSceneObject()) return;
            modularAvatarMenuItem = GetComponent<ModularAvatarMenuItem>();
        }
    }
}