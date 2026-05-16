using System.Linq;
using nadena.dev.ndmf;
using nadena.dev.ndmf.animator;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor.Animation
{
    // Copy from nadena.dev.modular_avatar.animation.GameObjectDelayDisablePass
    internal class GameObjectDelayDisablePass : Pass<GameObjectDelayDisablePass>
    {
        internal const string AlwaysOne = "__ModularAvatarInternal/One";

        protected override void Execute(BuildContext context)
        {
            var asc = context.Extension<AnimatorServicesContext>();
            var activeProxies = context.GetState<ReadablePropertyExtension.Retained>().ProxyProps
                .ToDictionary(kv => kv.Key, kv => kv.Value);
            if (activeProxies.Count == 0) return;

            var usedProxies = asc.ControllerContext.Controllers[VRCAvatarDescriptor.AnimLayerType.FX]
                .AllReachableNodes().OfType<VirtualTransitionBase>()
                .SelectMany(t => t.Conditions)
                .Select(c => c.parameter)
                .ToHashSet();

            foreach (var proxyBinding in activeProxies.ToList()
                         .Where(proxyBinding => !usedProxies.Contains(proxyBinding.Value)))
                activeProxies.Remove(proxyBinding.Key);

            var fx = asc.ControllerContext.Controllers[VRCAvatarDescriptor.AnimLayerType.FX];
            if (fx == null) return;

            var nullMotion = new AnimationClip
            {
                name = "NullMotion"
            };

            var blendTree = new BlendTree
            {
                blendType = BlendTreeType.Direct,
                useAutomaticThresholds = false,
                children = activeProxies
                    .Select(prop => GenerateDelayChild(nullMotion, (prop.Key, prop.Value)))
                    .ToArray()
            };

            var layer = fx.AddLayer(LayerPriority.Default, "DelayDisable");
            var state = layer.StateMachine!.AddState("DelayDisable");
            layer.StateMachine.DefaultState = state;

            state.WriteDefaultValues = true;
            state.Motion = asc.ControllerContext.Clone(blendTree);

            foreach (var controller in asc.ControllerContext.GetAllControllers())
            foreach (var (binding, prop) in activeProxies)
            {
                var obj = asc.ObjectPathRemapper.GetObjectForPath(binding.path);

                if (obj == null || !controller.Parameters.TryGetValue(prop, out var p)) continue;

                p.defaultFloat = obj.activeSelf ? 1 : 0;
                controller.Parameters = controller.Parameters.SetItem(prop, p);
            }
        }

        private static ChildMotion GenerateDelayChild(Motion nullMotion, (EditorCurveBinding, string) binding)
        {
            var ecb = binding.Item1;
            var prop = binding.Item2;

            var motion = new AnimationClip();
            var curve = new AnimationCurve();
            curve.AddKey(0, 1);
            AnimationUtility.SetEditorCurve(motion, ecb, curve);

            var bufferBlendTree = new BlendTree
            {
                blendType = BlendTreeType.Simple1D,
                useAutomaticThresholds = false,
                blendParameter = prop,
                children = new[]
                {
                    new ChildMotion
                    {
                        motion = nullMotion,
                        timeScale = 1,
                        threshold = 0
                    },
                    new ChildMotion
                    {
                        motion = nullMotion,
                        timeScale = 1,
                        threshold = 0.01f
                    },
                    new ChildMotion
                    {
                        motion = motion,
                        timeScale = 1,
                        threshold = 1
                    }
                }
            };

            return new()
            {
                motion = bufferBlendTree,
                directBlendParameter = AlwaysOne,
                timeScale = 1
            };
        }
    }
}