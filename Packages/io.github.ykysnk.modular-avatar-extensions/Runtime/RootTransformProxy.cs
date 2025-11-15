#if UNITY_EDITOR
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [PublicAPI]
    public readonly struct RootTransformProxy
    {
        private readonly Component _component;
        private readonly string _fieldName;

        public RootTransformProxy(Component component, string fieldName = nameof(rootTransform))
        {
            _component = component;
            _fieldName = fieldName;
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public Transform? rootTransform
        {
            get => Traverse.Create(_component).Field<Transform?>(_fieldName).Value;
            set => Traverse.Create(_component).Field<Transform?>(_fieldName).Value = value;
        }
    }
}
#endif