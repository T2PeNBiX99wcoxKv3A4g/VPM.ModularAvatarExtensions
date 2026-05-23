using System;
using System.Linq;
using io.github.ykysnk.utils.Extensions;
using nadena.dev.ndmf;
using UnityEngine.Animations;
#if MAEX_VRCSDK3_BASE
using VRC.Dynamics;
#endif

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [RunsOnPlatforms(WellKnownPlatforms.VRChatAvatar30)]
    internal class ConstraintDisablerPass : MaexPass<ConstraintDisablerPass>
    {
        public override string QualifiedName => "io.github.ykysnk.ModularAvatarExtensions.ConstraintDisabler";
        public override string DisplayName => "Modular Avatar Extensions Constraint Disabler";

        protected override void Execute(BuildContext ctx)
        {
            var avatar = ctx.AvatarRootObject;
            var constraintDisables = avatar.GetComponentsInChildren<ModularAvatarExtensionsConstraintDisabler>(true)
                .Where(c => c).ToArray();

            LogC($"Find {constraintDisables.Length} constraint disabler inside \"{avatar.FullName()}\"");

            foreach (var constraintDisabler in constraintDisables)
                using (ErrorReport.WithContextObject(constraintDisabler))
                    try
                    {
                        var constraint = constraintDisabler.constraint;
                        if (constraint == null)
                        {
                            // TODO: Remove full name
                            LogError("error.constraint_disabler_pass.constraint_not_found",
                                constraintDisabler.FullName());
                            continue;
                        }

                        switch (constraint)
                        {
#if MAEX_VRCSDK3_BASE
                            case VRCConstraintBase { IsActive: false } vrcConstraintBase:
                                vrcConstraintBase.IsActive = true;
                                break;
#endif
                            case IConstraint _:
                                var constraintProxy = new ConstraintProxy(constraint);
                                if (!constraintProxy.constraintActive)
                                    constraintProxy.constraintActive = true;
                                break;
                            default:
                                // TODO: Remove full name
                                LogError("error.constraint_disabler_pass.unknown_constraint_type",
                                    constraintDisabler.FullName());
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        ErrorReport.ReportException(e);
                        throw;
                    }
        }
    }
}