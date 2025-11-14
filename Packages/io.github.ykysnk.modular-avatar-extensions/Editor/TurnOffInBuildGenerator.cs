using System.Collections.Generic;
using System.Linq;
using AnimatorAsCode.V1;
using io.github.ykysnk.ModularAvatarExtensions.Editor;
using io.github.ykysnk.utils.Extensions;
using nadena.dev.modular_avatar.core;
using nadena.dev.ndmf;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

[assembly: ExportsPlugin(typeof(TurnOffInBuildGenerator))]

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

public class TurnOffInBuildGenerator : Plugin<TurnOffInBuildGenerator>, IMaexPlugin
{
    public override string QualifiedName => "io.github.ykysnk.ModularAvatarExtensions.TurnOffInBuild";
    public override string DisplayName => "Modular Avatar Extensions Turn Off In Build Generator";
    private const string SystemName = "Modular Avatar EX";
    private const string GenName = "Turn Off In Build";
    private int _mergeAnimatorIndex;
    private GameObject? _root;

    public void Log(object message) => Debug.Log($"[{DisplayName}] {message}");

    public void Log(string detail, string hint)
    {
        var error = new SimpleStringError($"{DisplayName} Warning", detail, hint, ErrorSeverity.Information);
        ErrorReport.ReportError(error);
    }

    public void LogError(string detail, string hint)
    {
        var error = new SimpleStringError($"{DisplayName} Failed", detail, hint, ErrorSeverity.Error);
        ErrorReport.ReportError(error);
    }

    protected override void Configure() =>
        InPhase(BuildPhase.Resolving).Run($"Generate {DisplayName}", Generate);

    private void Generate(BuildContext ctx)
    {
        var avatar = ctx.AvatarRootObject;
        var turnOffInBuilds = avatar.GetComponentsInChildren<TurnOffInBuild>(true).Where(c => c).ToArray();
        var animObjs = new List<GameObject>();

        Log($"Find {turnOffInBuilds.Length} turn off in build inside \"{avatar.FullName()}\"");

        foreach (var turnOffInBuild in turnOffInBuilds)
        {
            var obj = turnOffInBuild.gameObject;
            if (!obj.activeSelf)
            {
                Log($"Game Object \"{obj.FullName()}\" already is non active");
                continue;
            }

            obj.SetActive(false);
            animObjs.Add(obj);
            Log($"Game Object \"{obj.FullName()}\" is now non active");
            Object.DestroyImmediate(turnOffInBuild);
        }

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
    }

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
}