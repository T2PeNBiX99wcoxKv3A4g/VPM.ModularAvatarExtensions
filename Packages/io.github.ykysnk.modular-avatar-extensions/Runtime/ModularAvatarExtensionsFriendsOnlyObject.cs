#if MAEX_VRCSDK3_BASE
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Modular Avatar EX/MAEX Friends Only Object")]
    public class ModularAvatarExtensionsFriendsOnlyObject : ModularAvatarExtensionsParamOnlyObjectBase
    {
        protected override void OnChange()
        {
            paramDatas = new()
            {
                new("IsOnFriendsList", !reverse)
            };
        }
    }
}
#endif