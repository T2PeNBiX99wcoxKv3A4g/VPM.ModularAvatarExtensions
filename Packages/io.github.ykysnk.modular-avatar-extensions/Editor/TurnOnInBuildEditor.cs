using io.github.ykysnk.Localization.Editor;
using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(ModularAvatarExtensionsTurnOnInBuild))]
[CanEditMultipleObjects]
public class TurnOnInBuildEditor : MaexEditor
{
    protected override void OnMaexInspectorGUI()
    {
        EditorGUILayout.HelpBox("label.turn_on_in_build.info".L(LocalizationID), MessageType.Info, true);
    }
}