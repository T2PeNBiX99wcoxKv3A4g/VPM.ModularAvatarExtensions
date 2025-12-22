using System;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [Serializable]
    [PublicAPI]
    public struct IconUsedPathData
    {
        public string fullName;
        public string fullPath;
        public GameObject? gameObject;

        public IconUsedPathData(string fullName, GameObject? gameObject)
        {
            this.fullName = fullName;
            fullPath = AssetDatabase.GetAssetPath(gameObject);
            this.gameObject = gameObject;
        }
    }
}