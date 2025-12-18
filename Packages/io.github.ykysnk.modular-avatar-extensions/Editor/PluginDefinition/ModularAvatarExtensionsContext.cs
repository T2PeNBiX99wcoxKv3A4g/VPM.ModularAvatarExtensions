using System;
using JetBrains.Annotations;
using nadena.dev.ndmf;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor.PluginDefinition
{
    // Copy from nadena.dev.modular_avatar.core.editor.ModularAvatarContext
    [PublicAPI]
    internal class ModularAvatarExtensionsContext : IExtensionContext
    {
        private IDisposable? _toDispose;
        internal MaexBuildContext? BuildContext { get; private set; }

        public void OnActivate(BuildContext context)
        {
            BuildContext ??= new(context);
        }

        public void OnDeactivate(BuildContext context)
        {
            _toDispose?.Dispose();
            _toDispose = null;
        }
    }
}