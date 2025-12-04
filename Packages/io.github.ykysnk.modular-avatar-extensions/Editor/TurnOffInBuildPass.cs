using System.Linq;
using io.github.ykysnk.utils.Extensions;
using nadena.dev.ndmf;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

internal class TurnOffInBuildPass : MaexPass<TurnOffInBuildPass>
{
    public override string QualifiedName => "io.github.ykysnk.ModularAvatarExtensions.TurnOffInBuild";
    public override string DisplayName => "Modular Avatar Extensions Turn Off In Build";

    protected override void Execute(BuildContext ctx)
    {
        var avatar = ctx.AvatarRootObject;
        var turnOffInBuilds = avatar.GetComponentsInChildren<ModularAvatarExtensionsTurnOffInBuild>(true).Where(c => c)
            .ToArray();

        LogC($"Find {turnOffInBuilds.Length} turn off in build inside \"{avatar.FullName()}\"");

        foreach (var turnOffInBuild in turnOffInBuilds)
        {
            var obj = turnOffInBuild.gameObject;
            if (!obj.activeSelf)
            {
                LogC($"Game Object \"{obj.FullName()}\" already is inactive");
                continue;
            }

            obj.SetActive(false);
            LogC($"Game Object \"{obj.FullName()}\" is now inactive");
        }
    }
}