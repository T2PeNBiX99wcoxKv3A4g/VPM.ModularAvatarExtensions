using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(ModularAvatarExtensionsViewPosition))]
[CanEditMultipleObjects]
public class ViewPositionEditor : MaexEditor
{
    protected override void OnInspectorGUIDraw()
    {
        var viewPosition = (ModularAvatarExtensionsViewPosition)target;

        if (viewPosition.avatarDescriptor == null)
            EditorGUILayout.HelpBox("Please add any avatar descriptor in avatar root", MessageType.Error, true);

        EditorGUILayout.HelpBox("This object will be follow the view position", MessageType.Info, true);
    }
}