using io.github.ykysnk.utils.Editor;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

public abstract class RootTransformPathEditorBase<T> : UnityEditor.Editor where T : Component
{
    [PublicAPI] protected const string ReferenceProp = "reference";
    [PublicAPI] protected const string ComponentProp = "component";
    [PublicAPI] protected SerializedProperty? Component;
    [PublicAPI] protected SerializedProperty? Reference;

    protected abstract string ComponentLabel { get; }
    protected abstract string RootTransformType { get; }

    protected virtual void OnEnable()
    {
        Reference = serializedObject.FindProperty(ReferenceProp);
        Component = serializedObject.FindProperty(ComponentProp);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();

        var component = (RootTransformPathBase<T>)target;

        component.OnInspectorGUI();

        EditorGUILayout.PropertyField(Component, Utils.Label(ComponentLabel));
        EditorGUILayout.PropertyField(Reference, Utils.Label("Root Transform"));
        OnInspectorGUIDraw();
        EditorGUILayout.HelpBox(
            string.IsNullOrEmpty(component?.reference?.referencePath)
                ? $"Input any object want to become {RootTransformType} root transform"
                : $"Object of '{component?.reference?.referencePath}' will become {RootTransformType} root transform",
            MessageType.Info,
            true);

        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
    }

    protected virtual void OnInspectorGUIDraw()
    {
    }
}