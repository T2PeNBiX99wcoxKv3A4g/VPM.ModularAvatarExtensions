using io.github.ykysnk.utils.Editor;
using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(MoveToRootOfReference))]
public class MoveToRootOfReferenceEditor : MaexEditor
{
    private const string ReferenceProp = "reference";
    private SerializedProperty? _reference;

    private void OnEnable()
    {
        _reference = serializedObject.FindProperty(ReferenceProp);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();

        var component = (MoveToRootOfReference)target;

        EditorGUILayout.PropertyField(_reference, Utils.Label("Move Transform"));
        EditorGUILayout.HelpBox(
            string.IsNullOrEmpty(component?.reference?.referencePath)
                ? "Input any object want to move to avatar root"
                : $"Object of '{component?.reference?.referencePath}' will be move to avatar root", MessageType.Info,
            true);

        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
    }
}