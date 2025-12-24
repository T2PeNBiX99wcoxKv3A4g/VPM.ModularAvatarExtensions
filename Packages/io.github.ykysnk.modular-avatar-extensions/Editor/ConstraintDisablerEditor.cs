using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
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

        protected override VisualElement? CreateInnerInspectorGUI()
        {
            var tree = uxml!.CloneTree();
            var constraint = tree.Q<ObjectField>("constraint");
            constraint.style.display = DisplayStyle.None;

            var constraintError = tree.Q<HelpBox>("constraintError");
            constraintError.style.display = DisplayStyle.None;

            tree.schedule.Execute(SetDisplay).Every(100);
            SetDisplay();
            return tree;

            void SetDisplay()
            {
                if (target is not ModularAvatarExtensionsConstraintDisabler component) return;
                var isConstraint = component.constraint is
#if MAEX_VRCSDK3_BASE
                    VRCConstraintBase or
#endif
                    IConstraint;
                var count = component.GetComponents<Component>().Count(c => c && c is
#if MAEX_VRCSDK3_BASE
                    VRCConstraintBase or
#endif
                    IConstraint);

                constraint.style.display = count > 1 || !isConstraint ? DisplayStyle.Flex : DisplayStyle.None;
                constraintError.style.display = count > 1 || !isConstraint ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }
    }
}