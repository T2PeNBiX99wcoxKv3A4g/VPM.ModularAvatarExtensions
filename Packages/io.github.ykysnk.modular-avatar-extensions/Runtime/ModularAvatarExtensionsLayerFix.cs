using JetBrains.Annotations;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Modular Avatar EX/MAEX Layer Fix")]
    public class ModularAvatarExtensionsLayerFix : AvatarMaexComponent
    {
        [SerializeField] private bool fixMmdLayer;

        [PublicAPI] public bool FixMmdLayer => fixMmdLayer;
    }
}