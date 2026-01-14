using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    internal static class IconGeneratorBaseMenu
    {
        [MenuItem("Tools/Modular Avatar EX/Remove All Unused Icon", false, Util.ToolsMenuItemPriority)]
        private static void MenuRemoveAllUnusedIcon(MenuCommand menuCommand) =>
            ModularAvatarExtensionsIconGeneratorBase.RemoveAllUnusedIcon();

        [MenuItem("Tools/Modular Avatar EX/Apply Preset To All Icon", false, Util.ToolsMenuItemPriority)]
        private static void MenuApplyPresetToAllIcon(MenuCommand menuCommand) =>
            ModularAvatarExtensionsIconGeneratorBase.ApplyPresetToAllIcon();
    }
}