using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using nadena.dev.ndmf;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor.PluginDefinition
{
    // Copy from nadena.dev.modular_avatar.core.editor.ModularAvatarContext
    internal class ModularAvatarExtensionsContext : IExtensionContext
    {
        private Retained _retained = null!;
        private IDisposable? _toDispose;
        internal MaexBuildContext? BuildContext { get; private set; }

        public void OnActivate(BuildContext context)
        {
            BuildContext ??= new(context);
            _retained = context.GetState<Retained>();
        }

        public void OnDeactivate(BuildContext context)
        {
            _toDispose?.Dispose();
            _toDispose = null;
        }

        [PublicAPI]
        public void AddComponent(AvatarMaexComponent component, string? path)
        {
            if (component == null) return;
            _retained.ComponentFullPathProps[component] = path ?? component.name;
        }

        [PublicAPI]
        public class Retained
        {
            public readonly Dictionary<AvatarMaexComponent, string> ComponentFullPathProps = new();
        }
    }
}