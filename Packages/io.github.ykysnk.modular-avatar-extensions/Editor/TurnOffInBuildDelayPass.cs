using System;
using System.Linq;
using io.github.ykysnk.utils.Extensions;
using nadena.dev.ndmf;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    internal class TurnOffInBuildDelayPass : MaexPass<TurnOffInBuildDelayPass>
    {
        public override string QualifiedName => "io.github.ykysnk.ModularAvatarExtensions.TurnOffInBuildDelay";
        public override string DisplayName => "Modular Avatar Extensions Turn Off In Build Delay";

        protected override void Execute(BuildContext ctx)
        {
            var avatar = ctx.AvatarRootObject;
            var turnOffInBuilds = avatar.GetComponentsInChildren<ModularAvatarExtensionsTurnOffInBuild>(true)
                .Where(c => c && c.IsDelay).ToArray();

            LogC($"Find {turnOffInBuilds.Length} turn off in build delay inside \"{avatar.FullName()}\"");

            foreach (var turnOffInBuild in turnOffInBuilds)
                using (ErrorReport.WithContextObject(turnOffInBuild))
                    try
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
                    catch (Exception e)
                    {
                        ErrorReport.ReportException(e);
                        throw;
                    }
        }
    }
}