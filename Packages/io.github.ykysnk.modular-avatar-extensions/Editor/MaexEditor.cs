using io.github.ykysnk.utils.Editor;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    internal abstract class MaexEditor : BasicEditor
    {
        private bool _showDebug;

        protected override void OnErrorHandleInspectorGUI()
        {
            OnInnerInspectorGUI();
            EditorGUILayout.Separator();
            InternalLocalizationExtensions.Helper.SelectLanguageGUI();

            if (!SessionState.GetBool("MAEX_DebugMode", false)) return;
            EditorGUILayout.Separator();
            _showDebug = EditorGUILayout.Foldout(_showDebug, "Debug UI");

            if (!_showDebug) return;
            var iterator = serializedObject.GetIterator();
            if (!iterator.NextVisible(true)) return;
            while (iterator.NextVisible(false))
                EditorGUILayout.PropertyField(iterator.Copy());
        }

        protected override VisualElement? CreateErrorHandleInspectorGUI()
        {
            var tree = CreateInnerInspectorGUI();
            if (tree == null) return null;
            var root = new VisualElement();
            root.Add(tree);
            root.Bind(serializedObject);
            InternalLocalizationExtensions.Helper.UILocalize(root);

            if (!SessionState.GetBool("MAEX_DebugMode", false)) return root;

            root.Add(new()
            {
                style =
                {
                    height = 10
                }
            });

            var debugFoldout = new Foldout
            {
                text = "Debug UI",
                value = false
            };

            var iterator = serializedObject.GetIterator();

            if (iterator.NextVisible(true))
                while (iterator.NextVisible(false))
                    debugFoldout.contentContainer.Add(new PropertyField(iterator.Copy()));

            root.Add(debugFoldout);
            return root;
        }

        [PublicAPI]
        protected abstract void OnInnerInspectorGUI();

        [PublicAPI]
        protected virtual VisualElement? CreateInnerInspectorGUI() => null;
    }
}