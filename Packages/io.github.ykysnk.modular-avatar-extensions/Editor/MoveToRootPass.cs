using System.Linq;
using io.github.ykysnk.utils.Extensions;
using nadena.dev.ndmf;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

internal class MoveToRootPass : MaexPass<MoveToRootPass>
{
    public override string QualifiedName => "io.github.ykysnk.ModularAvatarExtensions.MoveToRoot";
    public override string DisplayName => "Modular Avatar Extensions Move To Root";

    protected override void Execute(BuildContext ctx)
    {
        var avatar = ctx.AvatarRootObject;
        var autoMoveToRoots = avatar.GetComponentsInChildren<ModularAvatarExtensionsMoveToRoot>(true).Where(c => c)
            .ToArray();

        LogC($"Find {autoMoveToRoots.Length} move to root inside \"{avatar.FullName()}\"");

        foreach (var moveToRoot in autoMoveToRoots)
        {
            var obj = moveToRoot.gameObject;
            if (obj.transform.parent == ctx.AvatarRootTransform)
            {
                LogC($"Already in root \"{obj.FullName()}\"");
                continue;
            }

            obj.transform.SetParent(ctx.AvatarRootTransform);
            LogC($"New Path: \"{obj.FullName()}\"");
        }

        var autoMoveToRootOfTransforms =
            avatar.GetComponentsInChildren<ModularAvatarExtensionsMoveToRootOfReference>(true).Where(c => c).ToArray();

        LogC($"Find {autoMoveToRootOfTransforms.Length} move to root inside \"{avatar.FullName()}\"");

        foreach (var moveToRootOfTransform in autoMoveToRootOfTransforms)
        {
            var referencePath = moveToRootOfTransform?.reference?.referencePath;

            if (string.IsNullOrEmpty(referencePath))
            {
                LogError("error.move_to_root_of_reference_pass.invalid_reference_path",
                    moveToRootOfTransform?.FullName());
                continue;
            }

            var found = ctx.AvatarRootTransform.Find(referencePath);

            if (found == null)
            {
                LogError("error.reference_path_not_found", referencePath, moveToRootOfTransform?.FullName());
                continue;
            }

            found.transform.SetParent(ctx.AvatarRootTransform);
            LogC($"New Path: \"{found.FullName()}\"");
        }
    }
}