using System.Linq;
using UnityEditor;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(ModularAvatarExtensionsTransformFollower))]
[CanEditMultipleObjects]
public class TransformFollowerEditor : MaexEditor
{
    private const string IsLockProp = "isLock";
    private const string ReferenceProp = "reference";
    private const string PositionOffsetProp = "positionOffset";
    private const string RotationOffsetProp = "rotationOffset";
    private const string ScaleOffsetProp = "scaleOffset";
    private const string IsLockPositionProp = "isLockPosition";
    private const string IsLockRotationProp = "isLockRotation";
    private const string IsLockScaleProp = "isLockScale";

    private SerializedProperty? _isLock;
    private SerializedProperty? _isLockPosition;
    private SerializedProperty? _isLockRotation;
    private SerializedProperty? _isLockScale;
    private SerializedProperty? _positionOffset;
    private SerializedProperty? _reference;
    private SerializedProperty? _rotationOffset;
    private SerializedProperty? _scaleOffset;

    protected override void OnEnable()
    {
        _isLock = serializedObject.FindProperty(IsLockProp);
        _reference = serializedObject.FindProperty(ReferenceProp);
        _positionOffset = serializedObject.FindProperty(PositionOffsetProp);
        _rotationOffset = serializedObject.FindProperty(RotationOffsetProp);
        _scaleOffset = serializedObject.FindProperty(ScaleOffsetProp);
        _isLockPosition = serializedObject.FindProperty(IsLockPositionProp);
        _isLockRotation = serializedObject.FindProperty(IsLockRotationProp);
        _isLockScale = serializedObject.FindProperty(IsLockScaleProp);
    }

    protected override void OnInnerInspectorGUI()
    {
        var transformFollowers = targets.Select(t => (ModularAvatarExtensionsTransformFollower)t).ToArray();

        EditorGUILayout.BeginHorizontal();

        var activateButton = GUILayout.Button("label.transform_follower.activate_button".G());
        var zeroButton = GUILayout.Button("label.transform_follower.zero_button".G());

        EditorGUILayout.EndHorizontal();

        foreach (var transformFollower in transformFollowers)
            if (activateButton)
                transformFollower.ActivateConstraint();
            else if (zeroButton)
                transformFollower.ZeroConstraint();

        EditorGUILayout.PropertyField(_isLock, "label.transform_follower.is_lock".G());
        EditorGUILayout.PropertyField(_reference, "label.transform_follower.reference".G());
        EditorGUILayout.PropertyField(_positionOffset, "label.transform_follower.position_offset".G());
        EditorGUILayout.PropertyField(_rotationOffset, "label.transform_follower.rotation_offset".G());
        EditorGUILayout.PropertyField(_scaleOffset, "label.transform_follower.scale_offset".G());
        EditorGUILayout.PropertyField(_isLockPosition, "label.transform_follower.is_lock_position".G());
        EditorGUILayout.PropertyField(_isLockRotation, "label.transform_follower.is_lock_rotation".G());
        EditorGUILayout.PropertyField(_isLockScale, "label.transform_follower.is_lock_scale".G());
        EditorGUILayout.HelpBox("label.transform_follower.warning".S(), MessageType.Warning, true);
    }
}