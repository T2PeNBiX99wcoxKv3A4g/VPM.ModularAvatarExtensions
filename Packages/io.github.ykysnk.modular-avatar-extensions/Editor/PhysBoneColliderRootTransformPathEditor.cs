#if MAEX_VRCSDK3_BASE
using UnityEditor;
using VRC.SDK3.Dynamics.PhysBone.Components;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [CustomEditor(typeof(ModularAvatarExtensionsPhysBoneColliderRootTransformPath))]
    [CanEditMultipleObjects]
    internal class PhysBoneColliderRootTransformPathEditor : RootTransformPathEditorBase<VRCPhysBoneCollider>
    {
    }
#endif
}