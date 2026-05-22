using System;
using nadena.dev.ndmf;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor.Animation
{
    internal class ParamOnlyObjectDelayDisablePass : MaexPass<ParamOnlyObjectDelayDisablePass>
    {
        public override string QualifiedName => "io.github.ykysnk.ModularAvatarExtensions.ParamOnlyObjectDelayDisable";
        public override string DisplayName => "Modular Avatar Extensions Param-Only Object Delay Disable";

        protected override void Execute(BuildContext context)
        {
            var objectSaver = context.GetState<ParamOnlyObjectExtension.Retained>().GameObjectProps;
            if (objectSaver.Count < 1) return;

            foreach (var (data, active) in objectSaver)
                using (ErrorReport.WithContextObject(data.GameObject))
                    try
                    {
                        if (data.GameObject == null)
                        {
                            LogSimple($"Game Object ({data.Path}) is gone", severity: ErrorSeverity.NonFatal);
                            // TODO: Localization
                            // LogNonFatal();
                            continue;
                        }

                        data.GameObject.SetActive(active);
                    }
                    catch (Exception e)
                    {
                        ErrorReport.ReportException(e);
                        throw;
                    }
        }
    }
}