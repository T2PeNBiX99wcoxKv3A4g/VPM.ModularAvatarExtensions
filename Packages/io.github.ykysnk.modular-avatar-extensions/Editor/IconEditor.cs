using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [CustomEditor(typeof(ModularAvatarExtensionsIcon))]
    [CanEditMultipleObjects]
    internal class IconEditor : MaexEditor
    {
        [SerializeField] protected VisualTreeAsset? uxml;

        protected override VisualElement CreateInnerInspectorGUI()
        {
            var tree = uxml!.CloneTree();
            var iconField = tree.Q<ObjectField>("icon");
            iconField.SetEnabled(false);
            var path = AssetDatabase.GetAssetPath(target);
            var pngPath = Path.ChangeExtension(path, ".png");
            var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(pngPath);
            iconField.value = icon;
            return tree;
        }
    }
}