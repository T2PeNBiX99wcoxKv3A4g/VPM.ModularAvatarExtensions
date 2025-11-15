using io.github.ykysnk.autohook;
using io.github.ykysnk.utils;
using JetBrains.Annotations;
using nadena.dev.modular_avatar.core;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    public abstract class RootTransformPathBase<T> : AvatarMaexComponent, IRootTransformPathBase where T : Component
    {
        [Autohook] public T? component;
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

        protected override void OnChange(bool isValidate)
        {
            if (isValidate)
                if (!component)
                    component = GetComponent<T>();
            SetPath();
        }

        protected virtual bool CheckIsValid() => !string.IsNullOrEmpty(reference?.referencePath);

        protected virtual void SetPath()
        {
#if UNITY_EDITOR
            var isInPrefab = Utils.IsInPrefab();
            if (isInPrefab || reference == null) return;
            var obj = reference.Get(this);
            if (!obj) return;
            var getTransform = obj.transform;
            if (!component) return;
            var proxy = new RootTransformProxy(component!, RootTransformFieldName);
            if (proxy.rootTransform == getTransform) return;
            proxy.rootTransform = getTransform;
#endif
        }
    }
}