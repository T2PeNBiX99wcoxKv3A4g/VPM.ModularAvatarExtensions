using io.github.ykysnk.utils;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Modular Avatar EX/MAEX New Name")]
    public class NewName : YkyEditorComponent
    {
        public string? newName;
        public bool changeOnInspector;

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(newName))
                newName = gameObject.name;
            if (gameObject.name == newName || !changeOnInspector || Utils.IsInPrefab()) return;
            gameObject.name = newName!;
        }
    }
}