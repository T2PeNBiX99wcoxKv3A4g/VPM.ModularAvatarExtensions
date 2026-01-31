using io.github.ykysnk.ModularAvatarExtensions.Editor.UIElements;
using JetBrains.Annotations;
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
            var componentField = tree.Q<ValidatedObjectField>("component");
            componentField.schedule.Execute(SetDisplay).Every(100);
            SetDisplay();

            return tree;

            void SetDisplay()
            {
                if (target == null || target is not RootTransformPathBase<T> rootTransformPathBase) return;
                componentField.AutoHideIfSameGameObject(rootTransformPathBase.gameObject);
            }
        }
    }
}