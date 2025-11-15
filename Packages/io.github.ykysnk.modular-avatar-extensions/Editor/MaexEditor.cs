using System;
using UnityEditor;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

public abstract class MaexEditor : UnityEditor.Editor
{
    protected virtual void OnEnable()
    {
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();

        try
        {
            OnInspectorGUIDraw();
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

    protected virtual void OnInspectorGUIDraw()
    {
    }
}