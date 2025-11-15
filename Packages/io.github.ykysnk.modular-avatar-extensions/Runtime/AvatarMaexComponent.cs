using JetBrains.Annotations;
using nadena.dev.ndmf.runtime;
using UnityEngine;
using VRC.SDKBase;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [PublicAPI]
    [DefaultExecutionOrder(-9999)]
    public abstract class AvatarMaexComponent : MonoBehaviour, IEditorOnly
    {
        protected virtual void OnDestroy()
        {
        }

        protected virtual void OnValidate()
        {
            if (RuntimeUtil.IsPlaying) return;
            OnChange();
        }

        protected virtual void OnChange()
        {
        }
    }
}