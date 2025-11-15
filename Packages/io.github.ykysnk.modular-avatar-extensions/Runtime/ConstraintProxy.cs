#if UNITY_EDITOR
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [PublicAPI]
    public readonly struct ConstraintProxy
    {
        private readonly Component _component;
        private readonly string _propertyName;

        public ConstraintProxy(Component component, string propertyName = nameof(constraintActive))
        {
            _component = component;
            _propertyName = propertyName;
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public bool constraintActive
        {
            get => Traverse.Create(_component).Property<bool>(_propertyName).Value;
            set => Traverse.Create(_component).Property<bool>(_propertyName).Value = value;
        }
    }
}
#endif