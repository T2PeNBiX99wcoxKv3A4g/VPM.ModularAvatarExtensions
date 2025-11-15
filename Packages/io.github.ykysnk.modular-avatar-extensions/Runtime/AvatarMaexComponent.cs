using JetBrains.Annotations;
using nadena.dev.ndmf.runtime;
using UnityEngine;
using VRC.SDKBase;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [PublicAPI]
    public abstract class AvatarMaexComponent : MonoBehaviour, IEditorOnly
    {
        public virtual void OnInspectorGUI()
        {
            if (RuntimeUtil.IsPlaying) return;
            OnChange(false);
        }

        protected virtual void OnValidate()
        {
            if (RuntimeUtil.IsPlaying) return;
            OnChange(true);
        }

        protected virtual void OnDestroy()
        {
        }

        protected virtual void OnChange(bool isValidate)
        {
        }
    }
}