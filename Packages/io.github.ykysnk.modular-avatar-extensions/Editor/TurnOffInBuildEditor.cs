using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(TurnOffInBuild))]
public class TurnOffInBuildEditor : MaexEditor
{
    protected override void OnInspectorGUIDraw()
    {
        EditorGUILayout.HelpBox("This object will be turn off in avatar building", MessageType.Info, true);
    }
}