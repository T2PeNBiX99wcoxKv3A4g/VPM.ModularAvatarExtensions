using io.github.ykysnk.utils.Editor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

public abstract class MaexEditor : BasicEditor
{
    protected override void OnErrorHandleInspectorGUI()
    {
        OnInnerInspectorGUI();
        InternalLocalizationExtensions.Helper.SelectLanguageGUI();
    }

    protected abstract void OnInnerInspectorGUI();
}