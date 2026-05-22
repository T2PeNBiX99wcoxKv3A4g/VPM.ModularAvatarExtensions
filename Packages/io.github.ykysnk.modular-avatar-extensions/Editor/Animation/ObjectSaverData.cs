using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor.Animation
{
    internal class ObjectSaverData
    {
        internal readonly GameObject GameObject;
        internal readonly string Path;

        internal ObjectSaverData(GameObject gameObject, string path)
        {
            GameObject = gameObject;
            Path = path;
        }
    }
}