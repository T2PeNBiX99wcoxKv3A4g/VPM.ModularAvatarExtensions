#if MAEX_VRCSDK3_BASE
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [CustomEditor(typeof(ModularAvatarExtensionsLocalOnlyObject))]
    [CanEditMultipleObjects]
    internal class LocalOnlyObjectEditor : MaexEditor
    {
        [SerializeField] private VisualTreeAsset? uxml;

        protected override VisualElement? CreateInnerInspectorGUI()
        {
            var tree = uxml!.CloneTree();
            return tree;
        }
    }
}
#endif