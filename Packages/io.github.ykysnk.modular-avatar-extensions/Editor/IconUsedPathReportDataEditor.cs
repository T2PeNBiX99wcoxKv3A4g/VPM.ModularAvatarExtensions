using System.IO;
using io.github.ykysnk.utils.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [CustomPropertyDrawer(typeof(IconUsedPathReportData))]
    internal class IconUsedPathReportDataEditor : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var uxml =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    AssetDatabase.GUIDToAssetPath("76c6d5041e790c947b09ca8bca0179a2"));

            if (uxml == null) return BasicEditor.CreateUxmlImportErrorUI();

            var tree = uxml.CloneTree();
            InternalLocalizationExtensions.Helper.UILocalize(tree, false);
            tree.Bind(property.serializedObject);

            var iconNameField = tree.Q<TextField>("iconName");
            var iconNameObjectField = tree.Q<ObjectField>("iconNameObject");
            iconNameObjectField.SetEnabled(false);
            iconNameObjectField.schedule.Execute(() =>
            {
                var iconPath = Path.Combine(ModularAvatarExtensionsIconGeneratorBase.FolderPath,
                    $"{iconNameField.value}.png");
                var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(iconPath);
                iconNameObjectField.value = icon;
            });
            return tree;
        }
    }
}