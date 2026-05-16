#if MAEX_VRCSDK3_BASE
using System.Collections.Generic;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Modular Avatar EX/MAEX Preview Mode-Only Object")]
    public class ModularAvatarExtensionsPreviewModeOnlyObject : ModularAvatarExtensionsParamOnlyObjectBase
    {
        public override List<ParamData> ParamDatas => new()
        {
            new("PreviewMode", !reverse)
        };
    }
}
#endif