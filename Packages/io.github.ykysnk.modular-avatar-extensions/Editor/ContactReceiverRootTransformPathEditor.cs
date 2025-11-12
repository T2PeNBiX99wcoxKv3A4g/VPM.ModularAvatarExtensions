using UnityEditor;
using VRC.SDK3.Dynamics.Contact.Components;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(ContactReceiverRootTransformPath))]
[CanEditMultipleObjects]
public class ContactReceiverRootTransformPathEditor : RootTransformPathEditorBase<VRCContactReceiver>
{
    protected override string ComponentLabel => "Contact Receiver";
    protected override string RootTransformType => "contact receiver";
}