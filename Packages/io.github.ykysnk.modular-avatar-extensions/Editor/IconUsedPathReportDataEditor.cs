using io.github.ykysnk.utils.Editor;
using UnityEditor;
using UnityEditor.UIElements;
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

            if (uxml == null)
            {
                var errorTree = new VisualElement();
                errorTree.Add(new HelpBox("Failed to load uxml assets, please reimport the package to fix this issue.",
                    HelpBoxMessageType.Error));
                return errorTree;
            }

            var tree = uxml.CloneTree();
            InternalLocalizationExtensions.Helper.UILocalize(tree, false);
            tree.Bind(property.serializedObject);
            return tree;
        }
    }
}