using System.Linq;
using Cysharp.Threading.Tasks;
using io.github.ykysnk.utils.Editor;
using io.github.ykysnk.utils.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
#if MAEX_VRCSDK3_BASE
using VRC.Dynamics;
#endif

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    internal static class ConstraintDisablerMenu
    {
        [MenuItem("GameObject/Modular Avatar EX/Add Constraint Disabler", false, Util.GameObjectMenuPriority)]
        private static void Menu(MenuCommand menuCommand)
        {
            var obj = menuCommand.context as GameObject;
            MenuAsync(obj).Forget();
        }

        private static async UniTask MenuAsync(GameObject? obj)
        {
            if (obj == null)
            {
                await EditorUtils.DisplayDialogAsync("Error", "Game Object is null");
                return;
            }

            var components = obj.GetComponentsInChildren<Component>(true)
                .Where(c => c is
#if MAEX_VRCSDK3_BASE
                                VRCConstraintBase or
#endif
                                IConstraint
#if MAEX_VRCSDK3_BASE
                            && c != null
#endif
                ).ToArray();

            foreach (var component in components)
            {
                if (component.TryGetComponent<ModularAvatarExtensionsConstraintDisabler>(out _)) continue;
                Undo.RecordObject(component, $"{component.FullName()} change");
                Undo.AddComponent<ModularAvatarExtensionsConstraintDisabler>(component.gameObject);
                await UniTask.DelayFrame(10);
            }
        }
    }
}