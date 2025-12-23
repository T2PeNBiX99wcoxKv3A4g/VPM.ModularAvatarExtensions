using io.github.ykysnk.utils.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [CustomPropertyDrawer(typeof(IconUsedPathData))]
    internal class IconUsedPathDataEditor : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var uxml =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    AssetDatabase.GUIDToAssetPath("513730486ca39854896e3b6f9052f8f6"));

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

            var gameObjectField = tree.Q<ObjectField>("gameObject");
            gameObjectField.SetEnabled(false);

            return tree;
        }
    }
}