using io.github.ykysnk.Localization.Editor;
using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(ModularAvatarExtensionsViewPosition))]
[CanEditMultipleObjects]
public class ViewPositionEditor : MaexEditor
{
    private const string IsLockProp = "isLock";

    private SerializedProperty? _isLock;

    protected override void OnEnable()
    {
        _isLock = serializedObject.FindProperty(IsLockProp);
    }

    protected override void OnMaexInspectorGUI()
    {
        var viewPosition = (ModularAvatarExtensionsViewPosition)target;
        if (viewPosition.avatarDescriptor == null)
            EditorGUILayout.HelpBox("label.view_position.info".L(LocalizationID), MessageType.Error, true);

        EditorGUILayout.PropertyField(_isLock, "label.view_position.is_lock".G(LocalizationID));
        EditorGUILayout.HelpBox("label.view_position.info2".L(LocalizationID), MessageType.Info, true);
    }
}