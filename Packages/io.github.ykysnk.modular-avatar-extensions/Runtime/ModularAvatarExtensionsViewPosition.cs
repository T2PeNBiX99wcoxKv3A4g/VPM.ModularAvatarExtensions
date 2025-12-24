#if MAEX_VRCSDK3_BASE
using io.github.ykysnk.utils;
using io.github.ykysnk.utils.Extensions;
using io.github.ykysnk.utils.NonUdon;
using nadena.dev.ndmf.runtime;
using UnityEngine;
using VRC.SDKBase;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Modular Avatar EX/MAEX View Position")]
    [ExecuteInEditMode]
    public class ModularAvatarExtensionsViewPosition : AvatarMaexComponent
    {
        public VRC_AvatarDescriptor? avatarDescriptor;
        [SerializeField] private BooleanVector3 isLock = BooleanVector3.True;

        private void Update()
        {
            if (!gameObject.IsSceneObject() || Utils.IsPlaying) return;
            if (avatarDescriptor == null) return;
            var oldPosition = transform.position;
            var newPosition = oldPosition;

            if (isLock.x) newPosition.x = avatarDescriptor.ViewPosition.x;
            if (isLock.y) newPosition.y = avatarDescriptor.ViewPosition.y;
            if (isLock.z) newPosition.z = avatarDescriptor.ViewPosition.z;

            if (oldPosition == newPosition) return;
            transform.position = newPosition;
        }

        protected override void OnChange()
        {
            var rootObj = RuntimeUtil.FindAvatarInParents(transform);
            if (!rootObj?.TryGetComponent<VRC_AvatarDescriptor>(out avatarDescriptor) ?? false)
                avatarDescriptor = null;
        }
    }
}
#endif