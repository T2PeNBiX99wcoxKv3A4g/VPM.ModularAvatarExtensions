#if MAEX_VRCSDK3_BASE
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Modular Avatar EX/MAEX Local Only Object")]
    public class ModularAvatarExtensionsLocalOnlyObject : ModularAvatarExtensionsParamOnlyObjectBase
    {
        protected override void OnChange()
        {
            paramDatas = new()
            {
                new("IsLocal", !reverse)
            };
        }
    }
}
#endif