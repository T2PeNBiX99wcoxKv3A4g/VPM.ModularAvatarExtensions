using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(ModularAvatarExtensionsWorldScale))]
[CanEditMultipleObjects]
public class WorldScaleEditor : MaexEditor
{
    private const string EditorOnlyProp = "editorOnly";

    private SerializedProperty? _editorOnly;

    protected override void OnEnable()
    {
        _editorOnly = serializedObject.FindProperty(EditorOnlyProp);
    }

    protected override void OnInnerInspectorGUI()
    {
        EditorGUILayout.PropertyField(_editorOnly, "label.world_scale.editor_only".G());
        EditorGUILayout.HelpBox("label.world_scale.info".S(), MessageType.Info);
    }
}