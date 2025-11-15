using System.Linq;
using io.github.ykysnk.utils.Extensions;
using nadena.dev.ndmf;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

internal class NewNamePass : MaexPass<NewNamePass>
{
    public override string QualifiedName => "io.github.ykysnk.ModularAvatarExtensions.NewName";
    public override string DisplayName => "Modular Avatar Extensions New Name Generator";

    protected override void Execute(BuildContext ctx)
    {
        var avatar = ctx.AvatarRootObject;
        var autoChangeNames = avatar.GetComponentsInChildren<NewName>(true).Where(c => c).ToArray();

        Log($"Find {autoChangeNames.Length} new name inside \"{avatar.FullName()}\"");

        foreach (var comp in autoChangeNames)
        {
            var obj = comp.gameObject;
            var newName = comp.newName;

            if (string.IsNullOrEmpty(newName))
            {
                LogError($"New name can't be null or empty \"{obj.FullName()}\"",
                    $"Check the new name of \"{obj.FullName()}\"");
                continue;
            }

            Log($"Old name: \"{obj.name}\" New name: \"{newName}\" Path: \"{obj.FullName()}\"");
            obj.name = newName;
        }
    }
}