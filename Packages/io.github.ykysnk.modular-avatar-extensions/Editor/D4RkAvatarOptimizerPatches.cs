#if MAEX_D4RK_AVATAR_OPTIMIZER
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using d4rkpl4y3r.AvatarOptimizer.Extensions;
using HarmonyLib;
using io.github.ykysnk.utils;
using UnityEditor;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

internal static class D4RkAvatarOptimizerPatches
{
    private const string PatchId = "io.github.ykysnk.modular-avatar-extensions.d4rk-avatar-optimizer-patches";
    private const string CacheGetAllExcludedTransformsName = "cache_GetAllExcludedTransforms";

    [InitializeOnLoadMethod]
    [MenuItem("Tools/Modular Avatar EX/ReInitialize D4Rk Avatar Optimizer Patches")]
    private static void Initialize()
    {
        var harmony = new Harmony(PatchId);
        var targetMethod = AccessTools.Method(typeof(d4rkAvatarOptimizer),
            nameof(d4rkAvatarOptimizer.GetAllExcludedTransforms));
        harmony.CreateReversePatcher(targetMethod,
            new(typeof(D4RkAvatarOptimizerPatches), nameof(OrigGetAllExcludedTransforms))).Patch();
        harmony.Patch(targetMethod, new(typeof(D4RkAvatarOptimizerPatches), nameof(GetAllExcludedTransforms)));
        AssemblyReloadEvents.beforeAssemblyReload += () => harmony.UnpatchAll(PatchId);
        Utils.Log(nameof(D4RkAvatarOptimizerPatches), "D4Rk Avatar Optimizer Patches Initialized");
    }

    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    private static HashSet<Transform> OrigGetAllExcludedTransforms(d4rkAvatarOptimizer instance) =>
        throw new NotImplementedException("HarmonyReversePatch is fucked");

    [SuppressMessage("ReSharper", "RedundantAssignment")]
    private static bool GetAllExcludedTransforms(d4rkAvatarOptimizer __instance, ref HashSet<Transform> __result)
    {
        var cacheGetAllExcludedTransforms = Traverse.Create(__instance)
            .Field<HashSet<Transform>>(CacheGetAllExcludedTransformsName).Value;
        if (cacheGetAllExcludedTransforms != null)
        {
            __result = cacheGetAllExcludedTransforms;
            return false;
        }

        var origList = OrigGetAllExcludedTransforms(__instance);
        var exExclusions = new List<Transform>();
        exExclusions.AddRange(__instance.transform
            .GetComponentsInChildren<ModularAvatarExtensionsD4RkAvatarOptimizerExclude>(true)
            .Select(c => c.transform));
        foreach (var excludedTransform in exExclusions.Where(excludedTransform => excludedTransform != null))
        {
            origList.Add(excludedTransform);
            origList.UnionWith(excludedTransform.GetAllDescendants());
        }

        Utils.Log(nameof(D4RkAvatarOptimizerPatches), $"Cached {origList.Count} transforms");
        __result = origList;
        return false;
    }
}
#endif