#if MAEX_VRCSDK3_BASE
using System.Collections.Generic;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Modular Avatar EX/MAEX Param-Only Object")]
    public class ModularAvatarExtensionsParamOnlyObject : ModularAvatarExtensionsParamOnlyObjectBase
    {
        [SerializeField] private List<ParamData> paramDatas = new();

        public override List<ParamData> ParamDatas => paramDatas;
    }
}
#endif