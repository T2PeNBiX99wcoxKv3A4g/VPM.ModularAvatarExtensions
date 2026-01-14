using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
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
            CreateReportAsync(Selection.assetGUIDs).Forget();
        }

        [MenuItem("Tools/Modular Avatar EX/All icons used path", false, Util.ToolsMenuItemPriority)]
        private static void CreateAllIconReport(MenuCommand menuCommand)
        {
            if (!Directory.Exists(ModularAvatarExtensionsIconGeneratorBase.FolderPath)) return;
            var window = GetWindow<IconGeneratorBaseReportWindow>("label.icon_generator_base_report_window.report".S());
            window.reportData = Resources.FindObjectsOfTypeAll<ModularAvatarExtensionsIconGeneratorBase>()
                .GroupBy(x => x.IconName).ToDictionary(x => x.Key,
                    x => x.Select(y => new IconUsedPathData(y?.FullName() ?? "", y?.gameObject)).ToList())
                .Select(x => new IconUsedPathReportData(x.Key, x.Value)).ToList();
        }

        private static async UniTask CreateReportAsync(string[] assetGUIDs)
        {
            var paths = assetGUIDs
                .Select(AssetDatabase.GUIDToAssetPath)
                .ToList();

            await UniTask.Yield();

            var needFindIds = await UniTask.RunOnThreadPool(() => paths
                .Where(path => Path.GetExtension(path) == ".png")
                .Where(path => path.Contains(ModularAvatarExtensionsIconGeneratorBase.FolderPath))
                .Select(Path.GetFileNameWithoutExtension)
                .ToList());

            var window = GetWindow<IconGeneratorBaseReportWindow>("label.icon_generator_base_report_window.report".S());
            window.reportData = Resources.FindObjectsOfTypeAll<ModularAvatarExtensionsIconGeneratorBase>()
                .Where(x => needFindIds.Contains(x.IconName)).GroupBy(x => x.IconName).ToDictionary(x => x.Key,
                    x => x.Select(y => new IconUsedPathData(y?.FullName() ?? "", y?.gameObject)).ToList())
                .Select(x => new IconUsedPathReportData(x.Key, x.Value)).ToList();
        }
    }
}