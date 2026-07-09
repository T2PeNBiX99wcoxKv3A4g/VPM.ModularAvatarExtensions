#if MAEX_D4RK_AVATAR_OPTIMIZER
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using d4rkpl4y3r.AvatarOptimizer.Extensions;
using HarmonyLib;
using io.github.ykysnk.utils.Editor.Patches;
using JetBrains.Annotations;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor.Patches
{
    public class D4RkAvatarOptimizerPatches : Patch<D4RkAvatarOptimizerPatches>
    {
        private const string CacheGetAllExcludedTransformsName = "cache_GetAllExcludedTransforms";

        private static readonly MethodInfo GetAllExcludedTransformsMethod =
            AccessTools.Method(typeof(d4rkAvatarOptimizer), nameof(d4rkAvatarOptimizer.GetAllExcludedTransforms));

        protected override void Execute(Harmony harmony)
        {
        }

        private class OrigGetAllExcludedTransforms : ReversePatchMethod<OrigGetAllExcludedTransforms>
        {
            public override MethodInfo TargetMethod => GetAllExcludedTransformsMethod;
            public override string ReverseMethod => nameof(Reverse);

            [SuppressMessage("ReSharper", "UnusedParameter.Local")]
            internal static HashSet<Transform> Reverse(d4rkAvatarOptimizer instance) =>
                throw new NotImplementedException($"{nameof(OrigGetAllExcludedTransforms)} is fucked");
        }

        [UsedImplicitly]
        private class GetAllExcludedTransforms : PatchMethod<GetAllExcludedTransforms>
        {
            public override MethodInfo TargetMethod => GetAllExcludedTransformsMethod;
            public override string PrefixMethod => nameof(Prefix);

            private static bool Prefix(d4rkAvatarOptimizer __instance,
                [SuppressMessage("ReSharper", "RedundantAssignment")]
                ref HashSet<Transform> __result)
            {
                var cacheGetAllExcludedTransforms = Traverse.Create(__instance)
                    .Field<HashSet<Transform>>(CacheGetAllExcludedTransformsName).Value;
                if (cacheGetAllExcludedTransforms != null)
                {
                    __result = cacheGetAllExcludedTransforms;
                    return false;
                }

                var origList = OrigGetAllExcludedTransforms.Reverse(__instance);
                var exExclusions = new List<Transform>();
                exExclusions.AddRange(__instance.transform
                    .GetComponentsInChildren<ModularAvatarExtensionsD4RkAvatarOptimizerExclude>(true)
                    .Select(c => c.transform));
                foreach (var excludedTransform in exExclusions.Where(excludedTransform => excludedTransform != null))
                {
                    origList.Add(excludedTransform);
                    origList.UnionWith(excludedTransform.GetAllDescendants());
                }

                Log2($"Cached {origList.Count} transforms");
                __result = origList;
                return false;
            }
        }
    }
}
#endif