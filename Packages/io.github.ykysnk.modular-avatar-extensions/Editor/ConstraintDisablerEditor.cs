using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using VRC.Dynamics;
using Utils = io.github.ykysnk.utils.Editor.Utils;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(ConstraintDisabler))]
[CanEditMultipleObjects]
public class ConstraintDisablerEditor : MaexEditor
{
    private const string ConstraintProp = "constraint";
    private const string StopDisableProp = "stopDisable";
    private SerializedProperty? _constraint;
    private SerializedProperty? _stopDisable;

    protected override void OnEnable()
    {
        _constraint = serializedObject.FindProperty(ConstraintProp);
        _stopDisable = serializedObject.FindProperty(StopDisableProp);
    }

    protected override void OnInspectorGUIDraw()
    {
        var component = (ConstraintDisabler)target;
        var isConstraint = component.constraint is VRCConstraintBase or IConstraint;
        var count = component.GetComponents<Component>().Count(c => c && c is VRCConstraintBase or IConstraint);

        if (count > 1)
            EditorGUILayout.PropertyField(_constraint, Utils.Label("Constraint"));
        EditorGUILayout.PropertyField(_stopDisable, Utils.Label("Stop Disable"));

        if (!isConstraint)
            EditorGUILayout.HelpBox("The target component is not constraint component", MessageType.Error, true);

        EditorGUILayout.HelpBox(
            "This constraint component will be disable, active when avatar is in building",
            MessageType.Info, true);
    }
}