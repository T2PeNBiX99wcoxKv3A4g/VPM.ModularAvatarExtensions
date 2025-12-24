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
            var errorBox = container.Q<HelpBox>("errorObjectToggle");
            errorBox.style.display = DisplayStyle.None;
            errorBox.schedule.Execute(SetDisplay).Every(100);
            return;

            void SetDisplay()
            {
                if (target is not ModularAvatarExtensionsIconGeneratorBase iconGeneratorBase) return;

                var isSubMenu = iconGeneratorBase.TryGetComponent<ModularAvatarMenuItem>(out var menuItem) &&
                                menuItem.PortableControl.Type == PortableControlType.SubMenu;

                errorBox.style.display = iconGeneratorBase.TryGetComponent<ModularAvatarObjectToggle>(out _) || isSubMenu
                    ? DisplayStyle.None
                    : DisplayStyle.Flex;
            }
        }
    }
}