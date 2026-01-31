using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor.UIElements
{
    public delegate bool ValidatedComponent(Component component);

    public class ValidatedObjectField : VisualElement
    {
        private readonly HelpBox _errorBox;

        private readonly ObjectField _objectField;
        private readonly List<ValidatedComponent> _validators = new();

        public ValidatedObjectField()
        {
            AddToClassList("validated-object-field");

            _objectField = new();
            _objectField.RegisterValueChangedCallback(OnValueChanged);
            _objectField.schedule.Execute(Validate).Every(1000);

            _errorBox = new()
            {
                messageType = HelpBoxMessageType.Error
            };

            Add(_objectField);
            Add(_errorBox);
        }

        private bool IsValid => _objectField.value is not Component ||
                                !_validators.Exists(v => !v((Component)_objectField.value));

        [PublicAPI]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public string bindingPath
        {
            get => _objectField.bindingPath;
            set => _objectField.bindingPath = value;
        }

        [PublicAPI]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public bool required { get; set; } = true;

        [PublicAPI]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public bool autoHide { get; set; } = true;

        [PublicAPI]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public Type objectType
        {
            get => _objectField.objectType;
            set => _objectField.objectType = value;
        }

        [PublicAPI]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public string errorMessage
        {
            get => _errorBox.text;
            set => _errorBox.text = value;
        }

        [PublicAPI]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public string label
        {
            get => _objectField.label;
            set => _objectField.label = value;
        }

        [PublicAPI]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public bool allowSceneObjects
        {
            get => _objectField.allowSceneObjects;
            set => _objectField.allowSceneObjects = value;
        }

        [PublicAPI]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public Object value
        {
            get => _objectField.value;
            set => _objectField.value = value;
        }

        private void OnValueChanged(ChangeEvent<Object> evt) => Validate();

        [PublicAPI]
        public void Validate()
        {
            if (!IsValid || required && _objectField.value == null)
            {
                _errorBox.style.display = DisplayStyle.Flex;
                AddToClassList("error");
            }
            else
            {
                _errorBox.style.display = DisplayStyle.None;
                RemoveFromClassList("error");
            }
        }

        [PublicAPI]
        public void AddValidator(ValidatedComponent validator)
        {
            _validators.Add(validator);
        }

        [PublicAPI]
        public void AutoHideIfSameGameObject(GameObject owner)
        {
            if (!autoHide || _objectField.value is not Component comp) return;
            style.display = IsValid && comp.gameObject == owner && owner.GetComponents(_objectField.objectType)
                .Count(c =>
                {
                    var pass = true;

                    foreach (var variable in _validators)
                    {
                        pass = variable(c);
                        if (!pass) break;
                    }

                    return pass;
                }) < 2
                ? DisplayStyle.None
                : DisplayStyle.Flex;
        }

        public new class UxmlFactory : UxmlFactory<ValidatedObjectField, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private readonly UxmlBoolAttributeDescription _allowSceneObjects = new()
            {
                name = "allow-scene-objects",
                defaultValue = true
            };

            private readonly UxmlBoolAttributeDescription _autoHideAttr = new()
            {
                name = "auto-hide",
                defaultValue = true
            };

            private readonly UxmlStringAttributeDescription _bindingPathAttr = new()
            {
                name = "binding-path"
            };

            private readonly UxmlStringAttributeDescription _errorMessageAttr = new()
            {
                name = "error-message",
                defaultValue = "Error message"
            };

            private readonly UxmlStringAttributeDescription _label = new()
            {
                name = "label",
                defaultValue = "Validated Object Field"
            };

            private readonly UxmlTypeAttributeDescription<Object> _objectType = new()
            {
                name = "type",
                defaultValue = typeof(Object)
            };

            private readonly UxmlBoolAttributeDescription _requiredAttr = new()
            {
                name = "required",
                defaultValue = true
            };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                var field = (ValidatedObjectField)ve;

                field.bindingPath = _bindingPathAttr.GetValueFromBag(bag, cc);
                field.label = _label.GetValueFromBag(bag, cc);
                field.allowSceneObjects = _allowSceneObjects.GetValueFromBag(bag, cc);
                field.objectType = _objectType.GetValueFromBag(bag, cc);
                field.required = _requiredAttr.GetValueFromBag(bag, cc);
                field.errorMessage = _errorMessageAttr.GetValueFromBag(bag, cc);
                field.autoHide = _autoHideAttr.GetValueFromBag(bag, cc);
            }
        }
    }
}