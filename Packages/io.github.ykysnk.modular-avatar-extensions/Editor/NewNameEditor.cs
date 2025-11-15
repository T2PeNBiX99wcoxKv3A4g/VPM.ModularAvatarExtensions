using io.github.ykysnk.utils.Editor;
using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(NewName))]
public class NewNameEditor : MaexEditor
{
    private const string NewNameProp = "newName";
    private const string ChangeOnInspectorProp = "changeOnInspector";
    private SerializedProperty? _changeOnInspector;
    private SerializedProperty? _newName;

    protected override void OnEnable()
    {
        _newName = serializedObject.FindProperty(NewNameProp);
        _changeOnInspector = serializedObject.FindProperty(ChangeOnInspectorProp);
    }

    protected override void OnInspectorGUIDraw()
    {
        EditorGUILayout.PropertyField(_newName, Utils.Label("New Name"));
        EditorGUILayout.PropertyField(_changeOnInspector, Utils.Label("Change On Inspector"));
        EditorGUILayout.HelpBox($"This object will be rename to '{_newName?.stringValue}'", MessageType.Info, true);
    }
}