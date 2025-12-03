using io.github.ykysnk.Localization.Editor;
using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(ModularAvatarExtensionsWorldScale))]
[CanEditMultipleObjects]
public class WorldScaleEditor : MaexEditor
{
    protected override void OnMaexInspectorGUI()
    {
        EditorGUILayout.HelpBox("label.world_scale.info".L(LocalizationID), MessageType.Info);
    }
}