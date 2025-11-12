using nadena.dev.modular_avatar.core;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions;

public interface IRootTransformPathBase
{
    AvatarObjectReference? Reference { get; set; }
    Component? Component { get; set; }
}