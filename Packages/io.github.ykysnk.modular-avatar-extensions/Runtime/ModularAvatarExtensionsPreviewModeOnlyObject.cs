#if MAEX_VRCSDK3_BASE
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Modular Avatar EX/MAEX Preview Mode Only Object")]
    public class ModularAvatarExtensionsPreviewModeOnlyObject : ModularAvatarExtensionsParamOnlyObjectBase
    {
        protected override void OnChange()
        {
            paramDatas = new()
            {
                new("PreviewMode", !reverse)
            };
        }
    }
}
#endif