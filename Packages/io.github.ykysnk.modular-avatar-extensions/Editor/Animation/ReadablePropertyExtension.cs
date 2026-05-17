using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using nadena.dev.ndmf;
using nadena.dev.ndmf.animator;
using UnityEditor;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor.Animation
{
    // Copy from nadena.dev.modular_avatar.animation.ReadablePropertyExtension
    [DependsOnContext(typeof(AnimatorServicesContext))]
    internal class ReadablePropertyExtension : IExtensionContext
    {
        private AnimatorServicesContext? _asc;
        private int _index;
        private Retained _retained = null!;

        private AnimatorServicesContext Asc =>
            _asc ?? throw new InvalidOperationException("ActiveSelfProxyExtension is not active");

        private Dictionary<EditorCurveBinding, string> ProxyProps => _retained.ProxyProps;

        [PublicAPI]
        public IEnumerable<(EditorCurveBinding, string)> ActiveProxyProps =>
            ProxyProps.Select(kvp => (kvp.Key, kvp.Value));

        public void OnActivate(BuildContext context)
        {
            _asc = context.Extension<AnimatorServicesContext>();
            _retained = context.GetState<Retained>();
        }

        public void OnDeactivate(BuildContext context)
        {
            Asc.AnimationIndex.EditClipsByBinding(ProxyProps.Keys, clip =>
            {
                foreach (var b in clip.GetFloatCurveBindings().ToList())
                {
                    if (!ProxyProps.TryGetValue(b, out var proxyProp)) continue;
                    var curve = clip.GetFloatCurve(b);
                    clip.SetFloatCurve("", typeof(Animator), proxyProp, curve);
                }
            });
        }

        [PublicAPI]
        public string GetActiveSelfProxy(GameObject obj)
        {
            var path = Asc.ObjectPathRemapper.GetVirtualPathForObject(obj);
            var ecb = EditorCurveBinding.FloatCurve(path, typeof(GameObject), "m_IsActive");

            if (ProxyProps.TryGetValue(ecb, out var prop)) return prop;

            prop = $"__MAEX/ActiveSelfProxy/{obj.name}##{_index++}";
            ProxyProps[ecb] = prop;

            foreach (var animator in Asc.ControllerContext.GetAllControllers())
                animator.Parameters = animator.Parameters.SetItem(
                    prop,
                    new()
                    {
                        name = prop,
                        type = AnimatorControllerParameterType.Float,
                        defaultFloat = obj.activeSelf ? 1 : 0
                    }
                );

            return prop;
        }

        [PublicAPI]
        public class Retained
        {
            public readonly Dictionary<EditorCurveBinding, string> ProxyProps = new();
        }
    }
}