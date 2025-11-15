using io.github.ykysnk.autohook;
using UnityEngine;
using VRC.Dynamics;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Modular Avatar EX/MAEX Constraint Disabler")]
    public class ConstraintDisabler : AvatarMaexComponent
    {
        [Autohook] public VRCConstraintBase? constraint;
        public bool stopDisable;

        protected override void OnChange(bool isValidate)
        {
            if (isValidate)
                if (!constraint)
                    constraint = GetComponent<VRCConstraintBase>();

            if (!constraint || Application.isPlaying || stopDisable) return;
            if (constraint!.IsActive)
                constraint.IsActive = false;
        }
    }
}