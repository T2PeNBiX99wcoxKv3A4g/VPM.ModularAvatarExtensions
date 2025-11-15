using System;
using io.github.ykysnk.utils.Editor;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[PublicAPI]
public abstract class RootTransformPathEditorBase<T> : MaexEditor where T : Component
{
    protected const string ReferenceProp = "reference";
    protected const string ComponentProp = "component";
    protected SerializedProperty? Component;
    protected SerializedProperty? Reference;

    protected abstract string ComponentLabel { get; }
    protected abstract string RootTransformType { get; }

    protected override void OnEnable()
    {
        Reference = serializedObject.FindProperty(ReferenceProp);
        Component = serializedObject.FindProperty(ComponentProp);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();

        try
        {
            var component = (RootTransformPathBase<T>)target;
            var count = component.GetComponents<T>().Length;

            if (count > 1)
                EditorGUILayout.PropertyField(Component, Utils.Label(ComponentLabel));
            EditorGUILayout.PropertyField(Reference, Utils.Label("Root Transform"));

            OnInspectorGUIDraw();

            EditorGUILayout.HelpBox(
                string.IsNullOrEmpty(component.reference?.referencePath)
                    ? $"Input any object want to become {RootTransformType} root transform"
                    : $"Object of '{component.reference?.referencePath}' will become {RootTransformType} root transform",
                MessageType.Info,
                true);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            EditorGUILayout.HelpBox(
                $"Editor Error: {e.Message}\n{e.StackTrace}",
                MessageType.Error, true);
        }

        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
    }
}