using JetBrains.Annotations;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [DisallowMultipleComponent]
    public abstract class TurnInBuild : AvatarMaexComponent
    {
        [SerializeField] private bool isEarly;

        [PublicAPI] public bool IsEarly => isEarly;
    }
}