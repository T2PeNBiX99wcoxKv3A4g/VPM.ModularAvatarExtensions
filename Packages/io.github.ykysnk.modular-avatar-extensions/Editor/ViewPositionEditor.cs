#if MAEX_VRCSDK3_BASE
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [CustomEditor(typeof(ModularAvatarExtensionsViewPosition))]
    [CanEditMultipleObjects]
    internal class ViewPositionEditor : MaexEditor
    {
        [SerializeField] private VisualTreeAsset? uxml;

        private SerializedProperty? _isLock;

        protected override void OnEnable()
        {
            _isLock = serializedObject.FindProperty("isLock");
        }

        protected override VisualElement CreateInnerInspectorGUI()
        {
            var tree = uxml!.CloneTree();
            var errorInfo = tree.Q<HelpBox>("errorInfo");
            errorInfo.schedule.Execute(SetDisplay).Every(100);
            SetDisplay();
            return tree;

            void SetDisplay()
            {
                if (target == null || target is not ModularAvatarExtensionsViewPosition viewPosition) return;
                errorInfo.style.display = viewPosition.avatarDescriptor == null ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        protected override void OnInnerInspectorGUI()
        {
            EditorGUILayout.PropertyField(_isLock, "label.view_position.is_lock".G());
        }
    }
#endif
}