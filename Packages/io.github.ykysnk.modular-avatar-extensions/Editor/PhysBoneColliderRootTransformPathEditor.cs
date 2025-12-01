using UnityEditor;
using VRC.SDK3.Dynamics.PhysBone.Components;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(ModularAvatarExtensionsPhysBoneColliderRootTransformPath))]
[CanEditMultipleObjects]
public class PhysBoneColliderRootTransformPathEditor : RootTransformPathEditorBase<VRCPhysBoneCollider>
{
    protected override string RootTransformType => "phys_bone_collider";
}