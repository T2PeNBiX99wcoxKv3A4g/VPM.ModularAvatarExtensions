using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    internal static class IconGeneratorBaseMenu
    {
        private const string MenuPath = "Tools/Modular Avatar EX/Remove All Unused Icon";

        [MenuItem(MenuPath)]
        private static void Menu(MenuCommand menuCommand)
        {
            ModularAvatarExtensionsIconGeneratorBase.RemoveAllUnusedIcon();
        }
    }
}