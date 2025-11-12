using UnityEditor;
using VRC.SDK3.Dynamics.PhysBone.Components;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(PhysBoneColliderRootTransformPath))]
[CanEditMultipleObjects]
public class PhysBoneColliderRootTransformPathEditor : RootTransformPathEditorBase<VRCPhysBoneCollider>
{
    protected override string ComponentLabel => "Phys Bone Collider";
    protected override string RootTransformType => "phys bone collider";
}