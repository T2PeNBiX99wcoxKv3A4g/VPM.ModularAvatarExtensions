using JetBrains.Annotations;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Modular Avatar EX/MAEX Layer Index Control")]
    // ReSharper disable once RequiredBaseTypesIsNotInherited
    public class ModularAvatarExtensionsLayerIndexControl : AvatarMaexStateMachine
    {
        [SerializeField] private int layerIndex;

        [PublicAPI]
        public int LayerIndex
        {
            get => layerIndex;
            set => layerIndex = value;
        }
    }
}