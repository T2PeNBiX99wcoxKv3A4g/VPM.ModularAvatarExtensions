#if MAEX_VRCSDK3_BASE
using UnityEditor;
using VRC.SDK3.Dynamics.PhysBone.Components;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [CustomEditor(typeof(ModularAvatarExtensionsPhysBoneRootTransformPath))]
    [CanEditMultipleObjects]
    internal class PhysBoneRootTransformPathEditor : RootTransformPathEditorBase<VRCPhysBone>
    {
        private const string IgnoreTransformsReferencesProp = "ignoreTransformsReferences";
        private const string ColliderReferencesProp = "colliderReferences";
        private const string SetIgnoreTransformsReferencesProp = "setIgnoreTransforms";
        private const string SetCollidersReferencesProp = "setColliders";

        private SerializedProperty? _colliderReferencesReferences;
        private SerializedProperty? _ignoreTransformsReferences;
        private SerializedProperty? _setCollidersReferences;
        private SerializedProperty? _setIgnoreTransformsReferences;

        protected override string RootTransformType => "phys_bone";

        protected override void OnEnable()
        {
            base.OnEnable();
            _ignoreTransformsReferences = serializedObject.FindProperty(IgnoreTransformsReferencesProp);
            _colliderReferencesReferences = serializedObject.FindProperty(ColliderReferencesProp);
            _setIgnoreTransformsReferences = serializedObject.FindProperty(SetIgnoreTransformsReferencesProp);
            _setCollidersReferences = serializedObject.FindProperty(SetCollidersReferencesProp);
        }

        protected override void OnInnerInspectorGUI()
        {
            EditorGUILayout.PropertyField(_setIgnoreTransformsReferences,
                "label.phys_bone_root_transform_path.set_ignore_transforms".G());
            EditorGUILayout.PropertyField(_ignoreTransformsReferences,
                "label.phys_bone_root_transform_path.ignore_transforms".G());
            EditorGUILayout.PropertyField(_setCollidersReferences,
                "label.phys_bone_root_transform_path.set_colliders".G());
            EditorGUILayout.PropertyField(_colliderReferencesReferences,
                "label.phys_bone_root_transform_path.colliders".G());
        }
    }
#endif
}