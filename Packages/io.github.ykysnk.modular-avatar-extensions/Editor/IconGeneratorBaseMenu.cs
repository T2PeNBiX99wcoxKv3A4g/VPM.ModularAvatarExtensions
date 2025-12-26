using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    internal static class IconGeneratorBaseMenu
    {
        [MenuItem("Tools/Modular Avatar EX/Remove All Unused Icon", false, Util.ToolsMenuItemPriority)]
        private static void Menu(MenuCommand menuCommand) =>
            ModularAvatarExtensionsIconGeneratorBase.RemoveAllUnusedIcon();

        [MenuItem("Tools/Modular Avatar EX/Set Preset To All Icon", false, Util.ToolsMenuItemPriority)]
        private static void MenuSetPresetToAllIcon(MenuCommand menuCommand) =>
            ModularAvatarExtensionsIconGeneratorBase.SetPresetToAllIcon();
    }
}