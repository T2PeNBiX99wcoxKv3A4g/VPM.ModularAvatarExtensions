using System.Linq;
using nadena.dev.ndmf.runtime;
using UnityEngine;
using UnityEngine.Animations;
using VRC.Dynamics;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Modular Avatar EX/MAEX Constraint Disabler")]
    public class ConstraintDisabler : AvatarMaexComponent
    {
        public Component? constraint;
        public bool stopDisable;

        protected override void OnChange()
        {
            if (constraint == null)
                constraint = GetComponent<VRCConstraintBase>();
            if (constraint == null)
                constraint = GetComponents<Component>().FirstOrDefault(c => c && c is IConstraint);

            if (!constraint || RuntimeUtil.IsPlaying || stopDisable) return;

            switch (constraint)
            {
                case VRCConstraintBase { IsActive: true } vrcConstraintBase:
                    vrcConstraintBase.IsActive = false;
                    break;
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