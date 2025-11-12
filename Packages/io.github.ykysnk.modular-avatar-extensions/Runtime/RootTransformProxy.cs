using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions;

public readonly struct RootTransformProxy
{
    private readonly Component _component;

    public RootTransformProxy(Component component) => _component = component;

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public Transform rootTransform
    {
        get => Traverse.Create(_component).Field<Transform>(nameof(rootTransform)).Value;
        set => Traverse.Create(_component).Field<Transform>(nameof(rootTransform)).Value = value;
    }
}