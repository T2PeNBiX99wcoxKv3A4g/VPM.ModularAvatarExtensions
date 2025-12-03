using io.github.ykysnk.Localization.Editor;
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

    protected override void OnMaexInspectorGUI()
    {
        EditorGUILayout.PropertyField(_editorOnly, "label.world_scale.editor_only".G(LocalizationID));
        EditorGUILayout.HelpBox("label.world_scale.info".L(LocalizationID), MessageType.Info);
    }
}