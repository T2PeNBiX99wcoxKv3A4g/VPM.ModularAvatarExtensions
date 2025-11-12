#if UNITY_EDITOR
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    public readonly struct RootTransformProxy
    {
        private readonly Component _component;
        private readonly string _rootTransformFieldName;

        public RootTransformProxy(Component component, string rootTransformFieldName = "rootTransform")
        {
            _component = component;
            _rootTransformFieldName = rootTransformFieldName;
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public Transform rootTransform
        {
            get => Traverse.Create(_component).Field<Transform>(nameof(_rootTransformFieldName)).Value;
            set => Traverse.Create(_component).Field<Transform>(nameof(_rootTransformFieldName)).Value = value;
        }
    }
}
#endif