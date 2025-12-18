#if MAEX_VRCSDK3_BASE
using System.Collections.Generic;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Modular Avatar EX/MAEX Friends Only Object")]
    public class ModularAvatarExtensionsFriendsOnlyObject : ModularAvatarExtensionsParamOnlyObjectBase
    {
        public override List<ParamData> ParamDatas => new()
        {
            new("IsOnFriendsList", !reverse)
        };
    }
}
#endif