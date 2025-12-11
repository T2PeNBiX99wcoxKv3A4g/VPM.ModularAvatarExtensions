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
            Renderer = TryGetRenderer(gameObject);
        }

        private static Renderer? TryGetRenderer(GameObject obj)
        {
            if (obj.TryGetComponent<Renderer>(out var renderer))
                return renderer;
            var renderers = obj.GetComponentsInChildren<Renderer>(true);
            return renderers.Length > 0 ? renderers[0] : null;
        }

        private static Mesh? GetMesh(GameObject obj)
        {
            if (obj.TryGetComponent<SkinnedMeshRenderer>(out var skinnedMeshRenderer))
                return skinnedMeshRenderer.sharedMesh;
            if (obj.TryGetComponent<MeshFilter>(out var meshFilter))
                return meshFilter.sharedMesh;
            var skinnedMeshRenderers = obj.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            if (skinnedMeshRenderers.Length > 0) return skinnedMeshRenderers[0].sharedMesh;
            var meshFilters = obj.GetComponentsInChildren<MeshFilter>(true);
            return meshFilters.Length > 0 ? meshFilters[0].sharedMesh : null;
        }

        private static Material[] GetMaterials(GameObject obj)
        {
            if (obj.TryGetComponent<SkinnedMeshRenderer>(out var skinnedMeshRenderer))
                return skinnedMeshRenderer.sharedMaterials;
            if (obj.TryGetComponent<MeshRenderer>(out var meshRenderer))
                return meshRenderer.materials;
            var skinnedMeshRenderers = obj.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            if (skinnedMeshRenderers.Length > 0) return skinnedMeshRenderers[0].sharedMaterials;
            var meshRenderers = obj.GetComponentsInChildren<MeshRenderer>(true);
            return meshRenderers.Length > 0
                ? meshRenderers[0].materials
                : new Material[]
                {
                };
        }
    }
}