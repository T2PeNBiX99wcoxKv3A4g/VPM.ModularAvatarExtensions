using System;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [PublicAPI]
    internal abstract class RootTransformPathEditorBase<T> : MaexEditor where T : Component
    {
        protected const string ReferenceProp = "reference";
        protected const string ComponentProp = "component";
        protected SerializedProperty? Component;
        protected SerializedProperty? Reference;

        protected abstract string RootTransformType { get; }

        protected override void OnEnable()
        {
            Reference = serializedObject.FindProperty(ReferenceProp);
            Component = serializedObject.FindProperty(ComponentProp);
        }

        public override void OnInspectorGUI()
        {
            if (IsBaseOnOldInspectorGUI)
                base.OnInspectorGUI();

            serializedObject.Update();
            EditorGUI.BeginChangeCheck();

            try
            {
                var component = (RootTransformPathBase<T>)target;
                var count = component.GetComponents<T>().Length;

                if (count > 1)
                    EditorGUILayout.PropertyField(Component,
                        $"label.root_transform_path_base.{RootTransformType}.component".G());
                EditorGUILayout.PropertyField(Reference,
                    "label.root_transform_path_base.root_transform".G());

                OnInnerInspectorGUI();

                EditorGUILayout.HelpBox(
                    string.IsNullOrEmpty(component.reference?.referencePath)
                        ? $"label.root_transform_path_base.{RootTransformType}.info".S()
                        : string.Format($"label.root_transform_path_base.{RootTransformType}.info2".S(),
                            component.reference?.referencePath),
                    MessageType.Info,
                    true);
                InternalLocalizationExtensions.Helper.SelectLanguageGUI();
            }
            catch (Exception e)
            {
                if (ConsoleLog)
                    Debug.LogException(e);
                OnError(e, Type.UGUI);
                EditorGUILayout.HelpBox($"Editor Error: {e.Message}\n{e.StackTrace}", MessageType.Error, true);
            }

            if (!EditorGUI.EndChangeCheck())
                return;
            OnChange();
            serializedObject.ApplyModifiedProperties();
        }

        protected override void OnInnerInspectorGUI()
        {
        }
    }
}