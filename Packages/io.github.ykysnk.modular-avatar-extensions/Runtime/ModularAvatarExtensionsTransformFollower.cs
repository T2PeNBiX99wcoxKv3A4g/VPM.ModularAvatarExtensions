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
        public Vector3 scaleOffset = Vector3.one;
        public BooleanVector3 isLockPosition = BooleanVector3.True;
        public BooleanVector3 isLockRotation = BooleanVector3.True;
        public BooleanVector3 isLockScale = BooleanVector3.True;
        [SerializeField] [Range(0, 6)] private int positionDecimals = 4;
        [SerializeField] [Range(0, 6)] private int rotationDecimals = 4;
        [SerializeField] [Range(0, 6)] private int scaleDecimals = 4;

        private void Update()
        {
            if (!gameObject.IsSceneObject() || !isLock || string.IsNullOrEmpty(reference?.referencePath)) return;

            var obj = reference?.Get(this);
            if (obj == null) return;

            var objPosition = obj.transform.TransformPointUnscaled(positionOffset).Round(positionDecimals);
            var oldPosition = transform.position.Round(positionDecimals);
            var newPosition = oldPosition;

            if (isLockPosition.x) newPosition.x = objPosition.x;
            if (isLockPosition.y) newPosition.y = objPosition.y;
            if (isLockPosition.z) newPosition.z = objPosition.z;

            if (!newPosition.Equals(oldPosition)) transform.position = newPosition;

            var objRotation =
                (obj.transform.rotation * Quaternion.Euler(rotationOffset)).eulerAngles.Round(rotationDecimals);
            var oldRotation = transform.eulerAngles.Round(rotationDecimals);
            var newRotation = oldRotation;

            if (isLockRotation.x) newRotation.x = objRotation.x;
            if (isLockRotation.y) newRotation.y = objRotation.y;
            if (isLockRotation.z) newRotation.z = objRotation.z;

            if (!newRotation.Equals(oldRotation)) transform.eulerAngles = newRotation;

            var objScale = Vector3.Scale(obj.transform.localScale, scaleOffset).Round(scaleDecimals);
            var oldScale = transform.localScale.Round(scaleDecimals);
            var newScale = oldScale;

            if (isLockScale.x) newScale.x = objScale.x;
            if (isLockScale.y) newScale.y = objScale.y;
            if (isLockScale.z) newScale.z = objScale.z;

            if (!newScale.Equals(oldScale)) transform.localScale = newScale;
        }

        protected override void OnChange() => FindBoneProxy();

        private void FindBoneProxy()
        {
            var boneProxy = GetComponent<ModularAvatarBoneProxy>();
            if (boneProxy == null || boneProxy.target == null) return;
            if (reference != null)
                reference.Set(boneProxy.target.gameObject);
            else
                reference = new(boneProxy.target.gameObject);

            boneProxy.attachmentMode = BoneProxyAttachmentMode.AsChildKeepWorldPose;
        }

        public void ActivateConstraint()
        {
            FindBoneProxy();
            var obj = reference?.Get(this);
            if (obj == null) return;
            positionOffset = obj.transform.InverseTransformPointUnscaled(transform.position).Round(positionDecimals);
            rotationOffset = (Quaternion.Inverse(obj.transform.rotation) * transform.rotation).eulerAngles.DeltaAngle()
                .Round(rotationDecimals);
            scaleOffset = transform.lossyScale.Divide(obj.transform.lossyScale).Round(scaleDecimals);
            isLock = true;
        }

        public void ZeroConstraint()
        {
            FindBoneProxy();
            positionOffset = Vector3.zero;
            rotationOffset = Vector3.zero;
            scaleOffset = Vector3.one;
            isLock = true;
        }
    }
}