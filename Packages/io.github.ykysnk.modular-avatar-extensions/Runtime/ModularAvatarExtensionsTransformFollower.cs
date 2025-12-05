using io.github.ykysnk.utils;
using io.github.ykysnk.utils.Extensions;
using io.github.ykysnk.utils.NonUdon;
using nadena.dev.modular_avatar.core;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Modular Avatar EX/MAEX Transform Follower")]
    [ExecuteInEditMode]
    public class ModularAvatarExtensionsTransformFollower : AvatarMaexComponent
    {
        public bool isLock;
        public AvatarObjectReference? reference;
        public Vector3 positionOffset = Vector3.zero;
        public Vector3 rotationOffset = Vector3.zero;
        public Vector3 scaleOffset = Vector3.zero;
        public BooleanVector3 isLockPosition = BooleanVector3.True;
        public BooleanVector3 isLockRotation = BooleanVector3.True;
        public BooleanVector3 isLockScale = BooleanVector3.True;

        private void Update()
        {
            if (!gameObject.scene.IsValid() || Utils.IsInPrefab() || Utils.IsPlaying() || !isLock ||
                string.IsNullOrEmpty(reference?.referencePath)) return;

            var obj = reference?.Get(this);
            if (obj == null) return;

            var objPosition = obj.transform.TransformPointUnscaled(positionOffset);
            var oldPosition = transform.position;
            var newPosition = oldPosition;

            if (isLockPosition.x) newPosition.x = objPosition.x;
            if (isLockPosition.y) newPosition.y = objPosition.y;
            if (isLockPosition.z) newPosition.z = objPosition.z;

            if (newPosition != oldPosition) transform.position = newPosition;

            var objRotation = (obj.transform.rotation * Quaternion.Euler(rotationOffset)).eulerAngles;
            var oldRotation = transform.eulerAngles;
            var newRotation = oldRotation;

            if (isLockRotation.x) newRotation.x = objRotation.x;
            if (isLockRotation.y) newRotation.y = objRotation.y;
            if (isLockRotation.z) newRotation.z = objRotation.z;

            if (newRotation != oldRotation) transform.eulerAngles = newRotation;

            var objScale = Vector3.Scale(obj.transform.localScale, scaleOffset);
            var oldScale = transform.localScale;
            var newScale = oldScale;

            if (isLockScale.x) newScale.x = objScale.x;
            if (isLockScale.y) newScale.y = objScale.y;
            if (isLockScale.z) newScale.z = objScale.z;

            if (newScale != oldScale) transform.localScale = newScale;
        }

        public void ActivateConstraint()
        {
            var obj = reference?.Get(this);
            if (obj == null) return;
            positionOffset = obj.transform.InverseTransformPointUnscaled(transform.position);
            // TODO: Rotation offset is work but weird
            rotationOffset = (Quaternion.Inverse(obj.transform.rotation) * transform.rotation).eulerAngles;
            scaleOffset = transform.lossyScale.Divide(obj.transform.lossyScale);
            isLock = true;
        }

        public void ZeroConstraint()
        {
            positionOffset = Vector3.zero;
            rotationOffset = Vector3.zero;
            scaleOffset = Vector3.zero;
            isLock = true;
        }
    }
}