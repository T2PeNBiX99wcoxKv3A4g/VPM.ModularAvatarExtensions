using io.github.ykysnk.Localization.Editor;
using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(ModularAvatarExtensionsNewName))]
[CanEditMultipleObjects]
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

    protected override void OnMaexInspectorGUI()
    {
        EditorGUILayout.PropertyField(_newName, "label.new_name.new_name".G(Util.LocalizationID));
        EditorGUILayout.PropertyField(_changeOnInspector, "label.new_name.change_on_inspector".G(Util.LocalizationID));
        EditorGUILayout.HelpBox(string.Format("label.new_name.info".L(Util.LocalizationID), _newName?.stringValue),
            MessageType.Info, true);
    }
}