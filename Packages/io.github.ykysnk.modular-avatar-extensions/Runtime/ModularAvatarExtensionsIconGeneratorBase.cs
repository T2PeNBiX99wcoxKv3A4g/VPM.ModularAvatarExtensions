using System.Collections.Generic;
using System.IO;
using System.Linq;
using io.github.ykysnk.utils;
using io.github.ykysnk.utils.Extensions;
using io.github.ykysnk.utils.NonUdon;
using JetBrains.Annotations;
using nadena.dev.modular_avatar.core;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;
using Directory = UnityEngine.Windows.Directory;
using UnityFile = UnityEngine.Windows.File;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [PublicAPI]
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public abstract class ModularAvatarExtensionsIconGeneratorBase : AvatarMaexComponent
    {
        protected const string FolderPath = "Assets/ModularAvatarExtensionsIconGenerator";
        protected const int TargetLayer = 21;
        protected const int CaptureWidthAndHeight = 2048;
        protected const int ScaleWidthAndHeight = 256;

        protected static bool IsQuitting;

        [SerializeField] protected ModularAvatarMenuItem? modularAvatarMenuItem;
        [SerializeField] protected Texture2D? iconTexture;
        [SerializeField] protected TextureImporter? iconImporter;
        [SerializeField] protected Preset? preset;
        [SerializeField] protected List<GameObject> objects = new();
        [SerializeField] protected string objectsHash = "";
        [SerializeField] protected string iconName = "";
        [SerializeField] protected bool shouldGenerateIcon;
        [SerializeField] protected int scaleWidth = ScaleWidthAndHeight;
        [SerializeField] protected int scaleHeight = ScaleWidthAndHeight;

        static ModularAvatarExtensionsIconGeneratorBase() => EditorApplication.wantsToQuit += WantToQuit;

        public Texture2D? IconTexture => iconTexture;

        public Preset? Preset
        {
            get => preset;
            set => preset = value;
        }

        protected virtual void LateUpdate()
        {
            if (shouldGenerateIcon)
            {
                shouldGenerateIcon = false;
                GenerateIcon();
            }
            else
                shouldGenerateIcon = ShouldGenerateIcon();
        }

        protected override void OnDestroy() => RemoveUnusedIcon();

        private static bool WantToQuit()
        {
            IsQuitting = true;
            return true;
        }

        protected override void OnChange()
        {
            modularAvatarMenuItem = GetComponent<ModularAvatarMenuItem>();
            objects = GetAllObjects();
            objectsHash = HashUtils.ComputeHash(string.Join("|", objects.Select(o => o.FullName())),
                HashUtils.HashType.SHA1);
            iconTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(FolderPath, $"{iconName}.png"));
            iconImporter = AssetImporter.GetAtPath(Path.Combine(FolderPath, $"{iconName}.png")) as TextureImporter;
            shouldGenerateIcon = ShouldGenerateIcon();

            var guid = PlayerPrefs.GetString("ModularAvatarExtensionsIconGeneratorPresetGUID", "");
            preset = AssetDatabase.LoadAssetAtPath<Preset>(AssetDatabase.GUIDToAssetPath(guid));
        }

        protected static string GetMaterialsSha256(MeshData meshData)
        {
            var guidWithTimes = meshData.Materials.Select(m =>
            {
                var path = AssetDatabase.GetAssetPath(m);
                var guid = AssetDatabase.AssetPathToGUID(path);
                var lastWriteTime = File.GetLastWriteTime(path);
                return $"{guid}.{lastWriteTime:yyyyMMddHHmmss}";
            });
            return HashUtils.ComputeHash(string.Join("|", guidWithTimes), HashUtils.HashType.SHA1);
        }

        protected static string GetAssetPathFromMesh(MeshData meshData)
        {
            var path = AssetDatabase.GetAssetPath(meshData.Mesh);
            var fbxPath = AssetDatabase.GetAssetPath(AssetDatabase.LoadMainAssetAtPath(path));
            return fbxPath;
        }

        protected static string? GetFBXAssetSha256(MeshData meshData)
        {
            if (meshData.Mesh == null) return null;
            var assetPath = GetAssetPathFromMesh(meshData);
            if (string.IsNullOrEmpty(assetPath)) return null;
            var assetGuid = AssetDatabase.AssetPathToGUID(assetPath);
            var lastWriteTime = File.GetLastWriteTime(assetPath);
            return $"{assetGuid}.{lastWriteTime:yyyyMMddHHmmss}";
        }

        protected static string GetIconName(MeshData[] meshData2)
        {
            var iconNames = meshData2.Select(meshData =>
            {
                if (meshData.Mesh == null) return "";
                var fbxAssetGuid = GetFBXAssetSha256(meshData);
                var matsSha256 = GetMaterialsSha256(meshData);
                return $"{fbxAssetGuid}.{meshData.Mesh?.name}.{matsSha256}";
            });
            return HashUtils.ComputeHash(string.Join("|", iconNames), HashUtils.HashType.SHA1);
        }

        protected bool ShouldGenerateIcon()
        {
            var meshDatas = objects.Select(obj => new MeshData(obj)).ToArray();
            var newIconName = GetIconName(meshDatas);
            return (iconName != newIconName || meshDatas.Length > 0 && iconTexture == null) &&
                   gameObject.scene.IsValid() && !Utils.IsInPrefab();
        }

        public void ForceGenerateIcon() => OnChange();

        protected void GenerateIcon()
        {
            if (!Directory.Exists(FolderPath)) Directory.CreateDirectory(FolderPath);
            var meshDatas = objects.Select(obj => new MeshData(obj)).ToArray();
            var oldIconName = iconName;
            var newIconName = GetIconName(meshDatas);
            var newIconPath = Path.Combine(FolderPath, newIconName);
            if (oldIconName != newIconName) RemoveUnusedIcon();
            var bytes = SaveMeshAsPng(meshDatas, scaleWidth, scaleHeight);
            if (bytes != null) UnityFile.WriteAllBytes($"{newIconPath}.png", bytes);
            iconName = newIconName;
            AssetDatabase.Refresh();
            iconTexture = AssetDatabase.LoadAssetAtPath<Texture2D>($"{newIconPath}.png");
            iconImporter = AssetImporter.GetAtPath($"{newIconPath}.png") as TextureImporter;
            if (iconImporter == null) return;
            iconImporter.alphaIsTransparency = true;
            iconImporter.alphaSource = TextureImporterAlphaSource.FromInput;
            preset?.ApplyTo(iconImporter);
            iconImporter.SaveAndReimport();

            if (modularAvatarMenuItem != null)
                modularAvatarMenuItem.PortableControl.Icon = iconTexture;
        }

        protected static void ChangeLayer(GameObject obj, int layer)
        {
            obj.layer = layer;
            foreach (Transform child in obj.transform) ChangeLayer(child.gameObject, layer);
        }

        // Refs: https://github.com/weasel-club/OneClickInventory/blob/main/Editor/Util/IconUtil.cs#L24
        protected static byte[]? SaveMeshAsPng(MeshData[] meshDatas, int scaleWidth, int scaleHeight)
        {
            var tempObj = new GameObject("TempObj")
            {
                transform =
                {
                    position = Vector3.zero
                }
            };

            var newMeshDatas = new List<MeshData>();

            foreach (var meshData in meshDatas)
            {
                var clone = Instantiate(meshData.GameObject, tempObj.transform, true);
                clone.SetActive(true);
                newMeshDatas.Add(new(clone));
            }

            ChangeLayer(tempObj, TargetLayer);

            var camObj = new GameObject("TempCam");
            var cam = camObj.AddComponent<Camera>();
            cam.clearFlags = CameraClearFlags.Nothing;
            cam.backgroundColor = Color.clear;
            cam.nearClipPlane = 0.00001f;
            cam.cullingMask = 1 << TargetLayer;

            var boundList = newMeshDatas.Select(data =>
            {
                if (data.Renderer is not SkinnedMeshRenderer skinnedMeshRenderer)
                    return data.Renderer?.bounds ?? data.Mesh?.bounds ?? new();
                skinnedMeshRenderer.updateWhenOffscreen = true;
                if (skinnedMeshRenderer.sharedMesh == null) return new();
                return new(skinnedMeshRenderer.bounds.center, skinnedMeshRenderer.bounds.size);
            }).ToArray();

            var totalBounds = boundList.Length > 0 ? boundList[0] : new();

            foreach (var bounds in boundList.Skip(1))
                totalBounds.Encapsulate(bounds);

            cam.transform.eulerAngles = new(0, -180, 0);

            var maxExtent = totalBounds.extents.magnitude;
            var minDistance = maxExtent / Mathf.Sin(Mathf.Deg2Rad * cam.fieldOfView / 2);
            var center = totalBounds.center;

            cam.transform.position = center + Vector3.forward * minDistance;

            var rt = new RenderTexture(CaptureWidthAndHeight, CaptureWidthAndHeight, 24);
            cam.targetTexture = rt;
            cam.Render();
            RenderTexture.active = rt;
            var tex = new Texture2D(cam.targetTexture.width, cam.targetTexture.height, TextureFormat.RGBA32, false);
            tex.ReadPixels(new(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
            tex.alphaIsTransparency = true;
            tex.Apply();

            var scaleTex = tex.ScaleGPU(scaleWidth, scaleHeight);
            var bytes = scaleTex.EncodeToPNG();
            RenderTexture.active = null;
            cam.targetTexture = null;
            DestroyImmediate(rt);
            DestroyImmediate(camObj);
            DestroyImmediate(tempObj);
            return bytes;
        }

        protected void RemoveUnusedIcon()
        {
            if (IsQuitting || string.IsNullOrEmpty(iconName)) return;
            var allIconGenerator = Resources.FindObjectsOfTypeAll<ModularAvatarExtensionsIconGeneratorBase>();
            if (allIconGenerator.Any(x => x != this && x.iconName == iconName)) return;
            var iconPath = Path.Combine(FolderPath, $"{iconName}.png");
            if (!UnityFile.Exists(iconPath)) return;
            UnityFile.Delete(iconPath);
            AssetDatabase.Refresh();
        }

        protected abstract List<GameObject> GetAllObjects();
    }
}