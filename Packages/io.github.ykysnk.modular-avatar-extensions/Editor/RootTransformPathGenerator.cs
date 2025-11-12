using System.Linq;
using io.github.ykysnk.ModularAvatarExtensions.Editor;
using io.github.ykysnk.utils.Extensions;
using nadena.dev.ndmf;
using UnityEngine;

[assembly: ExportsPlugin(typeof(RootTransformPathGenerator))]

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

public class RootTransformPathGenerator : Plugin<RootTransformPathGenerator>, IMaexPlugin
{
    public override string QualifiedName => "io.github.ykysnk.ModularAvatarExtensions.RootTransformPath";
    public override string DisplayName => "Modular Avatar Extensions Root Transform Path Generator";

    public void Log(object message) => Debug.Log($"[{DisplayName}] {message}");

    public void Log(string detail, string hint)
    {
        var error = new SimpleStringError($"{DisplayName} Warning", detail, hint, ErrorSeverity.Information);
        ErrorReport.ReportError(error);
    }

    public void LogError(string detail, string hint)
    {
        var error = new SimpleStringError($"{DisplayName} Failed", detail, hint, ErrorSeverity.Error);
        ErrorReport.ReportError(error);
    }

    protected override void Configure() =>
        InPhase(BuildPhase.Resolving).Run($"Generate {DisplayName}", Generate);

    private void Generate(BuildContext ctx)
    {
        var avatar = ctx.AvatarRootObject;
        var baseType = typeof(RootTransformPathBase<>);
        var assembly = baseType.Assembly;
        var types = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && !t.IsInterface)
            .Where(t => t.BaseType is { IsGenericType: true } && t.BaseType.GetGenericTypeDefinition() == baseType)
            .ToList();

        foreach (var type in types)
        {
            var components = avatar.GetComponentsInChildren(type, true);

            Log($"Find {components.Length} {type.Name} inside \"{avatar.FullName()}\"");

            var typeDefinition = type.BaseType?.GetGenericArguments();

            if (typeDefinition == null || typeDefinition.Length < 1) continue;

            var findType = typeDefinition[0];

            foreach (var component in components)
            {
                if (component is not IRootTransformPathBase rootTransformPathBase) continue;
                var setComponent = rootTransformPathBase.Component;
                if (!setComponent)
                    setComponent = component.GetComponent(findType);
                if (!setComponent)
                {
                    LogError($"Can't find {findType.Name} of \"{component.FullName()}\"",
                        $"Check the {findType.Name} path of {component.FullName()}");
                    continue;
                }

                var setComponentProxy = new RootTransformProxy(setComponent);

                if (string.IsNullOrEmpty(rootTransformPathBase.Reference.referencePath))
                {
                    Log(
                        $"Root transform reference path of \"{component.FullName()}\" is null, will be skip in the build.",
                        $"Check the root transform path of {component.FullName()}");
                    continue;
                }

                var rootTransform = ctx.AvatarRootTransform.Find(rootTransformPathBase.Reference.referencePath);
                if (!rootTransform)
                {
                    LogError($"Can't find anything using path \"{rootTransformPathBase.Reference.referencePath}\"",
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