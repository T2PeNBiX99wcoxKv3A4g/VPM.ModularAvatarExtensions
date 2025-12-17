using io.github.ykysnk.utils.Editor;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    internal abstract class MaexEditor : BasicEditor
    {
        protected override void OnErrorHandleInspectorGUI()
        {
            OnInnerInspectorGUI();
            EditorGUILayout.Separator();
            InternalLocalizationExtensions.Helper.SelectLanguageGUI();
        }

        protected override VisualElement? CreateErrorHandleInspectorGUI()
        {
            var tree = CreateInnerInspectorGUI();
            if (tree == null) return null;
            var root = new VisualElement();
            root.Add(tree);
            root.Bind(serializedObject);
            InternalLocalizationExtensions.Helper.UILocalize(root);
            return root;
        }

        [PublicAPI]
        protected abstract void OnInnerInspectorGUI();

        [PublicAPI]
        protected virtual VisualElement? CreateInnerInspectorGUI() => null;
    }
}