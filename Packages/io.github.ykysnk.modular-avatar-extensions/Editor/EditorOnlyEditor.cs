using io.github.ykysnk.Localization.Editor;
using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(ModularAvatarExtensionsEditorOnly))]
[CanEditMultipleObjects]
public class EditorOnlyEditor : MaexEditor
{
    protected override void OnMaexInspectorGUI()
    {
        EditorGUILayout.HelpBox("label.editor_only.info".L(LocalizationID), MessageType.Info, true);
    }
}