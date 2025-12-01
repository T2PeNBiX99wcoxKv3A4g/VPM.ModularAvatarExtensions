using System.Linq;
using io.github.ykysnk.Localization.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using VRC.Dynamics;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(ModularAvatarExtensionsConstraintDisabler))]
[CanEditMultipleObjects]
public class ConstraintDisablerEditor : MaexEditor
{
    private const string ConstraintProp = "constraint";
    private const string StopDisableProp = "stopDisable";
    private SerializedProperty? _constraint;
    private SerializedProperty? _stopDisable;

    protected override void OnEnable()
    {
        base.OnEnable();
        _constraint = serializedObject.FindProperty(ConstraintProp);
        _stopDisable = serializedObject.FindProperty(StopDisableProp);
    }

    protected override void OnMaexInspectorGUI()
    {
        var component = (ModularAvatarExtensionsConstraintDisabler)target;
        var isConstraint = component.constraint is VRCConstraintBase or IConstraint;
        var count = component.GetComponents<Component>().Count(c => c && c is VRCConstraintBase or IConstraint);

        if (count > 1)
            EditorGUILayout.PropertyField(_constraint, "label.constraint_disabler.constraint".G(Util.LocalizationID));
        EditorGUILayout.PropertyField(_stopDisable, "label.constraint_disabler.stop_disable".G(Util.LocalizationID));

        if (!isConstraint)
            EditorGUILayout.HelpBox("label.constraint_disabler.constraint_error".L(Util.LocalizationID),
                MessageType.Error, true);

        EditorGUILayout.HelpBox("label.constraint_disabler.info".L(Util.LocalizationID), MessageType.Info,
            true);
    }
}