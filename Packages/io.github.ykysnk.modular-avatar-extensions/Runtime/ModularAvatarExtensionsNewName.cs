using io.github.ykysnk.utils.Extensions;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

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
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(newName))
                newName = gameObject.name;
            if (gameObject.name == newName || !changeOnInspector || !gameObject.IsSceneObject()) return;
            Undo.RecordObjects(new Object[]
            {
                gameObject, this
            }, "Change Object Name");
            gameObject.name = newName!;
            EditorUtility.SetDirty(gameObject);
            EditorUtility.SetDirty(this);
#endif
        }
    }
}