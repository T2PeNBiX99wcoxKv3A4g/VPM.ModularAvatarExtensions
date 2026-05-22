using System.Collections.Generic;
using System.Linq;
using io.github.ykysnk.utils.Editor;
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

            if (uxml == null) return BasicEditor.CreateUxmlImportErrorUI();

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
            shapeKeyNameField.RegisterValueChangedCallback(evt => shapeKeyName.value = evt.newValue);

            var reference = tree.Q<PropertyField>("reference");
            reference.label = "";
            reference.RegisterValueChangeCallback(_ => UpdateShapeDropdown());
            reference.schedule.Execute(UpdateShapeDropdown);

            return tree;

            void UpdateShapeDropdown()
            {
                var targetObject = AvatarObjectReference.Get(property.FindPropertyRelative("reference"));
                var shapeNames = new List<string>();

                if (targetObject != null &&
                    targetObject.TryGetComponent<SkinnedMeshRenderer>(out var skinnedMeshRenderer))
                {
                    var mesh = skinnedMeshRenderer.sharedMesh;
                    shapeNames = Enumerable.Range(0, mesh.blendShapeCount)
                        .Select(mesh.GetBlendShapeName)
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
                        return !shapeNames.Contains(s)
                            ? $"<color=\"red\">{s}</color>"
                            : s;
                    };
            }
        }
    }
}