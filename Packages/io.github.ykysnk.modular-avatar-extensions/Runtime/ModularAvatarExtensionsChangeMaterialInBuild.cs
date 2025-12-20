using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Modular Avatar EX/MAEX Change Material In Build")]
    public class ModularAvatarExtensionsChangeMaterialInBuild : AvatarMaexComponent
    {
        [SerializeField] private new Renderer? renderer;

        [SerializeField] private MaterialChangeData[] materialChangeDatas =
        {
        };

        public Renderer? Renderer => renderer;

        public MaterialChangeData[] MaterialChangeDatas => materialChangeDatas;

        protected override void OnChange()
        {
            renderer = GetComponent<Renderer>();
        }
    }
}