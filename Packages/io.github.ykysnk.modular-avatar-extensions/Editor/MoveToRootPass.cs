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
        var autoMoveToRoots = avatar.GetComponentsInChildren<MoveToRoot>(true).Where(c => c).ToArray();

        Log($"Find {autoMoveToRoots.Length} move to root inside \"{avatar.FullName()}\"");

        foreach (var moveToRoot in autoMoveToRoots)
        {
            var obj = moveToRoot.gameObject;
            if (obj.transform.parent == ctx.AvatarRootTransform)
            {
                Log($"Already in root \"{obj.FullName()}\"");
                continue;
            }

            obj.transform.SetParent(ctx.AvatarRootTransform);
            Log($"New Path: \"{obj.FullName()}\"");
        }

        var autoMoveToRootOfTransforms =
            avatar.GetComponentsInChildren<MoveToRootOfReference>(true).Where(c => c).ToArray();

        Log($"Find {autoMoveToRootOfTransforms.Length} move to root inside \"{avatar.FullName()}\"");

        foreach (var moveToRootOfTransform in autoMoveToRootOfTransforms)
        {
            var referencePath = moveToRootOfTransform?.reference?.referencePath;

            if (string.IsNullOrEmpty(referencePath))
            {
                LogError($"Reference path of \"{moveToRootOfTransform?.FullName()}\" is invalid.",
                    $"Check the root transform path of {moveToRootOfTransform?.FullName()}");
                continue;
            }

            var found = ctx.AvatarRootTransform.Find(referencePath);

            if (found == null)
            {
                LogError($"Can't find anything using path \"{referencePath}\"",
                    $"Check the root transform path of {moveToRootOfTransform?.FullName()}");
                continue;
            }

            found.transform.SetParent(ctx.AvatarRootTransform);
            Log($"New Path: \"{found.FullName()}\"");
        }
    }
}