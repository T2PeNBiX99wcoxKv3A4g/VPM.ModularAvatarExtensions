#if MAEX_VRCSDK3_BASE
using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [CustomEditor(typeof(ModularAvatarExtensionsViewPosition))]
    [CanEditMultipleObjects]
    internal class ViewPositionEditor : MaexEditor
    {
        private const string IsLockProp = "isLock";

        private SerializedProperty? _isLock;

        protected override void OnEnable()
        {
            _isLock = serializedObject.FindProperty(IsLockProp);
        }

        protected override void OnInnerInspectorGUI()
        {
            var viewPosition = (ModularAvatarExtensionsViewPosition)target;
            if (viewPosition.avatarDescriptor == null)
                EditorGUILayout.HelpBox("label.view_position.info".S(), MessageType.Error, true);

            EditorGUILayout.PropertyField(_isLock, "label.view_position.is_lock".G());
            EditorGUILayout.HelpBox("label.view_position.info2".S(), MessageType.Info, true);
        }
    }
#endif
}