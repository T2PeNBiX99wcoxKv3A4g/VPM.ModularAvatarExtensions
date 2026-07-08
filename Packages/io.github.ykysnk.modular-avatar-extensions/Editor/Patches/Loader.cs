using io.github.ykysnk.ModularAvatarExtensions.Editor.Patches;
using io.github.ykysnk.utils.Editor.Patches;

[assembly: ExportsPatchLoader(typeof(Loader))]

namespace io.github.ykysnk.ModularAvatarExtensions.Editor.Patches
{
    public class Loader : PatchLoader<Loader>
    {
        public override string QualifiedName => "io.github.ykysnk.modular-avatar-extensions.patches";
        public override string DisplayName => "Modular Avatar Extensions Patches";

        public override void Load()
        {
#if MAEX_D4RK_AVATAR_OPTIMIZER
            Run(D4RkAvatarOptimizerPatches.Instance);
#endif
        }
    }
}