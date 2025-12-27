using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [CustomEditor(typeof(ModularAvatarExtensionsNewName))]
    [CanEditMultipleObjects]
    internal class NewNameEditor : MaexEditor
    {
        [SerializeField] private VisualTreeAsset? uxml;

        protected override VisualElement CreateInnerInspectorGUI()
        {
            var tree = uxml!.CloneTree();
            var newNameField = tree.Q<TextField>("newName");
            var oldNameObjectField = tree.Q<ObjectField>("oldNameObject");
            oldNameObjectField.SetEnabled(false);
            var newNameObjectField = tree.Q<ObjectField>("newNameObject");
            newNameObjectField.SetEnabled(false);
            tree.schedule.Execute(SetName).Every(100);
            SetName();
            return tree;

            void SetName()
            {
                if (target == null || target is not ModularAvatarExtensionsNewName newName) return;
                oldNameObjectField.value = newName.gameObject;
                newNameObjectField.value = newName.gameObject;
                var newNameObjectFieldLabel = newNameObjectField.Q<Label>();
                newNameObjectFieldLabel.text = newNameField.value;
            }
        }
    }
}