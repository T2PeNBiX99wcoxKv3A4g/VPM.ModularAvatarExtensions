using io.github.ykysnk.autohook;
using io.github.ykysnk.utils;
using UnityEngine;
using VRC.Dynamics;

namespace io.github.ykysnk.ModularAvatarExtensions;

[DisallowMultipleComponent]
[AddComponentMenu("Modular Avatar EX/MAEX Constraint Disabler")]
public class ConstraintDisabler : YkyEditorComponent
{
    [Autohook] public VRCConstraintBase? constraint;
    public bool stopDisable;

    private void OnValidate()
    {
        if (!constraint)
            constraint = GetComponent<VRCConstraintBase>();
        Disable();
    }

    private void Disable()
    {
        if (!constraint || Application.isPlaying || stopDisable) return;
        if (constraint!.IsActive)
            constraint.IsActive = false;
    }

    public override void OnInspectorGUI()
    {
        Disable();
    }
}