using System.Linq;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    public class MeshData
    {
        public readonly GameObject GameObject;
        public readonly Material[] Materials;
        public readonly Mesh? Mesh;
        public readonly Renderer? Renderer;

        public MeshData(GameObject gameObject)
        {
            GameObject = gameObject;
            Mesh = GetMesh(gameObject);
            Materials = GetMaterials(gameObject);
            Renderer = GetRenderer(gameObject);
        }

        private static Renderer? GetRenderer(GameObject obj)
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
            if (obj.TryGetComponent<Renderer>(out var renderer))
                return renderer.sharedMaterials;

            var renderers = obj.GetComponentsInChildren<Renderer>(true);
            return renderers.FirstOrDefault()?.sharedMaterials ?? new Material[]
            {
            };
        }
    }
}