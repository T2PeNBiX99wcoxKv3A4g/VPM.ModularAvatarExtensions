using System.Linq;
using io.github.ykysnk.ModularAvatarExtensions.Editor;
using io.github.ykysnk.utils.Extensions;
using nadena.dev.ndmf;
using UnityEngine;

[assembly: ExportsPlugin(typeof(NewNameGenerator))]

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

public class NewNameGenerator : Plugin<NewNameGenerator>, IMaexPlugin
{
    public override string QualifiedName => "io.github.ykysnk.ModularAvatarExtensions.NewName";
    public override string DisplayName => "Modular Avatar Extensions New Name Generator";

    public void Log(object message) =>
        Debug.Log($"[{DisplayName}] {message}");

    public void Log(string detail, string hint)
    {
        var error = new SimpleStringError($"{DisplayName} Warning", detail, hint, ErrorSeverity.Information);
        ErrorReport.ReportError(error);
    }

    public void LogError(string detail, string hint)
    {
        var error = new SimpleStringError($"{DisplayName} Failed", detail, hint, ErrorSeverity.Error);
        ErrorReport.ReportError(error);
    }
    
    public void LogNonFatal(string detail, string hint)
    {
        var error = new SimpleStringError($"{DisplayName} Failed", detail, hint, ErrorSeverity.NonFatal);
        ErrorReport.ReportError(error);
    }

    protected override void Configure() =>
        InPhase(BuildPhase.Generating).Run($"Generate {DisplayName}", Generate);

    private void Generate(BuildContext ctx)
    {
        var avatar = ctx.AvatarRootObject;
        var autoChangeNames = avatar.GetComponentsInChildren<NewName>(true).Where(c => c).ToArray();

        Log($"Find {autoChangeNames.Length} new name inside \"{avatar.FullName()}\"");

        foreach (var comp in autoChangeNames)
        {
            var obj = comp.gameObject;
            var newName = comp.newName;

            if (string.IsNullOrEmpty(newName))
            {
                LogError($"New name can't be null or empty \"{obj.FullName()}\"",
                    $"Check the new name of \"{obj.FullName()}\"");
                continue;
            }

            Log($"Old name: \"{obj.name}\" New name: \"{newName}\" Path: \"{obj.FullName()}\"");
            obj.name = newName;
            Object.DestroyImmediate(comp);
        }
    }
}