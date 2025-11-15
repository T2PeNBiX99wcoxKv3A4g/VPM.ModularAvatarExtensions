using io.github.ykysnk.utils;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Modular Avatar EX/MAEX New Name")]
    public class NewName : AvatarMaexComponent
    {
        public string? newName;
        public bool changeOnInspector;

        protected override void OnChange(bool isValidate)
        {
            if (!isValidate) return;
            if (string.IsNullOrEmpty(newName))
                newName = gameObject.name;
            if (gameObject.name == newName || !changeOnInspector || Utils.IsInPrefab()) return;
            gameObject.name = newName!;
        }
    }
}