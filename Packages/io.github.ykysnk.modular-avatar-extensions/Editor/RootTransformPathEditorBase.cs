using JetBrains.Annotations;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [PublicAPI]
    internal abstract class RootTransformPathEditorBase<T> : MaexEditor where T : Component
    {
        [SerializeField] protected VisualTreeAsset? uxml;

        protected override VisualElement CreateInnerInspectorGUI()
        {
            var tree = uxml!.CloneTree();
            var componentNotFoundField = tree.Q<HelpBox>("componentNotFound");
            var componentField = tree.Q<ObjectField>("component");
            componentField.schedule.Execute(SetDisplay).Every(100);
            SetDisplay();

            return tree;

            void SetDisplay()
            {
                if (target == null || target is not RootTransformPathBase<T> rootTransformPathBase) return;
                componentNotFoundField.style.display = rootTransformPathBase.GetComponents<T>().Length < 1
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;
                componentField.style.display = rootTransformPathBase.GetComponents<T>().Length is < 1 or > 1
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;
            }
        }
    }
}