using io.github.ykysnk.ModularAvatarExtensions.Editor.UIElements;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;
#if MAEX_VRCSDK3_BASE
using VRC.Dynamics;
#endif

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [CustomEditor(typeof(ModularAvatarExtensionsConstraintDisabler))]
    [CanEditMultipleObjects]
    internal class ConstraintDisablerEditor : MaexEditor
    {
        [SerializeField] private VisualTreeAsset? uxml;

        protected override VisualElement CreateInnerInspectorGUI()
        {
            var tree = uxml!.CloneTree();
            var constraint = tree.Q<ValidatedObjectField>("constraint");
            constraint.AddValidator(value => value is
#if MAEX_VRCSDK3_BASE
                VRCConstraintBase or
#endif
                IConstraint);

            tree.schedule.Execute(SetDisplay).Every(1000);
            SetDisplay();
            return tree;

            void SetDisplay()
            {
                if (target == null || target is not ModularAvatarExtensionsConstraintDisabler component) return;
                constraint.AutoHideIfSameGameObject(component.gameObject);
            }
        }
    }
}