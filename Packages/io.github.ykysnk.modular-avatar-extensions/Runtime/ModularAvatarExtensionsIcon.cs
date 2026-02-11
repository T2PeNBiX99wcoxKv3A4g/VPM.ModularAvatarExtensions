using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace io.github.ykysnk.ModularAvatarExtensions
{
    public class ModularAvatarExtensionsIcon : ScriptableObject
    {
        public string iconNameWithLastTime = "";

        public static ModularAvatarExtensionsIcon GetOrCreate(string path)
        {
            // Why the fuck this object be loaded in avatar runtime build?
#if UNITY_EDITOR
            var asset = AssetDatabase.LoadAssetAtPath<ModularAvatarExtensionsIcon>(path);
            if (asset != null) return asset;
            asset = CreateInstance<ModularAvatarExtensionsIcon>();
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            return asset;
#else
            throw new("Do not call this method in runtime.");
#endif
        }

#if UNITY_EDITOR
        public void Save() => EditorUtility.SetDirty(this);
#else
        public void Save() => throw new("Do not call this method in runtime.");
#endif
    }
}