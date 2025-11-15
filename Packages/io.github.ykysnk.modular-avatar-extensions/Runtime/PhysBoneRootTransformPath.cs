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
    public class PhysBoneRootTransformPath : RootTransformPathBase<VRCPhysBone>
    {
        public List<AvatarObjectReference>? ignoreTransformsReferences;
        public List<AvatarObjectReference>? colliderReferences;

        protected override bool CheckIsValid() => base.CheckIsValid() || ignoreTransformsReferences is { Count: > 0 } ||
                                                  colliderReferences is { Count: > 0 };

        protected override void SetPath()
        {
            base.SetPath();
            if (component == null || Utils.IsInPrefab()) return;
            if (ignoreTransformsReferences is { Count: > 0 })
                component.ignoreTransforms = (from t in ignoreTransformsReferences
                    select t.Get(this)
                    into obj
                    where obj
                    select obj.transform).ToList();
            else
                component.ignoreTransforms = new();

            if (colliderReferences is { Count: > 0 })
                component.colliders = (from t in colliderReferences
                    select t.Get(this)
                    into obj
                    where obj
                    select obj.GetComponent<VRCPhysBoneColliderBase>()).ToList();
            else
                component.colliders = new();
        }
    }
}