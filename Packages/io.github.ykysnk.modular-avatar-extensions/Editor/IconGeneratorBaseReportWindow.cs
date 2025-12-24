using System.Collections.Generic;
using System.IO;
using System.Linq;
using io.github.ykysnk.utils.Extensions;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    public class IconGeneratorBaseReportWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset? uxml;
        [PublicAPI] [SerializeField] private List<IconUsedPathReportData> reportData = new();

        public void CreateGUI()
        {
            var serializedObject = new SerializedObject(this);
            var tree = uxml!.CloneTree();
            tree.Bind(serializedObject);
            InternalLocalizationExtensions.Helper.UILocalize(tree);
            rootVisualElement.Add(tree);
        }

        [MenuItem("Assets/Modular Avatar EX/Find icons used path", false, Util.ToolsMenuItemPriority)]
        private static void CreateReport(MenuCommand menuCommand)
        {
            var needFindIds = (from guid in Selection.assetGUIDs
                select AssetDatabase.GUIDToAssetPath(guid)
                into path
                where Path.GetExtension(path) == ".png"
                where path.Contains(ModularAvatarExtensionsIconGeneratorBase.FolderPath)
                select Path.GetFileNameWithoutExtension(path)).ToList();

            var window = GetWindow<IconGeneratorBaseReportWindow>("label.icon_generator_base_report_window.report".S());
            window.reportData = Resources.FindObjectsOfTypeAll<ModularAvatarExtensionsIconGeneratorBase>()
                .Where(x => needFindIds.Contains(x.IconName)).GroupBy(x => x.IconName).ToDictionary(x => x.Key,
                    x => x.Select(y => new IconUsedPathData(y?.FullName() ?? "", y?.gameObject)).ToList())
                .Select(x => new IconUsedPathReportData(x.Key, x.Value)).ToList();
        }

        [MenuItem("Tools/Modular Avatar EX/All icons used path", false, Util.ToolsMenuItemPriority)]
        private static void CreateAllIconReport(MenuCommand menuCommand)
        {
            if (!Directory.Exists(ModularAvatarExtensionsIconGeneratorBase.FolderPath)) return;
            var allIds = Directory.GetFiles(ModularAvatarExtensionsIconGeneratorBase.FolderPath)
                .Where(x => Path.GetExtension(x) == ".png").Select(Path.GetFileNameWithoutExtension).ToList();

            var window = GetWindow<IconGeneratorBaseReportWindow>("label.icon_generator_base_report_window.report".S());
            window.reportData = Resources.FindObjectsOfTypeAll<ModularAvatarExtensionsIconGeneratorBase>()
                .GroupBy(x => x.IconName).ToDictionary(x => x.Key,
                    x => x.Select(y => new IconUsedPathData(y?.FullName() ?? "", y?.gameObject)).ToList())
                .Select(x => new IconUsedPathReportData(x.Key, x.Value)).ToList();
        }
    }
}