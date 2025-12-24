using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using io.github.ykysnk.utils;
using io.github.ykysnk.utils.Extensions;
using io.github.ykysnk.utils.NonUdon;
using JetBrains.Annotations;
using nadena.dev.modular_avatar.core;
using UnityEngine;
#if UNITY_EDITOR // Lets me sleep plz
using UnityEditor;
using UnityEditor.Presets;
using UnityDirectory = UnityEngine.Windows.Directory;
using UnityFile = UnityEngine.Windows.File;
#endif

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [PublicAPI]
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public abstract class ModularAvatarExtensionsIconGeneratorBase : AvatarMaexComponent
    {
        public const string FolderPath = "Assets/ModularAvatarExtensionsIconGenerator";
        protected const int TargetLayer = 21;
        protected const int CaptureWidthAndHeight = 2048;
        protected const int ScaleWidthAndHeight = 256;

        protected static bool IsQuitting;

        [SerializeField] protected ModularAvatarMenuItem? modularAvatarMenuItem;
        [SerializeField] protected Texture2D? iconTexture;
        [SerializeField] protected List<ShapeKeyData> shapeKeyDatas = new();
        [SerializeField] protected List<GameObject> objects = new();
        [SerializeField] protected string objectsHash = "";
        [SerializeField] protected string iconName = "";
        [SerializeField] protected bool shouldGenerateIcon;
        [SerializeField] protected int scaleWidth = ScaleWidthAndHeight;
        [SerializeField] protected int scaleHeight = ScaleWidthAndHeight;

#if UNITY_EDITOR
        static ModularAvatarExtensionsIconGeneratorBase()
        {
            EditorApplication.wantsToQuit -= WantToQuit;
            EditorApplication.wantsToQuit += WantToQuit;
        }
#endif

        public Texture2D? IconTexture => iconTexture;

#if UNITY_EDITOR
        public Preset? Preset
        {
            get => preset;
            set => preset = value;
        }
#endif

        public string IconName => iconName;

#if UNITY_EDITOR
        private void OnEnable() => StartCoroutine(CheckLoop());
#endif

        protected override void OnDestroy() => RemoveUnusedIcon();

#if UNITY_EDITOR
        private IEnumerator CheckLoop()
        {
            while (enabled && gameObject.activeSelf)
            {
                if (gameObject.IsSceneObject())
                    Check();
                yield return new WaitForSeconds(2f);
            }
        }
#endif

        private void OnProjectChanged()
        {
#if UNITY_EDITOR
            if (!gameObject.IsSceneObject() || Utils.IsPlaying) return;
#endif
            OnChange();
            Check();
        }

        protected virtual void Check()
        {
#if UNITY_EDITOR
            if (shouldGenerateIcon)
            {
                shouldGenerateIcon = false;
                GenerateIcon();
            }
            else
                shouldGenerateIcon = ShouldGenerateIcon();

            if (modularAvatarMenuItem == null || iconTexture == null ||
                iconTexture == modularAvatarMenuItem.PortableControl.Icon) return;

            Undo.RecordObject(modularAvatarMenuItem, "Change Icon");
            modularAvatarMenuItem.PortableControl.Icon = iconTexture;
            EditorUtility.SetDirty(modularAvatarMenuItem);
#endif
        }

        private static bool WantToQuit()
        {
            IsQuitting = true;
            return true;
        }


        protected override void OnChange()
        {
#if UNITY_EDITOR
            modularAvatarMenuItem = GetComponent<ModularAvatarMenuItem>();
            if (!gameObject.activeSelf || !gameObject.IsSceneObject()) return;
            objects = GetAllObjects().Distinct().ToList();
            objectsHash = HashUtils.ComputeHash(string.Join("|", objects.Select(o => o.FullName())),
                HashUtils.HashType.SHA1);
            shapeKeyDatas = GetAllShapeKeyDatas().Distinct().ToList();
            iconTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(FolderPath, $"{iconName}.png"));
            iconImporter = AssetImporter.GetAtPath(Path.Combine(FolderPath, $"{iconName}.png")) as TextureImporter;
            shouldGenerateIcon = ShouldGenerateIcon();

            var guid = PlayerPrefs.GetString("ModularAvatarExtensionsIconGeneratorPresetGUID", "");
            preset = AssetDatabase.LoadAssetAtPath<Preset>(AssetDatabase.GUIDToAssetPath(guid));

            EditorApplication.projectChanged -= OnProjectChanged;
            if (!gameObject.IsSceneObject()) return;
            EditorApplication.projectChanged += OnProjectChanged;
#endif
        }

        protected bool ShouldGenerateIcon()
        {
#if UNITY_EDITOR
            var meshDatas = objects.Select(obj => new MeshData(obj)).ToArray();
            var newIconName = GetIconName(meshDatas);
            return (iconName != newIconName || meshDatas.Length > 0 && iconTexture == null) && gameObject.IsSceneObject();
#else
            return false;
#endif
        }

        public void ForceGenerateIcon()
        {
            OnChange();
            shouldGenerateIcon = true;
            Check();
        }

        protected void GenerateIcon()
        {
#if UNITY_EDITOR
            if (!UnityDirectory.Exists(FolderPath)) UnityDirectory.CreateDirectory(FolderPath);
            var shapeKeyValues = shapeKeyDatas.GroupBy(x => x.gameObject).ToDictionary(x => x.Key,
                x => x.Select(y => new ShapeKeyValue(x.Key, y.shapeKeyName, y.value)).ToList());
            var meshDatas = objects.Select(obj => new MeshData(obj))
                .ToArray();
            var oldIconName = iconName;
            var newIconName = GetIconName(meshDatas);
            var newIconPath = Path.Combine(FolderPath, newIconName);
            if (oldIconName != newIconName) Task.Run(RemoveUnusedIcon);
            var bytes = SaveMeshAsPng(meshDatas, shapeKeyValues, scaleWidth, scaleHeight);
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
            EditorUtility.SetDirty(this);

            if (modularAvatarMenuItem == null || iconTexture == null ||
                iconTexture == modularAvatarMenuItem.PortableControl.Icon) return;
            Undo.RecordObject(modularAvatarMenuItem, "Change Icon");
            modularAvatarMenuItem.PortableControl.Icon = iconTexture;
            EditorUtility.SetDirty(modularAvatarMenuItem);
#endif
        }

        protected static void ChangeLayer(GameObject obj, int layer)
        {
            obj.layer = layer;
            foreach (Transform child in obj.transform) ChangeLayer(child.gameObject, layer);
        }

        // Refs: https://github.com/weasel-club/OneClickInventory/blob/main/Editor/Util/IconUtil.cs#L24
        protected static byte[]? SaveMeshAsPng(MeshData[] meshDatas,
            Dictionary<GameObject, List<ShapeKeyValue>> shapeKeyDatas, int scaleWidth, int scaleHeight)
        {
#if UNITY_EDITOR
            var tempObj = new GameObject("TempObj")
            {
                transform =
                {
                    position = Vector3.zero
                }
            };

            var cloneShapeKeyDatas = new Dictionary<GameObject, List<ShapeKeyValue>>();

            foreach (var meshData in meshDatas)
            {
                var clone = Instantiate(meshData.GameObject, tempObj.transform, true);
                clone.SetActive(true);
                if (shapeKeyDatas.TryGetValue(meshData.GameObject, out var shapeKeyValues))
                    cloneShapeKeyDatas.TryAdd(clone.gameObject, shapeKeyValues);
            }

            var renderer = tempObj.GetComponentsInChildren<Renderer>().ToList();
            var newMeshDatas = renderer.Select(r => new MeshData(r.gameObject)).ToList();

            ChangeLayer(tempObj, TargetLayer);

            foreach (var newMeshData in newMeshDatas)
            {
                if (newMeshData.Renderer is not SkinnedMeshRenderer skinnedMeshRenderer) continue;
                if (!cloneShapeKeyDatas.TryGetValue(skinnedMeshRenderer.gameObject, out var shapeKeyValues)) continue;
                foreach (var shapeKeyValue in shapeKeyValues)
                {
                    if (shapeKeyValue is not { ShapeKeyIndex: > -1, Value: > 0 }) continue;
                    skinnedMeshRenderer.SetBlendShapeWeight(shapeKeyValue.ShapeKeyIndex, shapeKeyValue.Value);
                }
            }

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
            RenderTexture.active = null;
            cam.targetTexture = null;
            DestroyImmediate(rt);
            DestroyImmediate(camObj);
            DestroyImmediate(tempObj);

            var trimTex = tex.TrimTransparentGPU();
            var scaleTex = trimTex.ScaleGPU(scaleWidth, scaleHeight);
            var bytes = scaleTex.EncodeToPNG();
            return bytes;
#else
            return null;
#endif
        }

        protected void RemoveUnusedIcon()
        {
#if UNITY_EDITOR
            if (IsQuitting || string.IsNullOrEmpty(iconName)) return;
            var allIconGenerator = Resources.FindObjectsOfTypeAll<ModularAvatarExtensionsIconGeneratorBase>();
            if (allIconGenerator.Any(x => x != this && x.iconName == iconName)) return;
            var iconPath = Path.Combine(FolderPath, $"{iconName}.png");
            if (!UnityFile.Exists(iconPath)) return;
            UnityFile.Delete(iconPath);
            AssetDatabase.Refresh();
#endif
        }

        protected abstract List<GameObject> GetAllObjects();

        protected abstract List<ShapeKeyData> GetAllShapeKeyDatas();

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        public static void RemoveAllUnusedIconLoader()
        {
            EditorApplication.projectChanged -= RemoveAllUnusedIcon;
            EditorApplication.projectChanged += RemoveAllUnusedIcon;
        }

        public static void RemoveAllUnusedIcon()
        {
            foreach (var path in Directory.GetFiles(FolderPath, "*.png"))
            {
                var iconName = Path.GetFileNameWithoutExtension(path);
                var allIconGenerator = Resources.FindObjectsOfTypeAll<ModularAvatarExtensionsIconGeneratorBase>();
                if (allIconGenerator.Any(x => x.iconName == iconName)) continue;
                Utils.Log(nameof(RemoveAllUnusedIcon), $"Removing unused icon: {iconName}");
                UnityFile.Delete(path);
            }
        }
#endif

#if UNITY_EDITOR
        protected static string GetMaterialsSha256(MeshData meshData)
        {
            var guidWithTimes = meshData.Materials.Select(m =>
            {
                var path = AssetDatabase.GetAssetPath(m);
                var guid = AssetDatabase.AssetPathToGUID(path);
                var lastWriteTime = !string.IsNullOrEmpty(path) ? File.GetLastWriteTime(path) : DateTime.MinValue;
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

        protected string GetIconName(MeshData[] meshData2)
        {
            var iconNames = meshData2.Select(meshData =>
            {
                if (meshData.Mesh == null) return "";
                var fbxAssetGuid = GetFBXAssetSha256(meshData);
                var matsSha256 = GetMaterialsSha256(meshData);
                return $"{fbxAssetGuid}.{meshData.Mesh?.name}.{matsSha256}";
            });
            return HashUtils.ComputeHash(string.Join("|", iconNames) + string.Join("|", shapeKeyDatas),
                HashUtils.HashType.SHA1);
        }
#endif
#if UNITY_EDITOR
        [SerializeField] protected TextureImporter? iconImporter;
        [SerializeField] protected Preset? preset;
#endif
    }
}