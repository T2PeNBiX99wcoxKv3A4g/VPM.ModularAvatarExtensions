using JetBrains.Annotations;
using nadena.dev.ndmf;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [PublicAPI]
    internal class MaexBuildContext
    {
        internal readonly BuildContext PluginBuildContext;

        public MaexBuildContext(BuildContext pluginBuildContext) => PluginBuildContext = pluginBuildContext;

        internal GameObject AvatarRootObject => PluginBuildContext.AvatarRootObject;
        internal Transform AvatarRootTransform => PluginBuildContext.AvatarRootTransform;
    }
}