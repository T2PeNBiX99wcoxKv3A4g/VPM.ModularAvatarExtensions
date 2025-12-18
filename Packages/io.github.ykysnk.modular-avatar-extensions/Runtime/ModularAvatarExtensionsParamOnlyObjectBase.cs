#if MAEX_VRCSDK3_BASE
using System.Collections.Generic;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    public abstract class ModularAvatarExtensionsParamOnlyObjectBase : AvatarMaexComponent
    {
        public bool reverse;
        public bool highPriority;
        public List<ParamData> paramDatas = new();
    }
}
#endif