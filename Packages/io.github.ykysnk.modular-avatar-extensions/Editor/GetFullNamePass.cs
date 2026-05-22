using System.Linq;
using io.github.ykysnk.ModularAvatarExtensions.Editor.PluginDefinition;
using io.github.ykysnk.utils.Extensions;
using nadena.dev.ndmf;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    internal class GetFullNamePass : MaexPass<GetFullNamePass>
    {
        protected override void Execute(BuildContext context)
        {
            var ext = context.Extension<ModularAvatarExtensionsContext>();
            var avatar = context.AvatarRootObject;

            foreach (var (component, fullName) in avatar.GetComponentsInChildren<AvatarMaexComponent>(true)
                         .Where(c => c && c.gameObject)
                         .ToDictionary(x => x, x => x.FullName()))
                ext.AddComponent(component, fullName);
        }
    }
}