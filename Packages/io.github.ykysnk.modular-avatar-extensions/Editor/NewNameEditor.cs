using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(ModularAvatarExtensionsNewName))]
[CanEditMultipleObjects]
internal class NewNameEditor : MaexEditor
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

    protected override void OnInnerInspectorGUI()
    {
        EditorGUILayout.PropertyField(_newName, "label.new_name.new_name".G());
        EditorGUILayout.PropertyField(_changeOnInspector, "label.new_name.change_on_inspector".G());
        EditorGUILayout.HelpBox("label.new_name.info".Sf(_newName?.stringValue), MessageType.Info, true);
    }
}