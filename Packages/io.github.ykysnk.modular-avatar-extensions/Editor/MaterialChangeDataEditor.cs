using System.Collections.Generic;
using System.Linq;
using io.github.ykysnk.ModularAvatarExtensions.Editor.UIElements;
using io.github.ykysnk.utils.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [CustomPropertyDrawer(typeof(MaterialChangeData))]
    internal class MaterialChangeDataEditor : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var uxml =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    AssetDatabase.GUIDToAssetPath("8efbcfb8012e58f4e9ef98c00c086ca5"));

            if (uxml == null) return BasicEditor.CreateUxmlImportErrorUI();

            var tree = uxml.CloneTree();
            InternalLocalizationExtensions.Helper.UILocalize(tree, false);
            tree.Bind(property.serializedObject);

            var previewMaterial = tree.Q<ObjectField>("previewMaterial");
            previewMaterial.SetEnabled(false);

            if (property.serializedObject.targetObject is not ModularAvatarExtensionsChangeMaterialInBuild
                targetComponent)
                return BasicEditor.CreateErrorUI("Target object is not ModularAvatarExtensionsChangeMaterialInBuild.");

            var renderer = targetComponent.Renderer;
            if (renderer == null)
                return BasicEditor.CreateWarningUI("Renderer is not assigned or found in the target component.");

            var materialIndexField = new IntegerField
            {
                style =
                {
                    display = DisplayStyle.None
                }
            };

            tree.Add(materialIndexField);
            materialIndexField.BindProperty(property.FindPropertyRelative("materialIndex"));

            var previewMaterialField = tree.Q<ObjectField>("previewMaterial");

            var materialIndex = tree.Q<MaterialField>("materialIndex");
            materialIndex.choices = renderer.sharedMaterials.Select(t => t.name)
                .Select((materialName, i) => new KeyValuePair<int, string>(i, materialName)).ToList();
            materialIndex.index = 0;
            materialIndex.formatListItemCallback = pair =>
                "label.material_change_data.material_index_item".Sf(pair.Key, pair.Value);
            materialIndex.formatSelectedValueCallback =
                pair => "label.material_change_data.material_selected_value".Sf(pair.Key);

            InternalLocalizationExtensions.Helper.UpdateRegister("label.material_change_data.material_index_item",
                (label, tooltip) =>
                {
                    materialIndex.formatListItemCallback = pair =>
                        "label.material_change_data.material_index_item".Sf(pair.Key, pair.Value);
                    materialIndex.formatSelectedValueCallback =
                        pair => "label.material_change_data.material_selected_value".Sf(pair.Key);
                });

            materialIndex.RegisterValueChangedCallback(x =>
            {
                materialIndexField.value = x.newValue.Key;
                previewMaterialField.value = renderer.sharedMaterials[x.newValue.Key];
            });

            tree.schedule.Execute(OnDelayCall);
            return tree;

            void OnDelayCall()
            {
                materialIndex.index = materialIndexField.value =
                    Mathf.Clamp(materialIndexField.value, 0, materialIndex.choices.Count - 1);
                var material = renderer.sharedMaterials[materialIndex.index];
                if (material == null) return;
                previewMaterialField.value = material;
            }
        }
    }
}