using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using io.github.ykysnk.utils.Extensions;
using nadena.dev.ndmf;
using nadena.dev.ndmf.animator;
using UnityEditor.Animations;
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using VRC.SDKBase;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    internal class LayerIndexControlPass : MaexPass<LayerIndexControlPass>
    {
        private const string MmdLayerName = "Modular Avatar: MMD Control";
        private const string MergeBlendTreeLayerName = "ModularAvatar: Merge Blend Tree";
        private const string DummyLayerName = "Modular Avatar Extensions: MMD Dummy";
        private const string DummyParameterName = "__MAEX/Internal/IsDummy";
        private const string IsLocalParameterName = "IsLocal";
        private const int MmdLayerMustIndex = 2;

        public override string QualifiedName => "io.github.ykysnk.ModularAvatarExtensions.LayerIndexControl";
        public override string DisplayName => "Modular Avatar Extensions Layer Index Control";

        protected override void Execute(BuildContext ctx)
        {
            var avatar = ctx.AvatarRootObject;
            var layerFix = avatar.GetComponentInChildren<ModularAvatarExtensionsLayerFix>(true);

            if (layerFix != null && layerFix.FixMmdLayer)
            {
                var asc = ctx.Extension<AnimatorServicesContext>();
                var fx = asc.ControllerContext.Controllers[VRCAvatarDescriptor.AnimLayerType.FX];

                var index = 0;
                var found = 0;
                VirtualLayer? mmdLayer = null;
                var mmdLayerIndex = 0;
                VirtualLayer? mergeBlendTreeLayer = null;
                var mergeBlendTreeLayerIndex = 0;

                foreach (var layer in fx.Layers)
                {
                    if (found > 1) break;

                    switch (layer.Name)
                    {
                        case MmdLayerName:
                            mmdLayer = layer;
                            mmdLayerIndex = index;
                            found++;
                            break;
                        case MergeBlendTreeLayerName:
                            mergeBlendTreeLayer = layer;
                            mergeBlendTreeLayerIndex = index;
                            found++;
                            break;
                    }

                    index++;
                }

                if (found < 1)
                {
                    LogC("MA MMD layer and merge blend tree layer are not found!");
                    // TODO: Localization
                    // LogNonFatal();
                    return;
                }

                var currentLayers = fx.Layers.ToList();
                var newLayers = new List<VirtualLayer>();
                var moveMmdLayer = false;

                if (mmdLayer != null)
                {
                    LogC($"MA MMD layer found! -> Name: {mmdLayer.Name} | Index: {mmdLayerIndex}");

                    if (mmdLayerIndex > MmdLayerMustIndex)
                    {
                        for (var i = 0; i < MmdLayerMustIndex; i++)
                        {
                            if (i is > 0 and < MmdLayerMustIndex)
                            {
                                CreateDummyLayer(fx, newLayers);
                                continue;
                            }

                            if (!currentLayers.TryGetValue(i, out var layer)) continue;
                            newLayers.Add(layer);
                            currentLayers.Remove(layer);
                        }

                        currentLayers.Remove(mmdLayer);
                        newLayers.Add(mmdLayer);
                        moveMmdLayer = true;
                        LogC($"MA MMD layer moved to index {MmdLayerMustIndex}!");
                    }
                }

                if (mergeBlendTreeLayer != null)
                {
                    LogC(
                        $"MA merge blend tree layer -> Name: {mergeBlendTreeLayer.Name} | Index: {mergeBlendTreeLayerIndex}");

                    if (mergeBlendTreeLayerIndex < MmdLayerMustIndex + 1)
                    {
                        if (!moveMmdLayer)
                            for (var i = 0; i < MmdLayerMustIndex + 1; i++)
                            {
                                if (i is > 0 and < MmdLayerMustIndex + 1)
                                {
                                    CreateDummyLayer(fx, newLayers);
                                    continue;
                                }

                                if (!currentLayers.TryGetValue(i, out var layer)) continue;
                                newLayers.Add(layer);
                                currentLayers.Remove(layer);
                            }

                        currentLayers.Remove(mergeBlendTreeLayer);
                        newLayers.Add(mergeBlendTreeLayer);
                        LogC($"MA merge blend tree layer moved to index {MmdLayerMustIndex + 1}!");
                    }
                }

                fx.Layers = newLayers.Concat(currentLayers).ToArray();
            }

            // TODO: index control
        }

        private static void CreateDummyLayer(VirtualAnimatorController fx, List<VirtualLayer> newLayers)
        {
            var dummy = fx.AddLayer(new(0), DummyLayerName);
            var s = dummy.StateMachine!.DefaultState = dummy.StateMachine.AddState("Dummy");
            var s2 = dummy.StateMachine.AddState("Dummy2");
            s.Motion = VirtualClip.Create("_");
            var behaviours = new List<StateMachineBehaviour>();
            var driver = ScriptableObject.CreateInstance<VRC_AvatarParameterDriver>();

            var parameter = fx.Parameters.FirstOrDefault(x => x.Key == DummyParameterName);

            if (parameter.Value == null)
                fx.Parameters = fx.Parameters.Add(DummyParameterName, new()
                {
                    name = DummyParameterName,
                    type = AnimatorControllerParameterType.Int,
                    defaultInt = 0
                });

            driver.parameters = new()
            {
                new()
                {
                    name = DummyParameterName,
                    type = VRC_AvatarParameterDriver.ChangeType.Copy,
                    source = IsLocalParameterName
                }
            };

            behaviours.Add(driver);
            s2.Behaviours = behaviours.ToImmutableList();

            var sTos2 = VirtualStateTransition.Create();
            sTos2.SetDestination(s2);

            var sTos2Conditions = new List<AnimatorCondition>
            {
                new()
                {
                    parameter = IsLocalParameterName,
                    mode = AnimatorConditionMode.NotEqual,
                    threshold = 0
                }
            };

            sTos2.Conditions = sTos2Conditions.ToImmutableList();
            newLayers.Add(dummy);
        }
    }
}