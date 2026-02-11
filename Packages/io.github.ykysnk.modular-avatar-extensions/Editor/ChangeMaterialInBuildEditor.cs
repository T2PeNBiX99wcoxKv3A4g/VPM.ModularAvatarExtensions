using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [CustomEditor(typeof(ModularAvatarExtensionsChangeMaterialInBuild))]
    [CanEditMultipleObjects]
    internal class ChangeMaterialInBuildEditor : MaexEditor
    {
        [SerializeField] private VisualTreeAsset? uxml;

        private SerializedProperty? _materialChangeDatas;

        protected override void OnEnable()
        {
            _materialChangeDatas = serializedObject.FindProperty("materialChangeDatas");
        }

        protected override VisualElement CreateInnerInspectorGUI()
        {
            var tree = uxml!.CloneTree();
            return tree;
        }

        protected override void OnInnerInspectorGUI()
        {
            GUI.enabled = false;
            EditorGUILayout.PropertyField(_materialChangeDatas);
        }
    }
}