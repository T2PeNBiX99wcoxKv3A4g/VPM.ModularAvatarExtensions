using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(ModularAvatarExtensionsMoveToRoot))]
[CanEditMultipleObjects]
public class MoveToRootEditor : MaexEditor
{
    protected override void OnInspectorGUIDraw()
    {
        EditorGUILayout.HelpBox("This object will be move to avatar root", MessageType.Info, true);
    }
}