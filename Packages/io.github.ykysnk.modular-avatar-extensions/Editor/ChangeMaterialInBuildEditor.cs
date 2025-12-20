using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [CustomEditor(typeof(ModularAvatarExtensionsChangeMaterialInBuild))]
    [CanEditMultipleObjects]
    internal class ChangeMaterialInBuildEditor : MaexEditor
    {
        [SerializeField] private VisualTreeAsset? uxml;

        protected override void OnInnerInspectorGUI()
        {
        }

        protected override VisualElement? CreateInnerInspectorGUI()
        {
            var tree = uxml!.CloneTree();
            return tree;
        }
    }
}