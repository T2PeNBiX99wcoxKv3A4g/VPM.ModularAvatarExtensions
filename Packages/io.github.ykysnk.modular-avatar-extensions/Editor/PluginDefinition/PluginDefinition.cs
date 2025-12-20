using io.github.ykysnk.ModularAvatarExtensions.Editor.PluginDefinition;
using nadena.dev.ndmf;
using UnityEngine;

[assembly: ExportsPlugin(typeof(PluginDefinition))]

namespace io.github.ykysnk.ModularAvatarExtensions.Editor.PluginDefinition
{
    [RunsOnAllPlatforms]
    internal class PluginDefinition : Plugin<PluginDefinition>
    {
        public override string QualifiedName => "io.github.ykysnk.ModularAvatarExtensions";
        public override string DisplayName => "Modular Avatar Extensions";
        public override Color? ThemeColor => new Color(0x00 / 255f, 0xa0 / 255f, 0xe9 / 255f, 1);

        protected override void Configure()
        {
            var seq = InPhase(BuildPhase.Generating);
            // TODO: Maybe Validate
            seq.WithRequiredExtension(typeof(ModularAvatarExtensionsContext), s =>
            {
                s.Run(ConstraintDisablerPass.Instance);
                s.Run(MoveToRootPass.Instance);
                s.Run(NewNamePass.Instance);
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
#if MAEX_VRCSDK3_BASE
                s.Run(ParamOnlyObjectPass.Instance);
#endif
            });

            seq = InPhase(BuildPhase.Transforming);
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