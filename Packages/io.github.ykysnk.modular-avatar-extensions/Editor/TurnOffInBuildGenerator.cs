using io.github.ykysnk.ModularAvatarExtensions.Editor;
using io.github.ykysnk.utils.Extensions;
using nadena.dev.ndmf;
using UnityEngine;

[assembly: ExportsPlugin(typeof(TurnOffInBuildGenerator))]

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

public class TurnOffInBuildGenerator : Plugin<TurnOffInBuildGenerator>, IMaexPlugin
{
    public override string QualifiedName => "io.github.ykysnk.ModularAvatarExtensions.TurnOffInBuild";
    public override string DisplayName => "Modular Avatar Extensions Turn Off In Build Generator";

    public void Log(object message) => Debug.Log($"[{DisplayName}] {message}");

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

    protected override void Configure() =>
        InPhase(BuildPhase.Resolving).Run($"Generate {DisplayName}", Generate);

    private void Generate(BuildContext ctx)
    {
        var avatar = ctx.AvatarRootObject;
        var turnOffInBuilds = avatar.GetComponentsInChildren<TurnOffInBuild>(true);

        Log($"Find {turnOffInBuilds.Length} turn off in build inside \"{avatar.FullName()}\"");

        foreach (var turnOffInBuild in turnOffInBuilds)
        {
            var obj = turnOffInBuild.gameObject;
            obj.SetActive(false);
            Log($"Game Object \"{obj.FullName()}\" is now non active");
        }
    }
}