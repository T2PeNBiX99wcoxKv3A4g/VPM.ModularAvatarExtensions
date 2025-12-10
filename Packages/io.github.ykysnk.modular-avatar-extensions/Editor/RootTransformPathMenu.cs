using System.Linq;
using io.github.ykysnk.utils;
using io.github.ykysnk.utils.Extensions;
using nadena.dev.modular_avatar.core;
using UnityEditor;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    internal static class RootTransformPathMenu
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
                var components = obj.GetComponentsInChildren(findType, true) ?? new Component[]
                {
                };

                foreach (var component in components)
                {
                    var componentProxy = new RootTransformProxy(component);

                    if (component.TryGetComponent(type, out _)) continue;

                    Undo.RecordObject(component, $"{component.FullName()} change");

                    var addComponent = Undo.AddComponent(component.gameObject, type);

                    if (addComponent is not IRootTransformPathBase rootTransformPathBase) continue;

                    var reference = new AvatarObjectReference();
                    var rootTransform = componentProxy.rootTransform;

                    if (rootTransform == null)
                    {
                        Utils.LogWarning(nameof(RootTransformPathMenu),
                            $"Root Transform is null, skip this component. ({component.FullName()})");
                        continue;
                    }

                    reference.Set(rootTransform.gameObject);
                    rootTransformPathBase.Reference = reference;
                }
            }
        }
    }
}