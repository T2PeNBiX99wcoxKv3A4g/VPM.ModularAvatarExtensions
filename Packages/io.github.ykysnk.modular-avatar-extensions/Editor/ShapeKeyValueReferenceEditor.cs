using System;
using System.Collections.Generic;
using System.Linq;
using nadena.dev.modular_avatar.core;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [CustomPropertyDrawer(typeof(ShapeKeyValueReference))]
    internal class ShapeKeyValueReferenceEditor : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var uxml =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    AssetDatabase.GUIDToAssetPath("74fa0341f40fc5c47b4977a7a13f08dd"));

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

            var shapeKeyName = new TextField
            {
                style =
                {
                    display = DisplayStyle.None
                }
            };

            tree.Add(shapeKeyName);
            shapeKeyName.BindProperty(property.FindPropertyRelative("shapeKeyName"));

            var shapeKeyNameField = tree.Q<DropdownField>("shapeKeyName");
            shapeKeyNameField.RegisterValueChangedCallback(evt => { shapeKeyName.value = evt.newValue; });

            var tempButton = new Button
            {
                text = "Temp Update Button"
            };
            tempButton.clicked += UpdateShapeDropdown;

            tree.Add(tempButton);

            // tree.RegisterCallback<SerializedPropertyChangeEvent>(_ =>
            // {
            //     Utils.Log(nameof(ShapeKeyValueReferenceEditor), $"SerializedPropertyChangeEvent: {property.propertyPath}");
            //     UpdateShapeDropdown();
            // });
            EditorApplication.delayCall += UpdateShapeDropdown;
            UpdateShapeDropdown();

            return tree;

            void UpdateShapeDropdown()
            {
                var findIndex = property.propertyPath.IndexOf(".shapeKeyValues.Array", StringComparison.Ordinal);
                if (findIndex < 0) return;
                var findProperty = property.serializedObject.FindProperty(property.propertyPath[..findIndex]);
                var targetObject = AvatarObjectReference.Get(findProperty.FindPropertyRelative("reference"));
                var shapeNames = new List<string>();

                if (targetObject != null &&
                    targetObject.TryGetComponent<SkinnedMeshRenderer>(out var skinnedMeshRenderer))
                {
                    var mesh = skinnedMeshRenderer.sharedMesh;
                    shapeNames = Enumerable.Range(0, mesh.blendShapeCount)
                        .Select(x => mesh.GetBlendShapeName(x))
                        .ToList();
                }

                shapeKeyNameField.SetEnabled(shapeNames.Count > 0);
                shapeKeyNameField.choices = shapeNames;
                shapeKeyNameField.value = shapeKeyName.value;

                shapeKeyNameField.formatSelectedValueCallback =
                    s =>
                    {
                        if (shapeNames.Count < 1)
                            return "<color=\"red\">Empty</color>";
                        return !shapeNames.Contains(shapeKeyName.value)
                            ? $"<color=\"red\">{shapeKeyName.value}</color>"
                            : s;
                    };
            }
        }
    }
}