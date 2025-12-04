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

    protected override void OnInnerInspectorGUI()
    {
        var component = (ModularAvatarExtensionsMoveToRootOfReference)target;

        EditorGUILayout.PropertyField(_reference, "label.move_to_root_of_reference.reference".G());
        EditorGUILayout.HelpBox(
            string.IsNullOrEmpty(component?.reference?.referencePath)
                ? "label.move_to_root_of_reference.info".S()
                : string.Format("label.move_to_root_of_reference.info2".S(), component?.reference?.referencePath),
            MessageType.Info, true);
    }
}