using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(ModularAvatarExtensionsTurnOnInBuild))]
public class TurnOnInBuildEditor : MaexEditor
{
    protected override void OnInspectorGUIDraw()
    {
        EditorGUILayout.HelpBox("This object will be turn on in avatar building", MessageType.Info, true);
    }
}