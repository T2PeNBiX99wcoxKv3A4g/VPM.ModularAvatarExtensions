using UnityEditor;
using VRC.SDK3.Dynamics.Contact.Components;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(ContactSenderRootTransformPath))]
[CanEditMultipleObjects]
public class ContactSenderRootTransformPathEditor : RootTransformPathEditorBase<VRCContactSender>
{
    protected override string ComponentLabel => "Contact Sender";
    protected override string RootTransformType => "contact sender";
}