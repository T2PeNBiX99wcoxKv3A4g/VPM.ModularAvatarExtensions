using io.github.ykysnk.utils;
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
        public bool isLock = true;
        public AvatarObjectReference? reference;
        public Vector3 positionOffset = Vector3.zero;
        public Vector3 rotationOffset = Vector3.zero;
        public Vector3 scaleOffset = Vector3.zero;
        public BooleanVector3 isLockPosition = BooleanVector3.True;
        public BooleanVector3 isLockRotation = BooleanVector3.True;
        public BooleanVector3 isLockScale;

        private void Update()
        {
            if (!gameObject.scene.IsValid() || Utils.IsInPrefab() || Utils.IsPlaying() || !isLock ||
                string.IsNullOrEmpty(reference?.referencePath)) return;

            var obj = reference?.Get(this);
            if (obj == null) return;

            var objPosition = obj.transform.position + positionOffset;
            var oldPosition = transform.position;
            var newPosition = oldPosition;

            if (isLockPosition.x) newPosition.x = objPosition.x;
            if (isLockPosition.y) newPosition.y = objPosition.y;
            if (isLockPosition.z) newPosition.z = objPosition.z;

            if (newPosition != oldPosition) transform.position = newPosition;

            var objRotation = obj.transform.eulerAngles + rotationOffset;
            var oldRotation = transform.eulerAngles;
            var newRotation = oldRotation;

            if (isLockRotation.x) newRotation.x = objRotation.x;
            if (isLockRotation.y) newRotation.y = objRotation.y;
            if (isLockRotation.z) newRotation.z = objRotation.z;

            if (newRotation != oldRotation) transform.eulerAngles = newRotation;

            var objLocalScale = GetTargetLocalScale(obj);
            var objScale = objLocalScale + scaleOffset;
            var oldScale = transform.localScale;
            var newScale = oldScale;

            if (isLockScale.x) newScale.x = objScale.x;
            if (isLockScale.y) newScale.y = objScale.y;
            if (isLockScale.z) newScale.z = objScale.z;

            if (newScale != oldScale) transform.localScale = newScale;
        }

        private Vector3 GetTargetLocalScale(GameObject obj)
        {
            var worldScale = Vector3.one;
            if (transform.parent != null) worldScale = transform.parent.TransformVector(Vector3.one);
            var objWorldScale = obj.transform.lossyScale;
            return new(objWorldScale.x / worldScale.x, objWorldScale.y / worldScale.y, objWorldScale.z / worldScale.z);
        }

        public void ActivateConstraint()
        {
            var obj = reference?.Get(this);
            if (obj == null) return;
            positionOffset = transform.position - obj.transform.position;
            rotationOffset = transform.eulerAngles - obj.transform.eulerAngles;
            var objLocalScale = GetTargetLocalScale(obj);
            scaleOffset = transform.localScale - objLocalScale;
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