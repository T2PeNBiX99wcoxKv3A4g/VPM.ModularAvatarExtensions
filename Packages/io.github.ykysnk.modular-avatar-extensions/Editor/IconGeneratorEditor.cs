using nadena.dev.modular_avatar.core;
using UnityEditor;
using UnityEngine.UIElements;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [CustomEditor(typeof(ModularAvatarExtensionsIconGenerator))]
    [CanEditMultipleObjects]
    internal class IconGeneratorEditor : IconGeneratorBaseEditor
    {
        protected override void OnCreateInnerInspectorGUI(TemplateContainer container)
        {
            var errorBox = container.Q<HelpBox>("error");
            errorBox.style.display = DisplayStyle.None;
            EditorApplication.hierarchyWindowItemOnGUI += (_, _) =>
            {
                var iconGeneratorBase = (ModularAvatarExtensionsIconGeneratorBase)target;
                if (iconGeneratorBase == null) return;
                errorBox.style.display = iconGeneratorBase.TryGetComponent<ModularAvatarObjectToggle>(out _)
                    ? DisplayStyle.None
                    : DisplayStyle.Flex;
            };
        }
    }
}