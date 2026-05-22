using System.Collections.Generic;
using io.github.ykysnk.utils.Extensions;
using JetBrains.Annotations;
using nadena.dev.ndmf;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor.Animation
{
    internal class ParamOnlyObjectExtension : IExtensionContext
    {
        private Retained _retained = null!;

        private Dictionary<ObjectSaverData, bool> GameObjectProps => _retained.GameObjectProps;

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
            if (gameObject == null) return;
            GameObjectProps[new(gameObject, gameObject.FullName() ?? gameObject.name)] = isActive;
        }

        [PublicAPI]
        public class Retained
        {
            public readonly Dictionary<ObjectSaverData, bool> GameObjectProps = new();
        }
    }
}