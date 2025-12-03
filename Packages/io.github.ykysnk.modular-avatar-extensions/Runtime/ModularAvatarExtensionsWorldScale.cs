using io.github.ykysnk.utils;
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

            var setScale = Vector3.one;
            if (transform.parent != null) setScale = transform.parent.InverseTransformVector(Vector3.one);

            transform.localScale = setScale;
        }
    }
}