using JetBrains.Annotations;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [DisallowMultipleComponent]
    public abstract class TurnInBuild : AvatarMaexComponent
    {
        [SerializeField] private bool isDelay;

        [PublicAPI] public bool IsDelay => isDelay;
    }
}