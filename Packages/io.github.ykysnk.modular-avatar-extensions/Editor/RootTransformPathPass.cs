using System;
using System.Linq;
using io.github.ykysnk.utils.Extensions;
using nadena.dev.ndmf;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
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

                LogC($"Find {components.Length} {type.Name} inside \"{avatar.FullName()}\"");

                var typeDefinition = type.BaseType?.GetGenericArguments();

                if (typeDefinition == null || typeDefinition.Length < 1) continue;

                var findType = typeDefinition[0];

                foreach (var component in components)
                    using (ErrorReport.WithContextObject(component))
                        try
                        {
                            if (component is not IRootTransformPathBase rootTransformPathBase) continue;
                            var setComponent = rootTransformPathBase.Component;
                            if (setComponent == null)
                                setComponent = component.GetComponent(findType);

                            if (setComponent == null)
                            {
                                // Avatar Pose System moves all phys bone to APS_PB when building, so try to find it.
                                var apsTransform = component.transform.Find("APS_PB");
                                if (apsTransform)
                                    setComponent = apsTransform.GetComponent(findType);
                            }

                            if (setComponent == null)
                            {
                                // TODO: Remove full name
                                LogError("error.root_transform_path_pass.root_transform_not_found", findType.Name,
                                    component.FullName());
                                continue;
                            }

                            var setComponentProxy = new RootTransformProxy(setComponent);
                            var referencePath = rootTransformPathBase.Reference?.referencePath;

                            if (string.IsNullOrEmpty(referencePath))
                            {
                                // TODO: Remove full name
                                if (!rootTransformPathBase.IsValid())
                                    LogError("error.root_transform_path_pass.invalid_reference_path",
                                        component.FullName());
                                continue;
                            }

                            var rootTransform = ctx.AvatarRootTransform.Find(referencePath);
                            if (rootTransform == null)
                            {
                                // TODO: Remove full name
                                LogError("error.reference_path_not_found", referencePath, component.FullName());
                                continue;
                            }

                            if (setComponentProxy.rootTransform == rootTransform) continue;

                            setComponentProxy.rootTransform = rootTransform;
                        }
                        catch (Exception e)
                        {
                            ErrorReport.ReportException(e);
                            return;
                        }
            }
        }
    }
}