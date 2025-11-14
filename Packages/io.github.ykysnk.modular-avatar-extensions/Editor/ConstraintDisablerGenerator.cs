using System.Linq;
using io.github.ykysnk.ModularAvatarExtensions.Editor;
using io.github.ykysnk.utils.Extensions;
using nadena.dev.ndmf;
using UnityEngine;
using VRC.Dynamics;

[assembly: ExportsPlugin(typeof(ConstraintDisablerGenerator))]

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

public class ConstraintDisablerGenerator : Plugin<ConstraintDisablerGenerator>, IMaexPlugin
{
    public override string QualifiedName => "io.github.ykysnk.ModularAvatarExtensions.ConstraintDisabler";
    public override string DisplayName => "Modular Avatar Extensions Constraint Disabler Generator";

    public string LogDisplayName => DisplayName;

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
        InPhase(BuildPhase.Generating).Run($"Generate {DisplayName}", Generate);

    private void Generate(BuildContext ctx)
    {
        var avatar = ctx.AvatarRootObject;
        var constraintDisables = avatar.GetComponentsInChildren<ConstraintDisabler>(true).Where(c => c).ToArray();

        Log($"Find {constraintDisables.Length} constraint disabler inside \"{avatar.FullName()}\"");

        foreach (var constraintDisabler in constraintDisables)
        {
            var constraint = constraintDisabler.constraint;
            if (!constraint)
                constraint = constraintDisabler.GetComponent<VRCConstraintBase>();
            if (!constraint)
            {
                LogError($"Can't find constraint of \"{constraintDisabler.FullName()}\"",
                    $"Check the constraint path of {constraintDisabler.FullName()}");
                continue;
            }

            if (!constraint!.IsActive)
                constraint.IsActive = true;
            Object.DestroyImmediate(constraintDisabler);
        }
    }
}