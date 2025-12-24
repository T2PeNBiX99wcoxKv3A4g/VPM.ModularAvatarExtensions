using io.github.ykysnk.utils.Extensions;
using io.github.ykysnk.utils.NonUdon;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Modular Avatar EX/MAEX Transform Lock")]
    [ExecuteInEditMode]
    public class ModularAvatarExtensionsTransformLock : AvatarMaexComponent
    {
        public bool isLock;
        public Vector3 lockPosition = Vector3.zero;
        public Vector3 lockRotation = Vector3.zero;
        public Vector3 lockScale = Vector3.one;
        public BooleanVector3 isLockPosition = BooleanVector3.True;
        public BooleanVector3 isLockRotation = BooleanVector3.True;
        public BooleanVector3 isLockScale = BooleanVector3.True;

        private void Update()
        {
            if (!gameObject.IsSceneObject() || !isLock) return;

            var oldPosition = transform.localPosition;
            var newPosition = oldPosition;

            if (isLockPosition.x) newPosition.x = lockPosition.x;
            if (isLockPosition.y) newPosition.y = lockPosition.y;
            if (isLockPosition.z) newPosition.z = lockPosition.z;

            if (newPosition != oldPosition) transform.localPosition = newPosition;

            var oldRotation = transform.localEulerAngles;
            var newRotation = oldRotation;

            if (isLockRotation.x) newRotation.x = lockRotation.x;
            if (isLockRotation.y) newRotation.y = lockRotation.y;
            if (isLockRotation.z) newRotation.z = lockRotation.z;

            if (newRotation != oldRotation) transform.localEulerAngles = newRotation;

            var oldScale = transform.localScale;
            var newScale = oldScale;

            if (isLockScale.x) newScale.x = lockScale.x;
            if (isLockScale.y) newScale.y = lockScale.y;
            if (isLockScale.z) newScale.z = lockScale.z;

            if (newScale != oldScale) transform.localScale = newScale;
        }

        public void ActivateConstraint()
        {
            lockPosition = transform.localPosition;
            lockRotation = transform.localEulerAngles;
            lockScale = transform.localScale;
            isLock = true;
        }

        public void ZeroConstraint()
        {
            lockPosition = Vector3.zero;
            lockRotation = Vector3.zero;
            lockScale = Vector3.one;
            isLock = true;
        }
    }
}