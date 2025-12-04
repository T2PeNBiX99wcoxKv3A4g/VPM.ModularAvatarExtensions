#if MAEX_VRCSDK3_BASE
using VRC.Dynamics;
using VRC.SDK3.Dynamics.Constraint.Components;
#else
using UnityEngine.Animations;
#endif
using System.Linq;
using io.github.ykysnk.utils.Extensions;
using nadena.dev.ndmf;
using UnityEditor;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

internal class WorldScalePass : MaexPass<WorldScalePass>
{
    public override string QualifiedName => "io.github.ykysnk.ModularAvatarExtensions.WorldScale";
    public override string DisplayName => "Modular Avatar Extensions World Scale";

    protected override void Execute(BuildContext ctx)
    {
        var avatar = ctx.AvatarRootObject;
        var worldScales = avatar.GetComponentsInChildren<ModularAvatarExtensionsWorldScale>(true)
            .Where(c => c && !c.editorOnly)
            .ToArray();

        LogC($"Find {worldScales.Length} world scales inside \"{avatar.FullName()}\"");

        var worldPrefabPath = AssetDatabase.GUIDToAssetPath("0fb864bbf2ec27c4586c64a0c7e40cc8");

        if (string.IsNullOrEmpty(worldPrefabPath))
        {
            LogError("error.world_scale_pass.world_prefab_not_found");
            return;
        }

        var worldPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(worldPrefabPath);

        foreach (var worldScale in worldScales)
        {
            var obj = worldScale.gameObject;

#if MAEX_VRCSDK3_BASE
            var constraint = obj.AddComponent<VRCScaleConstraint>();
            var newSource = new VRCConstraintSource(worldPrefab.transform, 1f, Vector3.zero, Vector3.zero);
            constraint.Sources.Add(newSource);
            constraint.ZeroConstraint();
#else
            var constraint = obj.AddComponent<ScaleConstraint>();
            var newSource = new ConstraintSource
            {
                sourceTransform = worldPrefab.transform,
                weight = 1f
            };
            constraint.AddSource(newSource);
            constraint.locked = true;
            constraint.constraintActive = true;
#endif
            LogC($"Add world scale constraint to {obj.FullName()}");
        }
    }
}