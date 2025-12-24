using io.github.ykysnk.utils.Extensions;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Modular Avatar EX/MAEX New Name")]
    public class ModularAvatarExtensionsNewName : AvatarMaexComponent
    {
        public string? newName;
        public bool changeOnInspector;

        protected override void OnChange()
        {
            if (string.IsNullOrEmpty(newName))
                newName = gameObject.name;
            if (gameObject.name == newName || !changeOnInspector || !gameObject.IsSceneObject()) return;
            gameObject.name = newName!;
        }
    }
}