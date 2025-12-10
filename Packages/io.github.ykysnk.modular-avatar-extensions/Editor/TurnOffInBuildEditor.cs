using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [CustomEditor(typeof(ModularAvatarExtensionsTurnOffInBuild))]
    [CanEditMultipleObjects]
    internal class TurnOffInBuildEditor : MaexEditor
    {
        protected override void OnInnerInspectorGUI()
        {
            EditorGUILayout.HelpBox("label.turn_off_in_build.info".S(), MessageType.Info, true);
        }
    }
}