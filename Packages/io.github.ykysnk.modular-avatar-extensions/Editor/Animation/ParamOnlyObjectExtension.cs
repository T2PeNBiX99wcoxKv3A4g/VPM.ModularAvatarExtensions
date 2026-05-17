using System.Collections.Generic;
using JetBrains.Annotations;
using nadena.dev.ndmf;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor.Animation
{
    internal class ParamOnlyObjectExtension : IExtensionContext
    {
        private Retained _retained = null!;

        private Dictionary<GameObject, bool> GameObjectProps => _retained.GameObjectProps;

        public void OnActivate(BuildContext context)
        {
            _retained = context.GetState<Retained>();
        }

        public void OnDeactivate(BuildContext context)
        {
        }

        [PublicAPI]
        public void AddGameObject(GameObject gameObject, bool isActive)
        {
            GameObjectProps[gameObject] = isActive;
        }

        [PublicAPI]
        public class Retained
        {
            public readonly Dictionary<GameObject, bool> GameObjectProps = new();
        }
    }
}