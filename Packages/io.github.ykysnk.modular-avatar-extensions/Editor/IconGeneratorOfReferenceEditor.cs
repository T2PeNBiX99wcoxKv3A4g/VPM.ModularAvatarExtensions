using UnityEditor;
using UnityEngine.UIElements;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [CustomEditor(typeof(ModularAvatarExtensionsIconGeneratorOfReference))]
    [CanEditMultipleObjects]
    internal class IconGeneratorOfReferenceEditor : IconGeneratorBaseEditor
    {
        protected override void OnCreateInnerInspectorGUI(TemplateContainer container)
        {
        }
    }
}