using io.github.ykysnk.utils.Editor;
using JetBrains.Annotations;
using UnityEditor;
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
            if (CreateInnerInspectorGUI() == null) return null;
            // TODO
            return CreateInnerInspectorGUI();
        }

        [PublicAPI]
        protected abstract void OnInnerInspectorGUI();

        [PublicAPI]
        protected virtual VisualElement? CreateInnerInspectorGUI() => null;
    }
}