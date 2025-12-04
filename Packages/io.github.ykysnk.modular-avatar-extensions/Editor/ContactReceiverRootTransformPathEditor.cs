using UnityEditor;
using VRC.SDK3.Dynamics.Contact.Components;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(ModularAvatarExtensionsContactReceiverRootTransformPath))]
[CanEditMultipleObjects]
internal class ContactReceiverRootTransformPathEditor : RootTransformPathEditorBase<VRCContactReceiver>
{
    protected override string RootTransformType => "contact_receiver";
}