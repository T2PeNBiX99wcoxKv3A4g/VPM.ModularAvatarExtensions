using io.github.ykysnk.ModularAvatarExtensions.Editor;
using nadena.dev.ndmf;
using UnityEngine;

[assembly: ExportsPlugin(typeof(PluginDefinition))]

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[RunsOnAllPlatforms]
internal class PluginDefinition : Plugin<PluginDefinition>
{
    public override string QualifiedName => "io.github.ykysnk.ModularAvatarExtensions";
    public override string DisplayName => "Modular Avatar Extensions";
    public override Color? ThemeColor => new Color(0x00 / 255f, 0xa0 / 255f, 0xe9 / 255f, 1);

    protected override void Configure()
    {
        var seq = InPhase(BuildPhase.Generating);
        seq.Run(ConstraintDisablerPass.Instance);
        seq.Run(MoveToRootPass.Instance);
        seq.Run(NewNamePass.Instance);
        seq.Run(RootTransformPathPass.Instance);
        seq.Run(TurnOffInBuildPass.Instance);

#if TEMP_DISABLE
        seq = InPhase(BuildPhase.Transforming);

        seq.Run("Purge ModularAvatar EX components", ctx =>
        {
            foreach (var component in ctx.AvatarRootTransform.GetComponentsInChildren<AvatarMaexComponent>(true))
                Object.DestroyImmediate(component);
        });
#endif
    }
}