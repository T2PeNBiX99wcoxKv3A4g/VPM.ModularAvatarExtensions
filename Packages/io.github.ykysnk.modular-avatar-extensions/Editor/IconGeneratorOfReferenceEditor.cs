using System.Linq;
using System.Reflection;
using io.github.ykysnk.ModularAvatarExtensions.ExtraData;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [CustomEditor(typeof(ModularAvatarExtensionsIconGeneratorOfReference))]
    [CanEditMultipleObjects]
    internal class IconGeneratorOfReferenceEditor : IconGeneratorBaseEditor
    {
        protected override void OnCreateInnerInspectorGUI(TemplateContainer container)
        {
            var referenceExtraDatasAdd = container.Q<DropdownField>("referenceExtraDatasAdd");
            var type = typeof(IExtraData);
            var types = type.Assembly.GetTypes().Where(t =>
                t is { IsClass: true, IsAbstract: false, IsInterface: false } && type.IsAssignableFrom(t));
            var nameWithTypes = types.ToDictionary(x =>
            {
                var attribute = x.GetCustomAttribute<AddComponentMenu>();
                return attribute == null ? x.FullName : attribute.componentMenu.Split('/').Last();
            });
            referenceExtraDatasAdd.choices = nameWithTypes.Keys.ToList();
            referenceExtraDatasAdd.formatSelectedValueCallback +=
                _ => "label.icon_generator_of_reference.add_reference_extra_data".S();
            referenceExtraDatasAdd.RegisterValueChangedCallback(evt =>
            {
                if (!nameWithTypes.TryGetValue(evt.newValue, out var addType)) return;
                foreach (var component in targets)
                {
                    if (component is not ModularAvatarExtensionsIconGeneratorOfReference iconGeneratorOfReference)
                        continue;
                    var obj = iconGeneratorOfReference.gameObject;
                    Undo.AddComponent(obj, addType);
                    EditorUtility.SetDirty(obj);
                }

                referenceExtraDatasAdd.value = "";
            });
        }
    }
}