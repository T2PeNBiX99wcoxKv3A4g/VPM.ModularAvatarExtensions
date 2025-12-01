using io.github.ykysnk.Localization.Editor;
using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(ModularAvatarExtensionsViewPosition))]
[CanEditMultipleObjects]
public class ViewPositionEditor : MaexEditor
{
    protected override void OnMaexInspectorGUI()
    {
        var viewPosition = (ModularAvatarExtensionsViewPosition)target;

        if (viewPosition.avatarDescriptor == null)
            EditorGUILayout.HelpBox("label.view_position.info".L(Util.LocalizationID), MessageType.Error, true);

        EditorGUILayout.HelpBox("label.view_position.info2".L(Util.LocalizationID), MessageType.Info, true);
    }
}