using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [CustomEditor(typeof(IconGeneratorOfReferenceWithShapeKey))]
    [CanEditMultipleObjects]
    internal class IconGeneratorOfReferenceWithShapeKeyEditor : MaexEditor
    {
        [SerializeField] protected VisualTreeAsset? uxml;

        protected override VisualElement? CreateInnerInspectorGUI()
        {
            var tree = uxml!.CloneTree();
            return tree;
        }
    }
}