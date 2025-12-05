using System;
using System.Linq;
using io.github.ykysnk.utils.Extensions;
using nadena.dev.modular_avatar.core;
using nadena.dev.ndmf;
using UnityEngine;
using UnityEngine.Animations;
using Object = UnityEngine.Object;
#if MAEX_VRCSDK3_BASE
using VRC.Dynamics;
#endif

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

        LogC($"Find {editorOnlyList.Length} editor only inside \"{avatar.FullName()}\"");

        foreach (var editorOnly in editorOnlyList)
            using (ErrorReport.WithContextObject(editorOnly))
                try
                {
                    var components = editorOnly.GetComponents<Component>();

                    foreach (var component in components)
                    {
                        if (component is not (ModularAvatarBoneProxy or
#if MAEX_VRCSDK3_BASE
                            VRCConstraintBase or
#endif
                            IConstraint))
                            continue;
                        Object.DestroyImmediate(component);
                    }

                    LogC($"Remove 'BoneProxy' and 'Constraint' components in \"{editorOnly.FullName()}\"");
                }
                catch (Exception e)
                {
                    ErrorReport.ReportException(e);
                    return;
                }
    }
}