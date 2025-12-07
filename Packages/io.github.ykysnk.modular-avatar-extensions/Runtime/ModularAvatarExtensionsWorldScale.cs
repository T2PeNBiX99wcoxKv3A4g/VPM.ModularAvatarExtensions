using io.github.ykysnk.utils;
using io.github.ykysnk.utils.Extensions;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Modular Avatar EX/MAEX World Scale")]
    [ExecuteInEditMode]
    public class ModularAvatarExtensionsWorldScale : AvatarMaexComponent
    {
        public bool editorOnly;

        private void Update()
        {
            if (!gameObject.scene.IsValid() || Utils.IsInPrefab() || Utils.IsPlaying()) return;
            var setLocalScale = transform.GetLocalScaleFollowWorldScale().Round(2);
            if (transform.localScale == setLocalScale) return;
            transform.localScale = setLocalScale;
        }
    }
}