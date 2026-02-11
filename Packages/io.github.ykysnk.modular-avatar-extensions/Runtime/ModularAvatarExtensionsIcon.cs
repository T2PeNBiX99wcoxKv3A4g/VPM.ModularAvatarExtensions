using UnityEditor;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    public class ModularAvatarExtensionsIcon : ScriptableObject
    {
        public string iconNameWithLastTime = "";

        public static ModularAvatarExtensionsIcon GetOrCreate(string path)
        {
            var asset = AssetDatabase.LoadAssetAtPath<ModularAvatarExtensionsIcon>(path);
            if (asset != null) return asset;
            asset = CreateInstance<ModularAvatarExtensionsIcon>();
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            return asset;
        }

        public void Save() => EditorUtility.SetDirty(this);
    }
}