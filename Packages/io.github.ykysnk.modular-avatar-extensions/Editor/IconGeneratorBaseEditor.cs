using JetBrains.Annotations;
using nadena.dev.modular_avatar.core;
using UnityEditor;
using UnityEditor.Presets;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    internal abstract class IconGeneratorBaseEditor : MaexEditor
    {
        [SerializeField] protected VisualTreeAsset? uxml;

        protected override void OnInnerInspectorGUI()
        {
        }

        protected override VisualElement? CreateInnerInspectorGUI()
        {
            var visualTree = uxml!.CloneTree();
            InternalLocalizationExtensions.Helper.UILocalize(visualTree);
            visualTree.Bind(serializedObject);

            var errorBox = visualTree.Q<HelpBox>("errorMenuItem");
            errorBox.style.display = DisplayStyle.None;
            EditorApplication.hierarchyWindowItemOnGUI += (_, _) =>
            {
                var iconGeneratorBase = (ModularAvatarExtensionsIconGeneratorBase)target;
                if (iconGeneratorBase == null) return;
                errorBox.style.display = iconGeneratorBase.TryGetComponent<ModularAvatarMenuItem>(out _)
                    ? DisplayStyle.None
                    : DisplayStyle.Flex;
            };

            var button = visualTree.Q<Button>("generateIcon");
            button.clicked += () =>
            {
                var iconGeneratorBase = (ModularAvatarExtensionsIconGeneratorBase)target;
                if (iconGeneratorBase == null) return;
                iconGeneratorBase.ForceGenerateIcon();
            };

            var presetField = visualTree.Q<ObjectField>("preset");
            var presetGuid = PlayerPrefs.GetString("ModularAvatarExtensionsIconGeneratorPresetGUID", "");
            presetField.value = AssetDatabase.LoadAssetAtPath<Preset>(AssetDatabase.GUIDToAssetPath(presetGuid));
            presetField.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue is not Preset preset) return;
                if (preset != null && preset.GetTargetFullTypeName() != typeof(TextureImporter).FullName) return;
                var iconGeneratorBase = (ModularAvatarExtensionsIconGeneratorBase)target;
                var newPresetPath = AssetDatabase.GetAssetPath(preset);
                var newPresetGuid = AssetDatabase.AssetPathToGUID(newPresetPath);
                var oldPresetGuid = PlayerPrefs.GetString("ModularAvatarExtensionsIconGeneratorPresetGUID", "");
                if (oldPresetGuid == newPresetGuid) return;
                PlayerPrefs.SetString("ModularAvatarExtensionsIconGeneratorPresetGUID", newPresetGuid);
                presetField.value = preset;
                iconGeneratorBase.Preset = preset;
            });

            OnCreateInnerInspectorGUI(visualTree);
            return visualTree;
        }

        [PublicAPI]
        protected abstract void OnCreateInnerInspectorGUI(TemplateContainer container);
    }
}