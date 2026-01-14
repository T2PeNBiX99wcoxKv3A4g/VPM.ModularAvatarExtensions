using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    internal static class IconGeneratorBaseMenu
    {
        [MenuItem("Tools/Modular Avatar EX/Remove All Unused Icons", false, Util.ToolsMenuItemPriority)]
        private static void MenuRemoveAllUnusedIcons(MenuCommand menuCommand) =>
            ModularAvatarExtensionsIconGeneratorBase.RemoveAllUnusedIcons();

        [MenuItem("Tools/Modular Avatar EX/Apply Preset To All Icons", false, Util.ToolsMenuItemPriority)]
        private static void MenuApplyPresetToAllIcons(MenuCommand menuCommand) =>
            ModularAvatarExtensionsIconGeneratorBase.ApplyPresetToAllIcons();

        [MenuItem("Tools/Modular Avatar EX/Force Generate All Icons", false, Util.ToolsMenuItemPriority)]
        private static void MenuForceGenerateAllIcons(MenuCommand menuCommand) =>
            ModularAvatarExtensionsIconGeneratorBase.ForceGenerateAllIcons();
    }
}