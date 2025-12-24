using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    internal static class IconGeneratorBaseMenu
    {
        [MenuItem("Tools/Modular Avatar EX/Remove All Unused Icon", false, Util.ToolsMenuItemPriority)]
        private static void Menu(MenuCommand menuCommand)
        {
            ModularAvatarExtensionsIconGeneratorBase.RemoveAllUnusedIcon();
        }
    }
}