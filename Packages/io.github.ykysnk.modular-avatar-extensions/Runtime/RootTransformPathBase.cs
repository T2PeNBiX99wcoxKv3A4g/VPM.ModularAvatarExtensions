using io.github.ykysnk.utils.Extensions;
using JetBrains.Annotations;
using nadena.dev.modular_avatar.core;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace io.github.ykysnk.ModularAvatarExtensions
{
    public abstract class RootTransformPathBase<T> : AvatarMaexComponent, IRootTransformPathBase where T : Component
    {
        public T? component;
        public AvatarObjectReference? reference;
        [PublicAPI] protected virtual string RootTransformFieldName => "rootTransform";

        public AvatarObjectReference? Reference
        {
            get => reference;
            set => reference = value;
        }

        public Component? Component
        {
            get => component;
            set => component = (T?)value;
        }

        public bool IsValid() => CheckIsValid();

        protected override void OnChange()
        {
            if (!component)
                component = GetComponent<T>();
            SetPath();
        }

        protected virtual bool CheckIsValid() => !string.IsNullOrEmpty(reference?.referencePath);

        protected virtual void SetPath()
        {
#if UNITY_EDITOR
            if (!gameObject.IsSceneObject() || reference == null) return;
            if (!component) return;
            var proxy = new RootTransformProxy(component!, RootTransformFieldName);
            var obj = reference.Get(this);
            if (!obj)
            {
                Undo.RecordObject(component, "Change Root Transform");
                proxy.rootTransform = null;
                EditorUtility.SetDirty(component);
                return;
            }

            var getTransform = obj.transform;
            if (proxy.rootTransform == getTransform) return;
            Undo.RecordObject(component, "Change Root Transform");
            proxy.rootTransform = getTransform;
            EditorUtility.SetDirty(component);
#endif
        }
    }
}