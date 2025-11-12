using io.github.ykysnk.utils;
using UnityEditor;
using Utils = io.github.ykysnk.utils.Editor.Utils;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(ConstraintDisabler))]
[CanEditMultipleObjects]
public class ConstraintDisablerEditor : UnityEditor.Editor
{
    private const string ConstraintProp = "constraint";
    private const string StopDisableProp = "stopDisable";
    private SerializedProperty? _constraint;
    private SerializedProperty? _stopDisable;

    private void OnEnable()
    {
        _constraint = serializedObject.FindProperty(ConstraintProp);
        _stopDisable = serializedObject.FindProperty(StopDisableProp);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();

        var component = (YkyEditorComponent)target;

        component.OnInspectorGUI();

        EditorGUILayout.PropertyField(_constraint, Utils.Label("Constraint"));
        EditorGUILayout.PropertyField(_stopDisable, Utils.Label("Stop Disable"));
        EditorGUILayout.HelpBox(
            "This constraint component will be disable, active when avatar is in building",
            MessageType.Info, true);

        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
    }
}