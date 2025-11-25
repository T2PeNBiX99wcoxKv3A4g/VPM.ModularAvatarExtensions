using nadena.dev.ndmf.runtime;
using UnityEditor;
using UnityEngine;
using VRC.SDKBase;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Modular Avatar EX/MAEX View Position")]
    public class ModularAvatarExtensionsViewPosition : AvatarMaexComponent
    {
        public VRC_AvatarDescriptor? avatarDescriptor;

        protected override void OnChange()
        {
            var rootObj = RuntimeUtil.FindAvatarInParents(transform);
            rootObj?.TryGetComponent<VRC_AvatarDescriptor>(out avatarDescriptor);
        }

#if UNITY_EDITOR
        public override void OnInspectorGUI()
        {
            if (avatarDescriptor == null) return;
            Undo.RecordObject(transform, "");
            transform.position = avatarDescriptor.ViewPosition;
        }
#endif
    }
}