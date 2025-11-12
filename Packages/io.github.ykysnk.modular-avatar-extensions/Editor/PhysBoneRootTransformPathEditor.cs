using io.github.ykysnk.utils.Editor;
using UnityEditor;
using VRC.SDK3.Dynamics.PhysBone.Components;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(PhysBoneRootTransformPath))]
[CanEditMultipleObjects]
public class PhysBoneRootTransformPathEditor : RootTransformPathEditorBase<VRCPhysBone>
{
    private const string IgnoreTransformsReferencesProp = "ignoreTransformsReferences";
    private const string ColliderReferencesProp = "colliderReferences";
    private SerializedProperty? _colliderReferencesReferences;

    private SerializedProperty? _ignoreTransformsReferences;
    protected override string ComponentLabel => "Phys Bone";
    protected override string RootTransformType => "phys bone";

    protected override void OnEnable()
    {
        base.OnEnable();
        _ignoreTransformsReferences = serializedObject.FindProperty(IgnoreTransformsReferencesProp);
        _colliderReferencesReferences = serializedObject.FindProperty(ColliderReferencesProp);
    }

    protected override void OnInspectorGUIDraw()
    {
        EditorGUILayout.PropertyField(_ignoreTransformsReferences, Utils.Label("Ignore Transforms"));
        EditorGUILayout.PropertyField(_colliderReferencesReferences, Utils.Label("Colliders"));
    }
}