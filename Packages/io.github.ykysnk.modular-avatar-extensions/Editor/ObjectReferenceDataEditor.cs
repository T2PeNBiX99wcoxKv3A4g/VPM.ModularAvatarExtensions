using io.github.ykysnk.utils.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [CustomPropertyDrawer(typeof(ObjectReferenceData))]
    internal class ObjectReferenceDataEditor : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var uxml =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    AssetDatabase.GUIDToAssetPath("3befee34a65e8eb469dde5652d7b3799"));

            if (uxml == null)
            {
                var errorTree = new VisualElement();
                errorTree.Add(new HelpBox("Failed to load uxml assets, please reimport the package to fix this issue.",
                    HelpBoxMessageType.Error));
                return errorTree;
            }

            // var root = new VisualElement();
            var tree = uxml.CloneTree();
            InternalLocalizationExtensions.Helper.UILocalize(tree, false);
            tree.Bind(property.serializedObject);

            var reference = tree.Q<PropertyField>("reference");
            // reference.RegisterValueChangeCallback(evt =>
            // {
            //     property.serializedObject.ApplyModifiedProperties();
            //     root.Clear();
            //     var newTree = uxml.CloneTree();
            //     // InternalLocalizationExtensions.Helper.UILocalize(newTree, false);
            //     newTree.Bind(property.serializedObject);
            //     root.Add(newTree);
            // });

            // root.Add(tree);
            return tree;
        }
    }
}