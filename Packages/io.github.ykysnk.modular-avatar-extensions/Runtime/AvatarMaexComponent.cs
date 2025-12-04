using JetBrains.Annotations;
using nadena.dev.ndmf;
using nadena.dev.ndmf.runtime;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [PublicAPI]
    [DefaultExecutionOrder(-9999)]
    public abstract class AvatarMaexComponent : MonoBehaviour, INDMFEditorOnly
    {
        public virtual bool DontDestroyOnBuild { get; set; }

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

        public virtual void OnInspectorGUI()
        {
        }
    }
}