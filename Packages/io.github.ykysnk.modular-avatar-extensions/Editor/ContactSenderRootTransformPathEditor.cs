#if MAEX_VRCSDK3_BASE
using UnityEditor;
using VRC.SDK3.Dynamics.Contact.Components;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [CustomEditor(typeof(ModularAvatarExtensionsContactSenderRootTransformPath))]
    [CanEditMultipleObjects]
    internal class ContactSenderRootTransformPathEditor : RootTransformPathEditorBase<VRCContactSender>
    {
        protected override string RootTransformType => "contact_sender";
    }
#endif
}