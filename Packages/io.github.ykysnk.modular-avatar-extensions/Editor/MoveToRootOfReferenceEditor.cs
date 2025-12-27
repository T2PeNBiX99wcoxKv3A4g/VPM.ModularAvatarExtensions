using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [CustomEditor(typeof(ModularAvatarExtensionsMoveToRootOfReference))]
    [CanEditMultipleObjects]
    internal class MoveToRootOfReferenceEditor : MaexEditor
    {
        private const string ReferenceProp = "reference";
        [SerializeField] private VisualTreeAsset? uxml;
        private SerializedProperty? _reference;

        protected override void OnEnable()
        {
            _reference = serializedObject.FindProperty(ReferenceProp);
        }

        protected override VisualElement CreateInnerInspectorGUI()
        {
            var tree = uxml!.CloneTree();
            var newPath = tree.Q<TextField>("newPath");
            var reference = tree.Q<PropertyField>("reference");
            reference.label = "";
            reference.RegisterValueChangeCallback(_ => SetNewPath());
            newPath.schedule.Execute(SetNewPath).Every(100);
            SetNewPath();
            return tree;

            void SetNewPath()
            {
                if (target == null ||
                    target is not ModularAvatarExtensionsMoveToRootOfReference moveToRootOfReference) return;
                var setName = moveToRootOfReference.reference?.Get(moveToRootOfReference)?.name;
                if (string.IsNullOrEmpty(setName))
                    setName = "None";

                newPath.value = $"{moveToRootOfReference.transform.root.name}/{setName}";
            }
        }

        protected override void OnInnerInspectorGUI()
        {
            var component = (ModularAvatarExtensionsMoveToRootOfReference)target;

            EditorGUILayout.PropertyField(_reference, "label.move_to_root_of_reference.reference".G());
            EditorGUILayout.HelpBox(
                string.IsNullOrEmpty(component?.reference?.referencePath)
                    ? "label.move_to_root_of_reference.info".S()
                    : string.Format("label.move_to_root_of_reference.info2".S(), component?.reference?.referencePath),
                MessageType.Info, true);
        }
    }
}