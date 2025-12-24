using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [CustomEditor(typeof(ModularAvatarExtensionsTurnOnInBuild))]
    [CanEditMultipleObjects]
    internal class TurnOnInBuildEditor : MaexEditor
    {
        [SerializeField] private VisualTreeAsset? uxml;

        protected override VisualElement? CreateInnerInspectorGUI()
        {
            var tree = uxml!.CloneTree();
            return tree;
        }
    }
}