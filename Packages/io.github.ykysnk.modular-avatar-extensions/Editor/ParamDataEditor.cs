#if MAEX_VRCSDK3_BASE
using System;
using io.github.ykysnk.utils.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [CustomPropertyDrawer(typeof(ParamData))]
    internal class ParamDataEditor : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var uxml =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    AssetDatabase.GUIDToAssetPath("980f6f042e1ace545bdddb4d9f7238e8"));

            if (uxml == null) return BasicEditor.CreateUxmlImportErrorUI();

            var tree = uxml.CloneTree();
            InternalLocalizationExtensions.Helper.UILocalize(tree, false);
            tree.Bind(property.serializedObject);

            var paramField = new FloatField
            {
                style =
                {
                    display = DisplayStyle.None
                }
            };

            tree.Add(paramField);
            paramField.BindProperty(property.FindPropertyRelative("paramValue"));

            var floatField = tree.Q<FloatField>("paramFloatValue");
            floatField.RegisterValueChangedCallback(evt => paramField.value = evt.newValue);

            var intField = tree.Q<IntegerField>("paramIntValue");
            intField.RegisterValueChangedCallback(evt => paramField.value = evt.newValue);

            var boolField = tree.Q<Toggle>("paramBoolValue");
            boolField.RegisterValueChangedCallback(evt => paramField.value = evt.newValue ? 1f : 0f);

            var enumField = tree.Q<EnumField>("paramType");
            EditorApplication.delayCall += () => OnTypeChanged(true, enumField.value, enumField.value);
            enumField.RegisterValueChangedCallback(evt => OnTypeChanged(false, evt.previousValue, evt.newValue));
            return tree;

            void OnTypeChanged(bool isInit, Enum previousEnum, Enum newEnum)
            {
                floatField.style.display = DisplayStyle.None;
                intField.style.display = DisplayStyle.None;
                boolField.style.display = DisplayStyle.None;

                var newValue = previousEnum switch
                {
                    ParamData.Type.Float => floatField.value,
                    ParamData.Type.Int => intField.value,
                    ParamData.Type.Bool => boolField.value ? 1f : 0f,
                    _ => 0
                };

                if (isInit)
                {
                    floatField.value = paramField.value;
                    intField.value = (int)paramField.value;
                    boolField.value = paramField.value > 0;
                }

                switch (newEnum)
                {
                    case ParamData.Type.Float:
                        floatField.style.display = DisplayStyle.Flex;

                        if (!isInit)
                        {
                            floatField.value = newValue;
                            intField.value = 0;
                            boolField.value = false;
                        }

                        break;
                    case ParamData.Type.Int:
                        intField.style.display = DisplayStyle.Flex;

                        if (!isInit)
                        {
                            floatField.value = 0;
                            intField.value = (int)newValue;
                            boolField.value = false;
                        }

                        break;
                    case ParamData.Type.Bool:
                        boolField.style.display = DisplayStyle.Flex;

                        if (!isInit)
                        {
                            floatField.value = 0;
                            intField.value = 0;
                            boolField.value = newValue > 0;
                        }

                        break;
                }
            }
        }
    }
}
#endif