using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SkinnedMeshRenderer))]
// [AddComponentMenu("yky/ModularAvatarEX/Shapes Changer")]
    public class ShapesChanger : AvatarMaexComponent
    {
        public string[]? shapes;
    }
}