using System;
using System.Linq;
using io.github.ykysnk.utils.Extensions;
using nadena.dev.ndmf;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    internal class NewNamePass : MaexPass<NewNamePass>
    {
        public override string QualifiedName => "io.github.ykysnk.ModularAvatarExtensions.NewName";
        public override string DisplayName => "Modular Avatar Extensions New Name";

        protected override void Execute(BuildContext ctx)
        {
            var avatar = ctx.AvatarRootObject;
            var autoChangeNames =
                avatar.GetComponentsInChildren<ModularAvatarExtensionsNewName>(true).Where(c => c).ToArray();

            LogC($"Find {autoChangeNames.Length} new name inside \"{avatar.FullName()}\"");

            foreach (var comp in autoChangeNames)
                using (ErrorReport.WithContextObject(comp))
                    try
                    {
                        var obj = comp.gameObject;
                        var newName = comp.newName;

                        if (string.IsNullOrEmpty(newName))
                        {
                            // TODO: Remove full name
                            LogError("error.new_name_pass.new_name_is_empty", obj.FullName());
                            continue;
                        }

                        LogC($"Old name: \"{obj.name}\" New name: \"{newName}\" Path: \"{obj.FullName()}\"");
                        obj.name = newName;
                    }
                    catch (Exception e)
                    {
                        ErrorReport.ReportException(e);
                        throw;
                    }
        }
    }
}