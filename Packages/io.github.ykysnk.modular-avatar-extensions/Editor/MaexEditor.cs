using io.github.ykysnk.utils.Editor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

public abstract class MaexEditor : BasicEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        foreach (var target2 in targets)
        {
            var component = (AvatarMaexComponent)target2;
            component.OnInspectorGUI();
        }
    }
}