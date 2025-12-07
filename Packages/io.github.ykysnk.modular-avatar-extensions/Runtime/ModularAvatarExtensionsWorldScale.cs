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
            if (transform.lossyScale.Round(2).Equals(Vector3.one)) return;
            transform.SetLossyScale(Vector3.one);
        }
    }
}