using UnityEngine;
using VRC.Dynamics;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Modular Avatar EX/MAEX Constraint Disabler")]
    public class ConstraintDisabler : AvatarMaexComponent
    {
        public VRCConstraintBase? constraint;
        public bool stopDisable;

        protected override void OnChange()
        {
            if (!constraint)
                constraint = GetComponent<VRCConstraintBase>();
            if (!constraint || Application.isPlaying || stopDisable) return;
            if (constraint!.IsActive)
                constraint.IsActive = false;
        }
    }
}