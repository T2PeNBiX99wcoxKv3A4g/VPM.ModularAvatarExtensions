using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(ModularAvatarExtensionsEditorOnly))]
[CanEditMultipleObjects]
internal class EditorOnlyEditor : MaexEditor
{
    protected override void OnInnerInspectorGUI()
    {
        EditorGUILayout.HelpBox("label.editor_only.info".S(), MessageType.Info, true);
    }
}