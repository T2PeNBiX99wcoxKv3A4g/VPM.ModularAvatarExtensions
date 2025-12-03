using io.github.ykysnk.Localization.Editor;
using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(ModularAvatarExtensionsTurnOffInBuild))]
[CanEditMultipleObjects]
public class TurnOffInBuildEditor : MaexEditor
{
    protected override void OnMaexInspectorGUI()
    {
        EditorGUILayout.HelpBox("label.turn_off_in_build.info".L(LocalizationID), MessageType.Info, true);
    }
}