using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    public readonly struct MeshData
    {
        public readonly GameObject GameObject;
        public readonly Mesh? Mesh;
        public readonly Material[] Materials;
        public readonly Renderer? Renderer;

        public MeshData(GameObject gameObject)
        {
            GameObject = gameObject;
            Mesh = GetMesh(gameObject);
            Materials = GetMaterials(gameObject);
            Renderer = gameObject.GetComponent<Renderer>();
        }

        private static Mesh? GetMesh(GameObject obj)
        {
            if (obj.TryGetComponent<SkinnedMeshRenderer>(out var skinnedMeshRenderer))
                return skinnedMeshRenderer.sharedMesh;
            return obj.TryGetComponent<MeshFilter>(out var meshFilter) ? meshFilter.sharedMesh : null;
        }

        private static Material[] GetMaterials(GameObject obj)
        {
            if (obj.TryGetComponent<SkinnedMeshRenderer>(out var skinnedMeshRenderer))
                return skinnedMeshRenderer.sharedMaterials;
            return obj.TryGetComponent<MeshRenderer>(out var meshRenderer)
                ? meshRenderer.materials
                : new Material[]
                {
                };
        }
    }
}