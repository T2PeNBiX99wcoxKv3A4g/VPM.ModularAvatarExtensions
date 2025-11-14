using System.Linq;
using io.github.ykysnk.ModularAvatarExtensions.Editor;
using io.github.ykysnk.utils.Extensions;
using nadena.dev.ndmf;
using UnityEngine;

[assembly: ExportsPlugin(typeof(MoveToRootGenerator))]

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

public class MoveToRootGenerator : Plugin<MoveToRootGenerator>, IMaexPlugin
{
    public override string QualifiedName => "io.github.ykysnk.ModularAvatarExtensions.MoveToRoot";
    public override string DisplayName => "Modular Avatar Extensions Move To Root Generator";

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
        var autoMoveToRoots = avatar.GetComponentsInChildren<MoveToRoot>(true).Where(c => c).ToArray();

        Log($"Find {autoMoveToRoots.Length} move to root inside \"{avatar.FullName()}\"");

        foreach (var moveToRoot in autoMoveToRoots)
        {
            var obj = moveToRoot.gameObject;
            if (obj.transform.parent == ctx.AvatarRootTransform)
            {
                Log($"Already in root \"{obj.FullName()}\"");
                continue;
            }

            obj.transform.SetParent(ctx.AvatarRootTransform);
            Log($"New Path: \"{obj.FullName()}\"");
            Object.DestroyImmediate(moveToRoot);
        }

        var autoMoveToRootOfTransforms =
            avatar.GetComponentsInChildren<MoveToRootOfReference>(true).Where(c => c).ToArray();

        Log($"Find {autoMoveToRootOfTransforms.Length} move to root inside \"{avatar.FullName()}\"");

        foreach (var moveToRootOfTransform in autoMoveToRootOfTransforms)
        {
            var found = ctx.AvatarRootTransform.Find(moveToRootOfTransform?.reference?.referencePath);

            if (found == null)
            {
                LogError($"Can't find anything using path \"{moveToRootOfTransform?.reference?.referencePath}\"",
                    $"Check the root transform path of {moveToRootOfTransform?.FullName()}");
                continue;
            }

            found.transform.SetParent(ctx.AvatarRootTransform);
            Log($"New Path: \"{found.FullName()}\"");
            Object.DestroyImmediate(moveToRootOfTransform);
        }
    }
}