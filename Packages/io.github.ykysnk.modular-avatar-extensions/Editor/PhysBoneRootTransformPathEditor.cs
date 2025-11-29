using io.github.ykysnk.utils.Editor;
using UnityEditor;
using VRC.SDK3.Dynamics.PhysBone.Components;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(ModularAvatarExtensionsPhysBoneRootTransformPath))]
[CanEditMultipleObjects]
public class PhysBoneRootTransformPathEditor : RootTransformPathEditorBase<VRCPhysBone>
{
    private const string IgnoreTransformsReferencesProp = "ignoreTransformsReferences";
    private const string ColliderReferencesProp = "colliderReferences";
    private const string SetIgnoreTransformsReferencesProp = "setIgnoreTransforms";
    private const string SetCollidersReferencesProp = "setColliders";

    private SerializedProperty? _colliderReferencesReferences;
    private SerializedProperty? _ignoreTransformsReferences;
    private SerializedProperty? _setCollidersReferences;
    private SerializedProperty? _setIgnoreTransformsReferences;

    protected override string ComponentLabel => "Phys Bone";
    protected override string RootTransformType => "phys bone";

    protected override void OnEnable()
    {
        base.OnEnable();
        _ignoreTransformsReferences = serializedObject.FindProperty(IgnoreTransformsReferencesProp);
        _colliderReferencesReferences = serializedObject.FindProperty(ColliderReferencesProp);
        _setIgnoreTransformsReferences = serializedObject.FindProperty(SetIgnoreTransformsReferencesProp);
        _setCollidersReferences = serializedObject.FindProperty(SetCollidersReferencesProp);
    }

    protected override void OnInspectorGUIDraw()
    {
        EditorGUILayout.PropertyField(_setIgnoreTransformsReferences, Utils.Label("Set Ignore Transforms"));
        EditorGUILayout.PropertyField(_ignoreTransformsReferences, Utils.Label("Ignore Transforms"));
        EditorGUILayout.PropertyField(_setCollidersReferences, Utils.Label("Set Colliders"));
        EditorGUILayout.PropertyField(_colliderReferencesReferences, Utils.Label("Colliders"));
    }
}