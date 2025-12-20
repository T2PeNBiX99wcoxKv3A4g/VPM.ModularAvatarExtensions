using System.Collections.Generic;
using UnityEngine.UIElements;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor.UIElements
{
    public class MaterialField : PopupField<KeyValuePair<int, string>>
    {
        public new class UxmlFactory : UxmlFactory<MaterialField, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
        }
    }
}