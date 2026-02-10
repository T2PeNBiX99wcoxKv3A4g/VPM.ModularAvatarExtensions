using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    internal static class IconGeneratorBaseMenu
    {
        [MenuItem("Tools/Modular Avatar EX/Apply Preset To All Icons", false, Util.ToolsMenuItemPriority)]
        private static void MenuApplyPresetToAllIcons(MenuCommand menuCommand) =>
            ModularAvatarExtensionsIconGeneratorBase.ApplyPresetToAllIcons();
    }
}