using io.github.ykysnk.Localization.Editor;
using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(ModularAvatarExtensionsMoveToRootOfReference))]
[CanEditMultipleObjects]
public class MoveToRootOfReferenceEditor : MaexEditor
{
    private const string ReferenceProp = "reference";
    private SerializedProperty? _reference;

    protected override void OnEnable()
    {
        _reference = serializedObject.FindProperty(ReferenceProp);
    }

    protected override void OnMaexInspectorGUI()
    {
        var component = (ModularAvatarExtensionsMoveToRootOfReference)target;

        EditorGUILayout.PropertyField(_reference, "label.move_to_root_of_reference.reference".G(Util.LocalizationID));
        EditorGUILayout.HelpBox(
            string.IsNullOrEmpty(component?.reference?.referencePath)
                ? "label.move_to_root_of_reference.info".L(Util.LocalizationID)
                : string.Format("label.move_to_root_of_reference.info2".L(Util.LocalizationID),
                    component?.reference?.referencePath), MessageType.Info,
            true);
    }
}