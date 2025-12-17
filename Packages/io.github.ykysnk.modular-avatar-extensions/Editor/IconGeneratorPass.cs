using System;
using System.Linq;
using nadena.dev.modular_avatar.core;
using nadena.dev.ndmf;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    internal class IconGeneratorPass : MaexPass<IconGeneratorPass>
    {
        public override string QualifiedName => "io.github.ykysnk.ModularAvatarExtensions.IconGenerator";
        public override string DisplayName => "Modular Avatar Extensions Icon Generator";

        protected override void Execute(BuildContext ctx)
        {
            var avatar = ctx.AvatarRootObject;
            var iconGeneratorBases = avatar.GetComponentsInChildren<ModularAvatarExtensionsIconGeneratorBase>(true)
                .Where(c => c)
                .ToArray();

            foreach (var iconGeneratorBase in iconGeneratorBases)
                using (ErrorReport.WithContextObject(iconGeneratorBase))
                    try
                    {
                        var menuItem = iconGeneratorBase.GetComponent<ModularAvatarMenuItem>();
                        if (menuItem == null) continue;
                        menuItem.PortableControl.Icon = iconGeneratorBase.IconTexture;
                    }
                    catch (Exception e)
                    {
                        ErrorReport.ReportException(e);
                        return;
                    }

            var iconGeneratorOfReferences = avatar
                .GetComponentsInChildren<ModularAvatarExtensionsIconGeneratorOfReference>(true).Where(c => c).ToArray();

            foreach (var iconGeneratorOfReference in iconGeneratorOfReferences)
                using (ErrorReport.WithContextObject(iconGeneratorOfReference))
                    try
                    {
                        var menuItem = iconGeneratorOfReference.GetComponent<ModularAvatarMenuItem>();
                        if (menuItem == null) continue;
                        menuItem.PortableControl.Icon = iconGeneratorOfReference.IconTexture;
                    }
                    catch (Exception e)
                    {
                        ErrorReport.ReportException(e);
                        return;
                    }
        }
    }
}