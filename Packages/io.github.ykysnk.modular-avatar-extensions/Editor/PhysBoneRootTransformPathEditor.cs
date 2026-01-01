#if MAEX_VRCSDK3_BASE
using UnityEditor;
using VRC.SDK3.Dynamics.PhysBone.Components;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [CustomEditor(typeof(ModularAvatarExtensionsPhysBoneRootTransformPath))]
    [CanEditMultipleObjects]
    internal class PhysBoneRootTransformPathEditor : RootTransformPathEditorBase<VRCPhysBone>
    {
    }
#endif
}