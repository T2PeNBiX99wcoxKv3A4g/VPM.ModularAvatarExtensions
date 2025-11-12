using System.Linq;
using nadena.dev.modular_avatar.core;
using UnityEditor;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

public static class RootTransformPathMenu
{
    private const string MenuPath = "GameObject/Modular Avatar EX/Add Root Transform Path";

    [MenuItem(MenuPath, false, 10)]
    private static void Menu(MenuCommand menuCommand)
    {
        var obj = menuCommand.context as GameObject;

        if (obj == null)
        {
            EditorUtility.DisplayDialog("Error", "Game Object is null", "OK");
            return;
        }

        var baseType = typeof(RootTransformPathBase<>);
        var assembly = baseType.Assembly;
        var types = assembly.GetTypes()
            .Where(t => t.IsClass && t is { IsAbstract: false, IsInterface: false })
            .Where(t => t.BaseType is { IsGenericType: true } && t.BaseType.GetGenericTypeDefinition() == baseType)
            .ToList();

        foreach (var type in types)
        {
            var typeDefinition = type.BaseType?.GetGenericArguments();

            if (typeDefinition == null || typeDefinition.Length < 1) continue;

            var findType = typeDefinition[0];
            var components = obj.GetComponentsInChildren(findType, true);

            foreach (var component in components)
            {
                var componentProxy = new RootTransformProxy(component);

                if (component.TryGetComponent(type, out _)) continue;

                var addComponent = component.gameObject.AddComponent(type);

                if (addComponent is not IRootTransformPathBase rootTransformPathBase) continue;

                var reference = new AvatarObjectReference();

                reference.Set(componentProxy.rootTransform.gameObject);
                rootTransformPathBase.Reference = reference;
            }
        }
    }
}