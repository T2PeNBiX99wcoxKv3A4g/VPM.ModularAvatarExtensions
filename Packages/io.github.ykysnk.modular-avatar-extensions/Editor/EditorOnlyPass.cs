using System.Linq;
using io.github.ykysnk.utils.Extensions;
using nadena.dev.modular_avatar.core;
using nadena.dev.ndmf;
using UnityEngine;
using UnityEngine.Animations;
using VRC.Dynamics;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

internal class EditorOnlyPass : MaexPass<EditorOnlyPass>
{
    public override string QualifiedName => "io.github.ykysnk.ModularAvatarExtensions.EditorOnly";
    public override string DisplayName => "Modular Avatar Extensions Editor Only";

    protected override void Execute(BuildContext ctx)
    {
        var avatar = ctx.AvatarRootObject;
        var editorOnlyList =
            avatar.GetComponentsInChildren<ModularAvatarExtensionsEditorOnly>(true).Where(c => c).ToArray();

        Log($"Find {editorOnlyList.Length} editor only inside \"{avatar.FullName()}\"");

        foreach (var editorOnly in editorOnlyList)
        {
            var components = editorOnly.GetComponents<Component>();

            foreach (var component in components)
            {
                if (component is not (ModularAvatarBoneProxy or VRCConstraintBase or IConstraint))
                    continue;
                Object.DestroyImmediate(component);
            }

            Log($"Remove 'BoneProxy' and 'Constraint' components in \"{editorOnly.FullName()}\"");
        }
    }
}