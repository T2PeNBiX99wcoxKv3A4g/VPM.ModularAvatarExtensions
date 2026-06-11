using JetBrains.Annotations;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [DisallowMultipleComponent]
    public abstract class TurnInBuild : AvatarMaexComponent
    {
        [SerializeField] private bool isEarly;
        [SerializeField] private bool preview = true;

        [PublicAPI] public bool IsEarly => isEarly;
        [PublicAPI] public bool Preview => preview;
    }
}