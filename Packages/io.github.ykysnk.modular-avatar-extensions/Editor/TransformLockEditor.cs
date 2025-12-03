using System.Linq;
using io.github.ykysnk.Localization.Editor;
using UnityEditor;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[CustomEditor(typeof(ModularAvatarExtensionsTransformLock))]
[CanEditMultipleObjects]
public class TransformLockEditor : MaexEditor
{
    private const string IsLockProp = "isLock";
    private const string LockPositionProp = "lockPosition";
    private const string LockRotationProp = "lockRotation";
    private const string LockScaleProp = "lockScale";
    private const string IsLockPositionProp = "isLockPosition";
    private const string IsLockRotationProp = "isLockRotation";
    private const string IsLockScaleProp = "isLockScale";

    private SerializedProperty? _isLock;
    private SerializedProperty? _isLockPosition;
    private SerializedProperty? _isLockRotation;
    private SerializedProperty? _isLockScale;
    private SerializedProperty? _lockPosition;
    private SerializedProperty? _lockRotation;
    private SerializedProperty? _lockScale;

    protected override void OnEnable()
    {
        _isLock = serializedObject.FindProperty(IsLockProp);
        _lockPosition = serializedObject.FindProperty(LockPositionProp);
        _lockRotation = serializedObject.FindProperty(LockRotationProp);
        _lockScale = serializedObject.FindProperty(LockScaleProp);
        _isLockPosition = serializedObject.FindProperty(IsLockPositionProp);
        _isLockRotation = serializedObject.FindProperty(IsLockRotationProp);
        _isLockScale = serializedObject.FindProperty(IsLockScaleProp);
    }

    protected override void OnMaexInspectorGUI()
    {
        var transformLocks = targets.Select(t => (ModularAvatarExtensionsTransformLock)t).ToArray();

        EditorGUILayout.BeginHorizontal();

        var activateButton = GUILayout.Button("label.transform_lock.activate_button".G(LocalizationID));
        var zeroButton = GUILayout.Button("label.transform_lock.zero_button".G(LocalizationID));

        EditorGUILayout.EndHorizontal();

        foreach (var transformLock in transformLocks)
            if (activateButton)
                transformLock.ActivateConstraint();
            else if (zeroButton)
                transformLock.ZeroConstraint();

        EditorGUILayout.PropertyField(_isLock, "label.transform_lock.is_lock".G(LocalizationID));
        EditorGUILayout.PropertyField(_lockPosition, "label.transform_lock.lock_position".G(LocalizationID));
        EditorGUILayout.PropertyField(_lockRotation, "label.transform_lock.lock_rotation".G(LocalizationID));
        EditorGUILayout.PropertyField(_lockScale, "label.transform_lock.lock_scale".G(LocalizationID));
        EditorGUILayout.PropertyField(_isLockPosition, "label.transform_lock.is_lock_position".G(LocalizationID));
        EditorGUILayout.PropertyField(_isLockRotation, "label.transform_lock.is_lock_rotation".G(LocalizationID));
        EditorGUILayout.PropertyField(_isLockScale, "label.transform_lock.is_lock_scale".G(LocalizationID));
        EditorGUILayout.HelpBox("label.transform_lock.warning".L(LocalizationID), MessageType.Warning, true);
    }
}