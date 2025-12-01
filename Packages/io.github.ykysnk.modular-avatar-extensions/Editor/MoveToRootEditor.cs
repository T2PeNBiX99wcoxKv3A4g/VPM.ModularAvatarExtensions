using io.github.ykysnk.Localization.Editor;
using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(ModularAvatarExtensionsMoveToRoot))]
[CanEditMultipleObjects]
public class MoveToRootEditor : MaexEditor
{
    protected override void OnMaexInspectorGUI()
    {
        EditorGUILayout.HelpBox("label.move_to_root.info".L(Util.LocalizationID), MessageType.Info, true);
    }
}