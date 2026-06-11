using System;
using System.Linq;
using io.github.ykysnk.utils.Extensions;
using nadena.dev.ndmf;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    internal class TurnOffInBuildEarlyPass : MaexPass<TurnOffInBuildEarlyPass>
    {
        public override string QualifiedName => "io.github.ykysnk.ModularAvatarExtensions.TurnOffInBuildEarly";
        public override string DisplayName => "Modular Avatar Extensions Turn Off In Build Early";

        protected override void Execute(BuildContext ctx)
        {
            var avatar = ctx.AvatarRootObject;
            var turnOffInBuilds = avatar.GetComponentsInChildren<ModularAvatarExtensionsTurnOffInBuild>(true)
                .Where(c => c && c.IsEarly).ToArray();

            LogC($"Find {turnOffInBuilds.Length} turn off in build inside \"{avatar.FullName()}\"");

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