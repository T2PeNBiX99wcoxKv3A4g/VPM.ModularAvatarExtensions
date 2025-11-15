#if MODULAR_AVATAR_EX_DISABLE
using System.Collections.Generic;
using AnimatorAsCode.V1;
using nadena.dev.modular_avatar.core;
using UnityEditor;
using VRC.SDK3.Avatars.Components;
#endif

using System.Linq;
using io.github.ykysnk.utils.Extensions;
using nadena.dev.ndmf;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

internal class TurnOffInBuildPass : MaexPass<TurnOffInBuildPass>
{
    public override string QualifiedName => "io.github.ykysnk.ModularAvatarExtensions.TurnOffInBuild";
    public override string DisplayName => "Modular Avatar Extensions Turn Off In Build Generator";

#if MODULAR_AVATAR_EX_DISABLE
    private const string SystemName = "Modular Avatar EX";
    private const string GenName = "Turn Off In Build";

    private int _mergeAnimatorIndex;
    private GameObject? _root;
#endif

    protected override void Execute(BuildContext ctx)
    {
        var avatar = ctx.AvatarRootObject;
        var turnOffInBuilds = avatar.GetComponentsInChildren<TurnOffInBuild>(true).Where(c => c).ToArray();
#if MODULAR_AVATAR_EX_DISABLE
        var animObjs = new List<GameObject>();
#endif

        Log($"Find {turnOffInBuilds.Length} turn off in build inside \"{avatar.FullName()}\"");

        foreach (var turnOffInBuild in turnOffInBuilds)
        {
            var obj = turnOffInBuild.gameObject;
            if (!obj.activeSelf)
            {
                Log($"Game Object \"{obj.FullName()}\" already is inactive");
                continue;
            }

            obj.SetActive(false);
#if MODULAR_AVATAR_EX_DISABLE
            animObjs.Add(obj);
#endif
            Log($"Game Object \"{obj.FullName()}\" is now inactive");
        }
#if MODULAR_AVATAR_EX_DISABLE
        var aac = AacV1.Create(new()
        {
            SystemName = SystemName,
            AnimatorRoot = ctx.AvatarRootTransform,
            DefaultValueRoot = ctx.AvatarRootTransform,
            AssetKey = GUID.Generate().ToString(),
            AssetContainer = ctx.AssetContainer,
            ContainerMode = AacConfiguration.Container.OnlyWhenPersistenceRequired,
            DefaultsProvider = new AacDefaultsProvider(true),
        });

        var ctrl = aac.NewAnimatorController();
        var layer = ctrl.NewLayer(GenName);

        var clip = aac.NewClip();
        clip.Toggling(animObjs.ToArray(), false);

        layer.NewState("Turn Off").WithAnimation(clip);

        var newObj = new GameObject($"{SystemName}: {GenName}")
        {
            transform =
            {
                parent = ctx.AvatarRootTransform
            }
        };
        _root = newObj;
        var mergeAnimator = NewMergeAnimator(ctrl, VRCAvatarDescriptor.AnimLayerType.FX);
        if (!mergeAnimator) return;
        mergeAnimator!.layerPriority = -999999;
        mergeAnimator.deleteAttachedAnimator = true;
        mergeAnimator.matchAvatarWriteDefaults = true;
#endif
    }

#if MODULAR_AVATAR_EX_DISABLE
    // Copy from MaAc
    private ModularAvatarMergeAnimator? NewMergeAnimator(AacFlController controller,
        VRCAvatarDescriptor.AnimLayerType layerType)
    {
        if (!_root) return null;

        var components = _root!.GetComponents<ModularAvatarMergeAnimator>();
        var mergeAnimator = _mergeAnimatorIndex >= components.Length
            ? Undo.AddComponent<ModularAvatarMergeAnimator>(_root)
            : components[_mergeAnimatorIndex];

        _mergeAnimatorIndex++;

        mergeAnimator.animator = controller.AnimatorController;
        mergeAnimator.layerType = layerType;
        mergeAnimator.pathMode = MergeAnimatorPathMode.Absolute;
        return mergeAnimator;
    }
#endif
}