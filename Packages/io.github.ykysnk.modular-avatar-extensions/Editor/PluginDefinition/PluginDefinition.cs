using io.github.ykysnk.ModularAvatarExtensions.Editor.Animation;
using io.github.ykysnk.ModularAvatarExtensions.Editor.PluginDefinition;
using nadena.dev.ndmf;
using nadena.dev.ndmf.animator;
using UnityEngine;

[assembly: ExportsPlugin(typeof(PluginDefinition))]

namespace io.github.ykysnk.ModularAvatarExtensions.Editor.PluginDefinition
{
    [RunsOnAllPlatforms]
    internal class PluginDefinition : Plugin<PluginDefinition>
    {
        private const string ModularAvatarQualifiedName = "nadena.dev.modular-avatar";
        public override string QualifiedName => "io.github.ykysnk.ModularAvatarExtensions";
        public override string DisplayName => "Modular Avatar Extensions";
        public override Color? ThemeColor => new Color(0x00 / 255f, 0xa0 / 255f, 0xe9 / 255f, 1);

        protected override void Configure()
        {
            var seq = InPhase(BuildPhase.Resolving);
            // TODO: Maybe Validate
            seq.WithRequiredExtension(typeof(ModularAvatarExtensionsContext), s =>
            {
                s.Run(GetFullNamePass.Instance);
                s.Run(NewNamePass.Instance);
                s.Run(MoveToRootPass.Instance);
            });

            seq = InPhase(BuildPhase.Generating);

            seq.WithRequiredExtension(typeof(ModularAvatarExtensionsContext), s =>
            {
                s.Run(ConstraintDisablerPass.Instance);
                s.Run(RootTransformPathPass.Instance);
                s.Run(TurnOffInBuildPass.Instance);
                s.Run(TurnOnInBuildPass.Instance);
                s.Run(EditorOnlyPass.Instance);
                s.Run(ChangeMaterialInBuildPass.Instance);
#if MAEX_VRCSDK3_BASE
                s.Run(ViewPositionPass.Instance);
#endif
                s.Run(WorldScalePass.Instance);
                s.Run(IconGeneratorPass.Instance);
            });

#if MAEX_VRCSDK3_BASE
            seq = InPhase(BuildPhase.Transforming);
            seq.BeforePlugin(ModularAvatarQualifiedName);
            seq.WithRequiredExtension(typeof(ModularAvatarExtensionsContext),
                s =>
                {
                    s.WithRequiredExtension(typeof(AnimatorServicesContext),
                        s2 =>
                        {
                            s2.WithRequiredExtension(typeof(ParamOnlyObjectExtension),
                                s3 => { s3.Run(ParamOnlyObjectPass.Instance); });
                        });
                });

            seq = InPhase(BuildPhase.Transforming);
            seq.AfterPlugin(ModularAvatarQualifiedName);
            seq.WithRequiredExtension(typeof(ModularAvatarExtensionsContext),
                s => { s.Run(ParamOnlyObjectDelayDisablePass.Instance); });
#endif

#if MAEX_VRCSDK3_BASE
            seq = InPhase(BuildPhase.Optimizing);
            seq.AfterPlugin(ModularAvatarQualifiedName);
            seq.WithRequiredExtension(typeof(ModularAvatarExtensionsContext),
                s =>
                {
                    s.WithRequiredExtension(typeof(AnimatorServicesContext), s2 => { s2.Run(MmdLayerFixPass.Instance); });
                });
#endif

            seq = InPhase(BuildPhase.PlatformFinish);
            seq.WithRequiredExtension(typeof(ModularAvatarExtensionsContext), s =>
                s.Run("Purge ModularAvatar EX components", ctx =>
                {
                    foreach (var component in ctx.AvatarRootTransform.GetComponentsInChildren<AvatarMaexComponent>(true))
                    {
                        if (component.DontDestroyOnBuild) continue;
                        Object.DestroyImmediate(component);
                    }
                }));
        }
    }
}