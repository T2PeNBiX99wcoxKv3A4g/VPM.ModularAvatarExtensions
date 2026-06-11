using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using nadena.dev.ndmf.preview;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor.ReactiveObjects
{
    internal class TurnOnOffPreview : IRenderFilter
    {
        private static readonly TogglablePreviewNode EnableNode = TogglablePreviewNode.Create(
            () => "Turn On or Off",
            "io.github.ykysnk.ModularAvatarExtensions/TurnOnOffPreview"
        );

        public bool CanEnableRenderers => true;

        public IEnumerable<TogglablePreviewNode> GetPreviewControlNodes()
        {
            yield return EnableNode;
        }

        public bool IsEnabled(ComputeContext context) => context.Observe(EnableNode.IsEnabled);

        public ImmutableList<RenderGroup> GetTargetGroups(ComputeContext context)
        {
            var roots = context.GetAvatarRoots();
            return roots.SelectMany(av => RootsForAvatar(context, av)).ToImmutableList();
        }

        public Task<IRenderFilterNode> Instantiate(RenderGroup group, IEnumerable<(Renderer, Renderer)> proxyPairs,
            ComputeContext context) =>
            Task.FromResult<IRenderFilterNode>(new Node(group.GetData<bool>()));

        private static IEnumerable<RenderGroup> RootsForAvatar(ComputeContext context, GameObject avatarRoot)
        {
            if (!context.ActiveInHierarchy(avatarRoot))
                yield break;

            var renderers = context.GetComponentsInChildren<Renderer>(avatarRoot, true);

            foreach (var renderer in renderers)
            {
                if (renderer is not MeshRenderer and not SkinnedMeshRenderer) continue;
                var currentlyEnabled = context.ActiveInHierarchy(renderer.gameObject);
                var overrideEnabled = currentlyEnabled;

                if (renderer.GetComponent<ModularAvatarExtensionsTurnOffInBuild>())
                    overrideEnabled = false;
                else if (renderer.GetComponent<ModularAvatarExtensionsTurnOnInBuild>())
                    overrideEnabled = true;

                if (overrideEnabled != currentlyEnabled)
                    yield return RenderGroup.For(renderer).WithData(overrideEnabled, (x, y) => x == y);
            }
        }

        private class Node : IRenderFilterNode
        {
            private readonly bool _shouldEnable;

            public Node(bool shouldEnable) => _shouldEnable = shouldEnable;
            public RenderAspects WhatChanged => 0;

            public void OnFrame(Renderer original, Renderer proxy)
            {
                proxy.enabled = _shouldEnable;
            }
        }
    }
}