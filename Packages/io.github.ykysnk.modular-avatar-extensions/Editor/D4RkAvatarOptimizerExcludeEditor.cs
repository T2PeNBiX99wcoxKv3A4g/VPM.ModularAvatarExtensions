#if MAEX_D4RK_AVATAR_OPTIMIZER
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [CustomEditor(typeof(ModularAvatarExtensionsD4RkAvatarOptimizerExclude))]
    [CanEditMultipleObjects]
    internal class D4RkAvatarOptimizerExcludeEditor : MaexEditor
    {
        [SerializeField] private VisualTreeAsset? uxml;

        protected override VisualElement? CreateInnerInspectorGUI()
        {
            var tree = uxml!.CloneTree();
            return tree;
        }
    }
#endif
}