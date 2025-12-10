#if MAEX_D4RK_AVATAR_OPTIMIZER
using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [CustomEditor(typeof(ModularAvatarExtensionsD4RkAvatarOptimizerExclude))]
    [CanEditMultipleObjects]
    internal class D4RkAvatarOptimizerExcludeEditor : MaexEditor
    {
        protected override void OnInnerInspectorGUI()
        {
            EditorGUILayout.HelpBox("label.d4rk_avatar_optimizer_exclude.info".S(), MessageType.Info, true);
        }
    }
#endif
}