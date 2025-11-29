using System.Linq;
using io.github.ykysnk.utils.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using VRC.Dynamics;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

internal static class ConstraintDisablerMenu
{
    private const string MenuPath = "GameObject/Modular Avatar EX/Add Constraint Disabler";

    [MenuItem(MenuPath, false, 10)]
    private static void Menu(MenuCommand menuCommand)
    {
        var obj = menuCommand.context as GameObject;

        if (obj == null)
        {
            EditorUtility.DisplayDialog("Error", "Game Object is null", "OK");
            return;
        }

        var components = obj.GetComponentsInChildren<Component>(true)
            .Where(c => c is VRCConstraintBase or IConstraint && c != null).ToArray();

        foreach (var component in components)
        {
            if (component.TryGetComponent<ModularAvatarExtensionsConstraintDisabler>(out _)) continue;
            Undo.RecordObject(component, $"{component.FullName()} change");
            Undo.AddComponent<ModularAvatarExtensionsConstraintDisabler>(component.gameObject);
        }
    }
}