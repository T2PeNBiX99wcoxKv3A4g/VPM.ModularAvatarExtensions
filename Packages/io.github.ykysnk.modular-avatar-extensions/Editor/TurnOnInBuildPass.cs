using System;
using System.Linq;
using io.github.ykysnk.utils.Extensions;
using nadena.dev.ndmf;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

internal class TurnOnInBuildPass : MaexPass<TurnOnInBuildPass>
{
    public override string QualifiedName => "io.github.ykysnk.ModularAvatarExtensions.TurnOnInBuild";
    public override string DisplayName => "Modular Avatar Extensions Turn On In Build";

    protected override void Execute(BuildContext ctx)
    {
        var avatar = ctx.AvatarRootObject;
        var turnOnInBuilds = avatar.GetComponentsInChildren<ModularAvatarExtensionsTurnOnInBuild>(true).Where(c => c)
            .ToArray();

        LogC($"Find {turnOnInBuilds.Length} turn on in build inside \"{avatar.FullName()}\"");

        foreach (var turnOnInBuild in turnOnInBuilds)
            using (ErrorReport.WithContextObject(turnOnInBuild))
                try
                {
                    var obj = turnOnInBuild.gameObject;
                    if (obj.activeSelf)
                    {
                        LogC($"Game Object \"{obj.FullName()}\" already is active");
                        continue;
                    }

                    obj.SetActive(true);
                    LogC($"Game Object \"{obj.FullName()}\" is now active");
                }
                catch (Exception e)
                {
                    ErrorReport.ReportException(e);
                    return;
                }
    }
}