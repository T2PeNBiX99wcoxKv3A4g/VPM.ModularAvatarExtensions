using System.Linq;
using io.github.ykysnk.utils.Extensions;
using nadena.dev.ndmf;
using VRC.SDKBase;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

internal class ViewPositionPass : MaexPass<ViewPositionPass>
{
    public override string QualifiedName => "io.github.ykysnk.ModularAvatarExtensions.ViewPosition";
    public override string DisplayName => "Modular Avatar Extensions View Position";

    protected override void Execute(BuildContext ctx)
    {
        var avatar = ctx.AvatarRootObject;
        var viewPositions = avatar.GetComponentsInChildren<ModularAvatarExtensionsViewPosition>(true).Where(c => c)
            .ToArray();

        LogC($"Find {viewPositions.Length} view positions inside \"{avatar.FullName()}\"");

        if (viewPositions.Length < 1) return;
        if (!ctx.AvatarRootObject.TryGetComponent<VRC_AvatarDescriptor>(out var avatarDescriptor))
        {
            LogError("error.view_position_pass.avatar_descriptor_not_found");
            return;
        }

        var getViewPosition = avatarDescriptor.ViewPosition;

        foreach (var viewPosition in viewPositions)
        {
            viewPosition.transform.position = getViewPosition;
            LogC($"Set position of {viewPosition.FullName()} to {getViewPosition}");
        }
    }
}