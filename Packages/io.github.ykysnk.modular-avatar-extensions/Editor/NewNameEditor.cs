using io.github.ykysnk.utils.Editor;
using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(NewName))]
public class NewNameEditor : UnityEditor.Editor
{
    private const string NewNameProp = "newName";
    private const string ChangeOnInspectorProp = "changeOnInspector";
    private SerializedProperty? _changeOnInspector;
    private SerializedProperty? _newName;

    private void OnEnable()
    {
        _newName = serializedObject.FindProperty(NewNameProp);
        _changeOnInspector = serializedObject.FindProperty(ChangeOnInspectorProp);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(_newName, Utils.Label("New Name"));
        EditorGUILayout.PropertyField(_changeOnInspector, Utils.Label("Change On Inspector"));
        EditorGUILayout.HelpBox($"This object will be rename to '{_newName?.stringValue}'", MessageType.Info, true);

        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
    }
}