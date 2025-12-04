using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(ModularAvatarExtensionsTurnOnInBuild))]
[CanEditMultipleObjects]
internal class TurnOnInBuildEditor : MaexEditor
{
    protected override void OnInnerInspectorGUI()
    {
        EditorGUILayout.HelpBox("label.turn_on_in_build.info".S(), MessageType.Info, true);
    }
}