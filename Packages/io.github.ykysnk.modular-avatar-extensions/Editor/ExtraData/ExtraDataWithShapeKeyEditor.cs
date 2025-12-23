using io.github.ykysnk.ModularAvatarExtensions.ExtraData;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor.ExtraData
{
    [CustomEditor(typeof(ExtraDataWithShapeKey))]
    [CanEditMultipleObjects]
    internal class ExtraDataWithShapeKeyEditor : MaexEditor
    {
        [SerializeField] protected VisualTreeAsset? uxml;

        protected override VisualElement? CreateInnerInspectorGUI()
        {
            var tree = uxml!.CloneTree();
            return tree;
        }
    }
}