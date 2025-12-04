using io.github.ykysnk.utils.Editor;
using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

internal abstract class MaexEditor : BasicEditor
{
    protected override void OnErrorHandleInspectorGUI()
    {
        OnInnerInspectorGUI();
        EditorGUILayout.Separator();
        InternalLocalizationExtensions.Helper.SelectLanguageGUI();
    }

    protected abstract void OnInnerInspectorGUI();
}