#if MAEX_VRCSDK3_BASE
using System;
using System.Collections.Generic;
using System.Linq;
using io.github.ykysnk.utils.Extensions;
using nadena.dev.ndmf;
using nadena.dev.ndmf.animator;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor.Animation
{
    [RunsOnPlatforms(WellKnownPlatforms.VRChatAvatar30)]
    internal class ParamOnlyObjectPass : MaexPass<ParamOnlyObjectPass>
    {
        private readonly Dictionary<GameObject, ParamPassData> _gameObjectWithParamData = new();
        private readonly HashSet<string> _paramNames = new();

        public override string QualifiedName => "io.github.ykysnk.ModularAvatarExtensions.ParamOnlyObject";
        public override string DisplayName => "Modular Avatar Extensions Param-Only Object";

        protected override void Execute(BuildContext ctx)
        {
            var avatar = ctx.AvatarRootObject;
            var paramOnlyObjectBases = avatar.GetComponentsInChildren<ModularAvatarExtensionsParamOnlyObjectBase>(true)
                .Where(c => c).Select(x => x.gameObject).Distinct().ToDictionary(x => x,
                    x => x.GetComponents<ModularAvatarExtensionsParamOnlyObjectBase>());

            LogC($"Find {paramOnlyObjectBases.Count} is param only object inside \"{avatar.FullName()}\"");

            if (paramOnlyObjectBases.Count < 1) return;

            var asc = ctx.Extension<AnimatorServicesContext>();

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
                        var passData = new ParamPassData(allParams, reverse);
                        _gameObjectWithParamData.TryAdd(gameObject, passData);
                        _paramNames.UnionWith(allParams.Select(x => x.paramName));
                    }
                    catch (Exception e)
                    {
                        ErrorReport.ReportException(e);
                        return;
                    }

            var fx = asc.ControllerContext.Controllers[VRCAvatarDescriptor.AnimLayerType.FX];
            if (fx == null) return;

            var nullMotion = new AnimationClip
            {
                name = "_"
            };

            var blendTree = new BlendTree
            {
                blendType = BlendTreeType.Direct,
                useAutomaticThresholds = false,
                children = _gameObjectWithParamData
                    .SelectMany(prop => GenerateChild(asc, nullMotion, prop.Key, prop.Value))
                    .ToArray()
            };

            var layer = fx.AddLayer(new(999999), "Modular Avatar EX: Param Only");
            var state = layer.StateMachine!.AddState("Param Only");
            layer.StateMachine.DefaultState = state;

            state.WriteDefaultValues = true;
            state.Motion = asc.ControllerContext.Clone(blendTree);

            foreach (var paramName in _paramNames.Where(paramName => !fx.Parameters.TryGetValue(paramName, out _)))
                fx.Parameters = fx.Parameters.Add(paramName, new()
                {
                    name = paramName,
                    type = AnimatorControllerParameterType.Float,
                    defaultFloat = 0
                });
        }

        private static void Toggling(AnimatorServicesContext asc, AnimationClip clip, GameObject[] gameObjects,
            bool active)
        {
            var curve = new AnimationCurve();
            curve.AddKey(0, active ? 1 : 0);

            foreach (var activeBinding in gameObjects
                         .Select(gameObject => asc.ObjectPathRemapper.GetVirtualPathForObject(gameObject)).Select(path =>
                             EditorCurveBinding.FloatCurve(path, typeof(GameObject), "m_IsActive")))
                AnimationUtility.SetEditorCurve(clip, activeBinding, curve);
        }

        private static void Toggling(AnimatorServicesContext asc, AnimationClip clip, GameObject gameObject,
            bool active) => Toggling(asc, clip, new[]
        {
            gameObject
        }, active);

        private static ChildMotion[] GenerateChild(AnimatorServicesContext asc, Motion nullMotion, GameObject gameObject,
            ParamPassData data)
        {
            var clip = new AnimationClip
            {
                name = data.Reverse ? "Enable" : "Disable"
            };

            Toggling(asc, clip, gameObject, data.Reverse);

            return data.ParamDatas.Select(paramData => paramData.paramValue > 0
                ? new ChildMotion
                {
                    motion = new BlendTree
                    {
                        name = $"{gameObject.name} - {paramData.paramName} - {paramData.paramValue}",
                        blendType = BlendTreeType.Simple1D,
                        useAutomaticThresholds = false,
                        blendParameter = paramData.paramName,
                        children = new[]
                        {
                            new ChildMotion
                            {
                                motion = clip,
                                timeScale = 1,
                                threshold = 0
                            },
                            new ChildMotion
                            {
                                motion = nullMotion,
                                timeScale = 1,
                                threshold = paramData.paramValue
                            }
                        }
                    },
                    directBlendParameter = GameObjectDelayDisablePass.AlwaysOne,
                    timeScale = 1
                }
                : new()
                {
                    motion = new BlendTree
                    {
                        name = $"{gameObject.name} - {paramData.paramName} - {paramData.paramValue}",
                        blendType = BlendTreeType.Simple1D,
                        useAutomaticThresholds = false,
                        blendParameter = paramData.paramName,
                        children = new[]
                        {
                            new ChildMotion
                            {
                                motion = nullMotion,
                                timeScale = 1,
                                threshold = paramData.paramValue
                            },
                            new ChildMotion
                            {
                                motion = clip,
                                timeScale = 1,
                                threshold = 1
                            }
                        }
                    },
                    directBlendParameter = GameObjectDelayDisablePass.AlwaysOne,
                    timeScale = 1
                }).ToArray();
        }
    }
}
#endif