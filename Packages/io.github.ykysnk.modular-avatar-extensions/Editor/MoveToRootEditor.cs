using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [CustomEditor(typeof(ModularAvatarExtensionsMoveToRoot))]
    [CanEditMultipleObjects]
    internal class MoveToRootEditor : MaexEditor
    {
        [SerializeField] private VisualTreeAsset? uxml;

        protected override VisualElement? CreateInnerInspectorGUI()
        {
            var tree = uxml!.CloneTree();
            var newPath = tree.Q<TextField>("newPath");
            newPath.schedule.Execute(SetNewPath).Every(100);
            SetNewPath();
            return tree;

            void SetNewPath()
            {
                if (target is not ModularAvatarExtensionsMoveToRoot moveToRoot) return;
                newPath.value = $"{moveToRoot.transform.root.name}/{moveToRoot.name}";
            }
        }
    }
}