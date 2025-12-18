#if MAEX_VRCSDK3_BASE
using System;
using System.Collections.Generic;
using System.Linq;
using AnimatorAsCode.V1;
using io.github.ykysnk.utils.Extensions;
using io.github.ykysnk.utils.NonUdon;
using nadena.dev.modular_avatar.core;
using nadena.dev.ndmf;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    internal class ParamOnlyObjectPass : MaexPass<ParamOnlyObjectPass>
    {
        public override string QualifiedName => "io.github.ykysnk.ModularAvatarExtensions.ParamOnlyObject";
        public override string DisplayName => "Modular Avatar Extensions Param Only Object";

        // TODO: Search anim clip then change them, Preview
        protected override void Execute(BuildContext ctx)
        {
            var avatar = ctx.AvatarRootObject;
            var paramOnlyObjectBases = avatar.GetComponentsInChildren<ModularAvatarExtensionsParamOnlyObjectBase>(true)
                .Where(c => c).Select(x => x.gameObject).Distinct().ToDictionary(x => x,
                    x => x.GetComponents<ModularAvatarExtensionsParamOnlyObjectBase>());

            LogC($"Find {paramOnlyObjectBases.Count} is param only object inside \"{avatar.FullName()}\"");

            if (paramOnlyObjectBases.Count < 1) return;

            var gameObjectWithParamData = new Dictionary<GameObject, ParamPassData>();
            var paramDataWithGameObject = new Dictionary<ParamPassData, List<GameObject>>();

            foreach (var (gameObject, components) in paramOnlyObjectBases)
                using (ErrorReport.WithContextObject(gameObject))
                    try
                    {
                        var allParams = components.SelectMany(x => x.ParamDatas).GroupBy(x => x.paramName)
                            .Select(x => x.First())
                            .ToArray();
                        var reverse = components.Any(x => x is ModularAvatarExtensionsParamOnlyObject)
                            ? components.Any(x => x is ModularAvatarExtensionsParamOnlyObject { reverse: true })
                            : components.First().reverse;
                        var highPriority = components.Any(x => x is ModularAvatarExtensionsParamOnlyObject)
                            ? components.Any(x => x is ModularAvatarExtensionsParamOnlyObject { highPriority: true })
                            : components.First().highPriority;
                        var passData = new ParamPassData(allParams, reverse, highPriority);
                        gameObjectWithParamData.TryAdd(gameObject, passData);
                        paramDataWithGameObject.TryAdd(passData, new());
                        paramDataWithGameObject[passData].Add(gameObject);
                    }
                    catch (Exception e)
                    {
                        ErrorReport.ReportException(e);
                        return;
                    }

            foreach (var (gameObject, data) in gameObjectWithParamData)
                using (ErrorReport.WithContextObject(gameObject))
                    try
                    {
                        gameObject.SetActive(data.Reverse);
                    }
                    catch (Exception e)
                    {
                        ErrorReport.ReportException(e);
                        return;
                    }

            var aac = AacV1.Create(new()
            {
                SystemName = Util.SystemName,
                AnimatorRoot = ctx.AvatarRootTransform,
                DefaultValueRoot = ctx.AvatarRootTransform,
                AssetKey = GUID.Generate().ToString(),
                AssetContainer = ctx.AssetContainer,
                ContainerMode = AacConfiguration.Container.OnlyWhenPersistenceRequired,
                DefaultsProvider = new AacDefaultsProvider(true)
            });

            var heightPriorityParamData = paramDataWithGameObject.Where(x => x.Key.HighPriority).ToArray();
            var lowPriorityParamData = paramDataWithGameObject.Where(x => !x.Key.HighPriority).ToArray();
            var emptyClip = aac.NewClip();

            var heightPriorityController = aac.NewAnimatorController();

            foreach (var (data, gameObjects) in heightPriorityParamData)
            {
                var layer = heightPriorityController.NewLayer(
                    $"ParamOnlyObjectHighPriority/{HashUtils.ComputeHash(string.Join("|", data.ParamDatas.Select(x => x.paramName)), HashUtils.HashType.MD5)}");
                var offClip = aac.NewClip().Toggling(gameObjects.ToArray(), data.Reverse);
                var onClip = aac.NewClip().Toggling(gameObjects.ToArray(), !data.Reverse);

                var idleState = layer.NewState("Idle").WithAnimation(emptyClip);
                var isOffState = layer.NewState("Off").WithAnimation(offClip);
                var isOnState = layer.NewState("On").WithAnimation(onClip);
                var idleToOn = idleState.TransitionsTo(isOnState);

                foreach (var paramData in data.ParamDatas)
                {
                    var floatParam = layer.FloatParameter(paramData.paramName);
                    idleState.TransitionsTo(isOffState).When(floatParam.IsLessThan(0.01f));
                    idleToOn.When(floatParam.IsGreaterThan(paramData.GetFloatValue() - 0.01f));
                    idleToOn.When(floatParam.IsLessThan(paramData.GetFloatValue() + 0.01f));
                }
            }

            var lowPriorityController = aac.NewAnimatorController();

            foreach (var (data, gameObjects) in lowPriorityParamData)
            {
                var layer = lowPriorityController.NewLayer(
                    $"ParamOnlyObjectLowPriority/{HashUtils.ComputeHash(string.Join("|", data.ParamDatas.Select(x => x.paramName)), HashUtils.HashType.MD5)}");
                var offClip = aac.NewClip().Toggling(gameObjects.ToArray(), data.Reverse);
                var onClip = aac.NewClip().Toggling(gameObjects.ToArray(), !data.Reverse);

                var idleState = layer.NewState("Idle").WithAnimation(emptyClip);
                var isOffState = layer.NewState("Off").WithAnimation(offClip);
                var isOnState = layer.NewState("On").WithAnimation(onClip);
                var idleToOn = idleState.TransitionsTo(isOnState);
                var offToOn = isOffState.TransitionsTo(isOnState);

                foreach (var paramData in data.ParamDatas)
                {
                    var floatParam = layer.FloatParameter(paramData.paramName);
                    idleState.TransitionsTo(isOffState).When(floatParam.IsLessThan(0.01f));
                    isOnState.TransitionsTo(isOffState).When(floatParam.IsLessThan(paramData.GetFloatValue()));
                    idleToOn.When(floatParam.IsGreaterThan(paramData.GetFloatValue() - 0.01f));
                    idleToOn.When(floatParam.IsLessThan(paramData.GetFloatValue() + 0.01f));
                    offToOn.When(floatParam.IsGreaterThan(paramData.GetFloatValue() - 0.01f));
                    offToOn.When(floatParam.IsLessThan(paramData.GetFloatValue() + 0.01f));
                }
            }

            var obj = new GameObject(DisplayName)
            {
                transform =
                {
                    parent = ctx.AvatarRootTransform
                }
            };

            if (heightPriorityParamData.Length > 0)
            {
                var mergeAnimator = obj.AddComponent<ModularAvatarMergeAnimator>();
                mergeAnimator.animator = heightPriorityController.AnimatorController;
                mergeAnimator.layerType = VRCAvatarDescriptor.AnimLayerType.FX;
                mergeAnimator.pathMode = MergeAnimatorPathMode.Absolute;
                mergeAnimator.layerPriority = 999999;
                mergeAnimator.deleteAttachedAnimator = true;
                mergeAnimator.matchAvatarWriteDefaults = true;
            }

            // ReSharper disable once InvertIf
            if (lowPriorityParamData.Length > 0)
            {
                var mergeAnimator = obj.AddComponent<ModularAvatarMergeAnimator>();
                mergeAnimator.animator = lowPriorityController.AnimatorController;
                mergeAnimator.layerType = VRCAvatarDescriptor.AnimLayerType.FX;
                mergeAnimator.pathMode = MergeAnimatorPathMode.Absolute;
                mergeAnimator.layerPriority = -999999;
                mergeAnimator.deleteAttachedAnimator = true;
                mergeAnimator.matchAvatarWriteDefaults = true;
            }
        }
    }
}
#endif