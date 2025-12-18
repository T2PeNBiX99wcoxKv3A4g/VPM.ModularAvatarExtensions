#if MAEX_VRCSDK3_BASE
using System.Collections.Generic;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Modular Avatar EX/MAEX Local Only Object")]
    public class ModularAvatarExtensionsLocalOnlyObject : ModularAvatarExtensionsParamOnlyObjectBase
    {
        public override List<ParamData> ParamDatas => new()
        {
            new("IsLocal", !reverse)
        };
    }
}
#endif