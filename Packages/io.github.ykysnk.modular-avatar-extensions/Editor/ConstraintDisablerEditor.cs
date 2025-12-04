using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
#if MAEX_VRCSDK3_BASE
using VRC.Dynamics;
#endif

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(ModularAvatarExtensionsConstraintDisabler))]
[CanEditMultipleObjects]
internal class ConstraintDisablerEditor : MaexEditor
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

    protected override void OnInnerInspectorGUI()
    {
        var component = (ModularAvatarExtensionsConstraintDisabler)target;
        var isConstraint = component.constraint is
#if MAEX_VRCSDK3_BASE
            VRCConstraintBase or
#endif
            IConstraint;
        var count = component.GetComponents<Component>().Count(c => c && c is
#if MAEX_VRCSDK3_BASE
            VRCConstraintBase or
#endif
            IConstraint);

        if (count > 1)
            EditorGUILayout.PropertyField(_constraint, "label.constraint_disabler.constraint".G());
        EditorGUILayout.PropertyField(_stopDisable, "label.constraint_disabler.stop_disable".G());

        if (!isConstraint)
            EditorGUILayout.HelpBox("label.constraint_disabler.constraint_error".S(),
                MessageType.Error, true);

        EditorGUILayout.HelpBox("label.constraint_disabler.info".S(), MessageType.Info,
            true);
    }
}