using System.Linq;
using io.github.ykysnk.utils.Extensions;
using nadena.dev.ndmf;
using VRC.Dynamics;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[RunsOnPlatforms(WellKnownPlatforms.VRChatAvatar30)]
internal class ConstraintDisablerPass : MaexPass<ConstraintDisablerPass>
{
    public override string QualifiedName => "io.github.ykysnk.ModularAvatarExtensions.ConstraintDisabler";
    public override string DisplayName => "Modular Avatar Extensions Constraint Disabler";

    protected override void Execute(BuildContext ctx)
    {
        var avatar = ctx.AvatarRootObject;
        var constraintDisables = avatar.GetComponentsInChildren<ConstraintDisabler>(true).Where(c => c).ToArray();

        Log($"Find {constraintDisables.Length} constraint disabler inside \"{avatar.FullName()}\"");

        foreach (var constraintDisabler in constraintDisables)
        {
            var constraint = constraintDisabler.constraint;
            if (constraint == null)
                constraint = constraintDisabler.GetComponent<VRCConstraintBase>();
            if (constraint == null)
            {
                LogError($"Can't find constraint of \"{constraintDisabler.FullName()}\"",
                    $"Check the constraint path of {constraintDisabler.FullName()}");
                continue;
            }

            if (!constraint.IsActive)
                constraint.IsActive = true;
        }
    }
}