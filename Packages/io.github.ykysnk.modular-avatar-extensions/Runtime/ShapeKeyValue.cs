using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    public readonly struct ShapeKeyValue
    {
        public readonly int ShapeKeyIndex;
        public readonly float Value;

        public ShapeKeyValue(GameObject gameObject, string shapeKeyName, float value)
        {
            if (gameObject.TryGetComponent<SkinnedMeshRenderer>(out var skinnedMeshRenderer))
                ShapeKeyIndex = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(shapeKeyName);
            else
                ShapeKeyIndex = -1;
            Value = value;
        }
    }
}