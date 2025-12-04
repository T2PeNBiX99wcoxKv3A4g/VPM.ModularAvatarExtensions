using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(ModularAvatarExtensionsMoveToRoot))]
[CanEditMultipleObjects]
public class MoveToRootEditor : MaexEditor
{
    protected override void OnInnerInspectorGUI()
    {
        EditorGUILayout.HelpBox("label.move_to_root.info".S(), MessageType.Info, true);
    }
}