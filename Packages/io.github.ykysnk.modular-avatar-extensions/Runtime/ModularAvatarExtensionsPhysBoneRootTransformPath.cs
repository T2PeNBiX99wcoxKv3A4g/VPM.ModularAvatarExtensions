#if MAEX_VRCSDK3_BASE
using System.Collections.Generic;
using System.Linq;
using io.github.ykysnk.utils;
using nadena.dev.modular_avatar.core;
using UnityEngine;
using VRC.Dynamics;
using VRC.SDK3.Dynamics.PhysBone.Components;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [AddComponentMenu("Modular Avatar EX/MAEX Phys Bone Root Transform Path")]
    public class ModularAvatarExtensionsPhysBoneRootTransformPath : RootTransformPathBase<VRCPhysBone>
    {
        public List<AvatarObjectReference>? ignoreTransformsReferences;
        public List<AvatarObjectReference>? colliderReferences;
        public bool setIgnoreTransforms;
        public bool setColliders;

        protected override bool CheckIsValid() => base.CheckIsValid() ||
                                                  ignoreTransformsReferences is { Count: > 0 } && setIgnoreTransforms ||
                                                  colliderReferences is { Count: > 0 } && setColliders;

        protected override void SetPath()
        {
            base.SetPath();
            if (component == null || Utils.IsInPrefab()) return;
            if (setIgnoreTransforms)
            {
                if (ignoreTransformsReferences is { Count: > 0 })
                    component.ignoreTransforms = (from t in ignoreTransformsReferences
                        select t.Get(this)
                        into obj
                        where obj
                        select obj.transform).ToList();
                else if (component.ignoreTransforms is { Count: > 0 })
                    component.ignoreTransforms = new();
            }

            if (!setColliders)
                return;
            if (colliderReferences is { Count: > 0 })
                component.colliders = (from t in colliderReferences
                    select t.Get(this)
                    into obj
                    where obj
                    select obj.GetComponent<VRCPhysBoneColliderBase>()).ToList();
            else if (component.colliders is { Count: > 0 })
                component.colliders = new();
        }
    }
}
#endif