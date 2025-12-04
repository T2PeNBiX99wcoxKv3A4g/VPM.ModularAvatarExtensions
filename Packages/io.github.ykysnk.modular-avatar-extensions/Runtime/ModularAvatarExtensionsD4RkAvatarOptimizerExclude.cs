#if D4RK_AVATAR_OPTIMIZER
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Modular Avatar EX/MAEX d4rk Avatar Optimizer Exclude")]
    public class ModularAvatarExtensionsD4RkAvatarOptimizerExclude : AvatarMaexComponent
    {
        public override bool DontDestroyOnBuild => true;
    }
}
#endif