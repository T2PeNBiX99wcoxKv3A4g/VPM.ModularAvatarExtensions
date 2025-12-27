using nadena.dev.modular_avatar.core;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [CustomEditor(typeof(ModularAvatarExtensionsIconGeneratorTarget))]
    [CanEditMultipleObjects]
    internal class IconGeneratorTargetEditor : MaexEditor
    {
        [SerializeField] protected VisualTreeAsset? uxml;

        protected override VisualElement CreateInnerInspectorGUI()
        {
            var tree = uxml!.CloneTree();
            var errorBox = tree.Q<HelpBox>("errorMenuItem");
            errorBox.style.display = DisplayStyle.None;
            errorBox.schedule.Execute(SetDisplay).Every(100);
            SetDisplay();
            return tree;

            void SetDisplay()
            {
                if (target == null ||
                    target is not ModularAvatarExtensionsIconGeneratorTarget iconGeneratorTarget) return;
                errorBox.style.display = iconGeneratorTarget.TryGetComponent<ModularAvatarMenuItem>(out _)
                    ? DisplayStyle.None
                    : DisplayStyle.Flex;
            }
        }
    }
}