#if MAEX_VRCSDK3_BASE
using UnityEngine;
using VRC.SDK3.Dynamics.Contact.Components;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [AddComponentMenu("Modular Avatar EX/MAEX Contact Receiver Root Transform Path")]
    public class ModularAvatarExtensionsContactReceiverRootTransformPath : RootTransformPathBase<VRCContactReceiver>
    {
    }
}
#endif