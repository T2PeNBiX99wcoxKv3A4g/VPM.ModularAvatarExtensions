using Cysharp.Threading.Tasks;
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

        protected override VisualElement CreateInnerInspectorGUI()
        {
            var tree = uxml!.CloneTree();
            var errorBox = tree.Q<HelpBox>("errorMenuItem");
            errorBox.style.display = DisplayStyle.None;
            errorBox.schedule.Execute(SetDisplay).Every(100);
            SetDisplay();

            var button = tree.Q<Button>("generateIcon");
            button.clicked += () =>
            {
                if (target == null || target is not ModularAvatarExtensionsIconGeneratorBase iconGeneratorBase) return;
                iconGeneratorBase.ForceGenerateIcon().Forget();
            };

            var presetField = tree.Q<ObjectField>("preset");
            var presetGuid = PlayerPrefs.GetString("ModularAvatarExtensionsIconGeneratorPresetGUID", "");
            presetField.value = AssetDatabase.LoadAssetAtPath<Preset>(AssetDatabase.GUIDToAssetPath(presetGuid));
            presetField.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue is not Preset preset) return;
                if (preset != null && preset.GetTargetFullTypeName() != typeof(TextureImporter).FullName) return;
                if (target == null || target is not ModularAvatarExtensionsIconGeneratorBase iconGeneratorBase) return;
                var newPresetPath = AssetDatabase.GetAssetPath(preset);
                var newPresetGuid = AssetDatabase.AssetPathToGUID(newPresetPath);
                var oldPresetGuid = PlayerPrefs.GetString("ModularAvatarExtensionsIconGeneratorPresetGUID", "");
                if (oldPresetGuid == newPresetGuid) return;
                PlayerPrefs.SetString("ModularAvatarExtensionsIconGeneratorPresetGUID", newPresetGuid);
                presetField.value = preset;
                iconGeneratorBase.Preset = preset;
            });

            var iconTextureField = tree.Q<ObjectField>("iconTexture");
            iconTextureField.SetEnabled(false);

            var iconField = tree.Q<ObjectField>("icon");
            iconField.SetEnabled(false);

            OnCreateInnerInspectorGUI(tree);
            return tree;

            void SetDisplay()
            {
                if (target == null || target is not ModularAvatarExtensionsIconGeneratorBase iconGeneratorBase) return;
                errorBox.style.display = iconGeneratorBase.TryGetComponent<ModularAvatarMenuItem>(out _)
                    ? DisplayStyle.None
                    : DisplayStyle.Flex;
            }
        }

        [PublicAPI]
        protected abstract void OnCreateInnerInspectorGUI(TemplateContainer container);
    }
}