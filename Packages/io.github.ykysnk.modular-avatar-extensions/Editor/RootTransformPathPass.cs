using System.Linq;
using io.github.ykysnk.utils.Extensions;
using nadena.dev.ndmf;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[RunsOnPlatforms(WellKnownPlatforms.VRChatAvatar30)]
internal class RootTransformPathPass : MaexPass<RootTransformPathPass>
{
    public override string QualifiedName => "io.github.ykysnk.ModularAvatarExtensions.RootTransformPath";
    public override string DisplayName => "Modular Avatar Extensions Root Transform Path";

    protected override void Execute(BuildContext ctx)
    {
        var avatar = ctx.AvatarRootObject;
        var baseType = typeof(RootTransformPathBase<>);
        var assembly = baseType.Assembly;
        var types = assembly.GetTypes()
            .Where(t => t.IsClass && t is { IsAbstract: false, IsInterface: false })
            .Where(t => t.BaseType is { IsGenericType: true } && t.BaseType.GetGenericTypeDefinition() == baseType)
            .ToList();

        foreach (var type in types)
        {
            var components = avatar.GetComponentsInChildren(type, true).Where(c => c).ToArray();

            Log($"Find {components.Length} {type.Name} inside \"{avatar.FullName()}\"");

            var typeDefinition = type.BaseType?.GetGenericArguments();

            if (typeDefinition == null || typeDefinition.Length < 1) continue;

            var findType = typeDefinition[0];

            foreach (var component in components)
            {
                if (component is not IRootTransformPathBase rootTransformPathBase) continue;
                var setComponent = rootTransformPathBase.Component;
                if (setComponent == null)
                    setComponent = component.GetComponent(findType);
                if (setComponent == null)
                {
                    LogNonFatal($"Can't find {findType.Name} of \"{component.FullName()}\"",
                        $"Check the {findType.Name} path of {component.FullName()}");
                    continue;
                }

                var setComponentProxy = new RootTransformProxy(setComponent);

                if (string.IsNullOrEmpty(rootTransformPathBase.Reference?.referencePath))
                {
                    if (!rootTransformPathBase.IsValid())
                        LogNonFatal(
                            $"Root transform reference path of \"{component.FullName()}\" is invalid, will be skip in the build.",
                            $"Check the root transform path of {component.FullName()}");
                    continue;
                }

                var rootTransform = ctx.AvatarRootTransform.Find(rootTransformPathBase.Reference?.referencePath);
                if (rootTransform == null)
                {
                    LogError($"Can't find anything using path \"{rootTransformPathBase.Reference?.referencePath}\"",
                        $"Check the root transform path of {component.FullName()}");
                    continue;
                }

                if (setComponentProxy.rootTransform == rootTransform)
                    continue;

                setComponentProxy.rootTransform = rootTransform;
            }
        }
    }
}