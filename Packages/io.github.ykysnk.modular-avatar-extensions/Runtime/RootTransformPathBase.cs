using HarmonyLib;
using io.github.ykysnk.autohook;
using io.github.ykysnk.utils;
using JetBrains.Annotations;
using nadena.dev.modular_avatar.core;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    public abstract class RootTransformPathBase<T> : YkyEditorComponent, IRootTransformPathBase where T : Component
    {
        [Autohook] public T? component;
        public AvatarObjectReference? reference;
        [PublicAPI] protected virtual string RootTransformFieldName => "rootTransform";

        [PublicAPI]
        protected Transform SetRootTransform
        {
            get => Traverse.Create(component).Field<Transform>(RootTransformFieldName).Value;
            set => Traverse.Create(component).Field<Transform>(RootTransformFieldName).Value = value;
        }

        protected virtual void OnValidate()
        {
            if (!component)
                component = GetComponent<T>();
            SetPath();
        }

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

        protected virtual void SetPath()
        {
            var isInPrefab = Utils.IsInPrefab();
            if (isInPrefab || reference == null) return;
            var obj = reference.Get(this);
            if (!obj) return;
            var getTransform = obj.transform;
            if (!component || SetRootTransform == getTransform) return;
            SetRootTransform = getTransform;
        }

        public override void OnInspectorGUI()
        {
            SetPath();
        }
    }
}