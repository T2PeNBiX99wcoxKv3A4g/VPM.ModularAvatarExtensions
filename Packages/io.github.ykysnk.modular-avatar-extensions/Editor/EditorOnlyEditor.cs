using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(ModularAvatarExtensionsEditorOnly))]
public class EditorOnlyEditor : MaexEditor
{
    protected override void OnInspectorGUIDraw()
    {
        EditorGUILayout.HelpBox(
            "This object will be mark as editor only, all bone proxy or constraint will be remove. (If want to remove this object in build, just change tag to 'EditorOnly')",
            MessageType.Info, true);
    }
}