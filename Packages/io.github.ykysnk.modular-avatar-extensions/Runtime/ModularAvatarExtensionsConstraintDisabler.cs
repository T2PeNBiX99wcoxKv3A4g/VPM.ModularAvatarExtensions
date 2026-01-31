using System.Linq;
using nadena.dev.ndmf.runtime;
using UnityEngine;
using UnityEngine.Animations;
#if MAEX_VRCSDK3_BASE
using VRC.Dynamics;
#endif

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Modular Avatar EX/MAEX Constraint Disabler")]
    public class ModularAvatarExtensionsConstraintDisabler : AvatarMaexComponent
    {
        public Component? constraint;
        public bool isActive = true;

        protected override void OnChange()
        {
            if (constraint == null)
                constraint = GetComponents<Component>().FirstOrDefault(c => c && c is
#if MAEX_VRCSDK3_BASE
                    VRCConstraintBase or
#endif
                    IConstraint);

            if (!constraint || RuntimeUtil.IsPlaying || !isActive) return;

            switch (constraint)
            {
#if MAEX_VRCSDK3_BASE
                case VRCConstraintBase { IsActive: true } vrcConstraintBase:
                    vrcConstraintBase.IsActive = false;
                    break;
#endif
                case IConstraint _:
#if UNITY_EDITOR
                    var constraintProxy = new ConstraintProxy(constraint);
                    if (constraintProxy.constraintActive)
                        constraintProxy.constraintActive = false;
#endif
                    break;
            }
        }
    }
}