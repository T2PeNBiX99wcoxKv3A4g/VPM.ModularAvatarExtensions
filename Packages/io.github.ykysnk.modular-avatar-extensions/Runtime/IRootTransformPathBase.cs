using JetBrains.Annotations;
using nadena.dev.modular_avatar.core;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [PublicAPI]
    public interface IRootTransformPathBase
    {
        AvatarObjectReference? Reference { get; set; }
        Component? Component { get; set; }
        bool IsValid();
    }
}