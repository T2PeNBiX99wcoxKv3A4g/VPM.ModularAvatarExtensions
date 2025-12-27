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
    }
#endif
}