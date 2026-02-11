using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using io.github.ykysnk.utils;
using io.github.ykysnk.utils.Extensions;
using io.github.ykysnk.utils.NonUdon;
using io.github.ykysnk.utils.NonUdon.Extensions;
using JetBrains.Annotations;
using nadena.dev.modular_avatar.core;
using nadena.dev.ndmf.runtime;
using UnityEngine;
#if UNITY_EDITOR // Lets me sleep plz
using Progress = UnityEditor.Progress;
using UnityEditor;
using UnityEditor.Presets;
#endif

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [PublicAPI]
    [ExecuteInEditMode]
    public abstract class ModularAvatarExtensionsIconGeneratorBase : AvatarMaexComponent
    {
        public const string FolderPath = "Assets/ModularAvatarExtensionsIconGenerator";
        protected const int TargetLayer = 21;
        protected const int CaptureWidthAndHeight = 2048;
        protected const int ScaleWidthAndHeight = 256;

        [SerializeField] protected ModularAvatarMenuItem? modularAvatarMenuItem;
        [SerializeField] protected Texture2D? iconTexture;
        [SerializeField] protected List<ShapeKeyData> shapeKeyDatas = new();
        [SerializeField] protected List<GameObject>? objects = new();
        [SerializeField] protected string objectsHash = "";
        [SerializeField] protected string iconName = "";
        [SerializeField] protected int scaleWidth = ScaleWidthAndHeight;
        [SerializeField] protected int scaleHeight = ScaleWidthAndHeight;

        private bool _forceShouldGenerateIcon;
        private bool _shouldGenerateIcon;

        public Texture2D? IconTexture => iconTexture;

#if UNITY_EDITOR
        public Preset? Preset
        {
            get => preset;
            set => preset = value;
        }
#endif

        public string IconName => iconName;

        public bool IsFirst => this != null && GetComponents<ModularAvatarExtensionsIconGeneratorBase>().First() == this;

        public ModularAvatarExtensionsIconGeneratorBase? First =>
            this == null ? null : GetComponents<ModularAvatarExtensionsIconGeneratorBase>().First();

#if UNITY_EDITOR
        private async UniTask CheckLoop()
        {
            while (enabled && gameObject.activeSelf)
            {
                if (gameObject.IsSceneObject() && !Utils.IsPlaying)
                    await Check();
                await UniTask.Delay(2000, cancellationToken: _checkTokenSource?.Token ?? CancellationToken.None);
            }
        }
#endif

        protected virtual async UniTask Check()
        {
#if UNITY_EDITOR
            if (_shouldGenerateIcon || _forceShouldGenerateIcon)
            {
                _shouldGenerateIcon = false;
                _forceShouldGenerateIcon = false;
                await GenerateIcon();
            }
            else
                _shouldGenerateIcon = ShouldGenerateIcon();

            if (!IsFirst)
            {
                iconTexture = First?.iconTexture;
                iconName = First?.iconName ?? "";
                return;
            }

            if (modularAvatarMenuItem == null || iconTexture == null ||
                iconTexture == modularAvatarMenuItem.PortableControl.Icon) return;

            Undo.RecordObject(modularAvatarMenuItem, "Change Icon");
            modularAvatarMenuItem.PortableControl.Icon = iconTexture;
            EditorUtility.SetDirty(modularAvatarMenuItem);
#endif
        }

        protected override void OnChange()
        {
#if UNITY_EDITOR
            modularAvatarMenuItem = GetComponent<ModularAvatarMenuItem>();
            var guid = PlayerPrefs.GetString("ModularAvatarExtensionsIconGeneratorPresetGUID", "");
            preset = AssetDatabase.LoadAssetAtPath<Preset>(AssetDatabase.GUIDToAssetPath(guid));
            if (!gameObject.activeSelf || !gameObject.IsSceneObject() || !IsFirst) return;
            objects = GetAllObjectsFromAllGenerator().Distinct().OrderBy(x => x.name).ToList();
            objectsHash = ListHash(objects.Select(RuntimeUtil.AvatarRootPath));
            shapeKeyDatas = GetAllShapeKeyDatasFromAllGenerator().Distinct().OrderBy(x => x.shapeKeyName).ToList();
            iconTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(FolderPath, $"{iconName}.png"));
            iconImporter = AssetImporter.GetAtPath(Path.Combine(FolderPath, $"{iconName}.png")) as TextureImporter;
            _shouldGenerateIcon = ShouldGenerateIcon();
#endif
        }

        protected bool ShouldGenerateIcon()
        {
#if UNITY_EDITOR
            if (objects == null) return false;
            var meshDatas = objects.Select(obj => new MeshData(obj)).ToArray();
            var newIconName = GetIconName(meshDatas);
            var newIconNameWithLastTime = GetIconNameWithLastTime(meshDatas);
            var newIconPath = Path.Combine(FolderPath, newIconName);
            var asset = ModularAvatarExtensionsIcon.GetOrCreate($"{newIconPath}.asset");
            return (iconName != newIconName || asset.iconNameWithLastTime != newIconNameWithLastTime ||
                    meshDatas.Length > 0 && iconTexture == null) &&
                   gameObject.IsSceneObject() && IsFirst;
#else
            return false;
#endif
        }

        public static string ListHash<T>(IEnumerable<T> enumerable) =>
            HashUtils.ComputeHash(enumerable, HashUtils.HashType.SHA1);

        public static string StringHash(params string[] strings) =>
            HashUtils.ComputeHash(string.Join("|", strings), HashUtils.HashType.SHA1);

        public void ForceGenerateIcon()
        {
            if (!IsFirst) return;
            OnChange();
            _forceShouldGenerateIcon = true;
            Check().Forget();
        }

        public static void ForceGenerateAllIcons() => ForceGenerateAllIconsAsync(CancellationToken.None).Forget();

        public static async UniTask ForceGenerateAllIconsAsync() =>
            await ForceGenerateAllIconsAsync(CancellationToken.None);

        public static async UniTask ForceGenerateAllIconsAsync(CancellationToken token)
        {
#if UNITY_EDITOR
            var iconGenerators = FindObjectsOfType<ModularAvatarExtensionsIconGeneratorBase>(true);
            var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
            var progressId = Progress.Start(
                "Force Generate All Icons",
                "Force Generating all icons...",
                Progress.Options.Managed
            );

            Progress.RegisterCancelCallback(progressId, () =>
            {
                if (cts.IsCancellationRequested || EditorApplication.isCompiling || EditorApplication.isUpdating)
                    return false;
                Utils.Log(nameof(ModularAvatarExtensionsIconGeneratorBase), "Cancel requested by the user.");
                cts.Cancel();
                return true;
            });

            var result = await Try.Run(async () =>
            {
                for (var i = 0; i < iconGenerators.Length; i++)
                {
                    var iconGenerator = iconGenerators[i];
                    if (cts.IsCancellationRequested)
                        throw new OperationCanceledException(cts.Token);

                    Progress.Report(progressId, (float)i / iconGenerators.Length, $"Generating: {iconGenerator.name}");
                    iconGenerator.ForceGenerateIcon();
                    await UniTask.DelayFrame(10, cancellationToken: token);
                }

                Progress.Finish(progressId);
            });

            result.OnFailure(ex =>
            {
                if (ex is OperationCanceledException)
                {
                    Progress.Finish(progressId, Progress.Status.Canceled);
                    Utils.LogWarning(nameof(ModularAvatarExtensionsIconGeneratorBase), "Generate was canceled.");
                }
                else
                {
                    Progress.Finish(progressId, Progress.Status.Failed);
                    Utils.LogError(nameof(ModularAvatarExtensionsIconGeneratorBase),
                        $"Generate Error: {ex.Message}\n{ex.StackTrace}");
                }
            });
#endif
        }

        protected async UniTask GenerateIcon()
        {
#if UNITY_EDITOR
            if (!AssetDatabase.IsValidFolder(FolderPath)) Directory.CreateDirectory(FolderPath);
            if (objects == null) return;

            var progressId = Progress.Start(
                "Generate Icon",
                "Generating icon...",
                Progress.Options.Managed | Progress.Options.Indefinite
            );

            var result = await Try.Run(async () =>
            {
                var shapeKeyValues = shapeKeyDatas.GroupBy(x => x.gameObject)
                    .ToDictionary(x => x.Key,
                        x => x.Select(y => new ShapeKeyValue(x.Key, y.shapeKeyName, y.value)).ToList());
                var meshDatas = objects.Select(obj => new MeshData(obj))
                    .ToArray();
                var newIconName = GetIconName(meshDatas);
                var newIconNameWithLastTime = GetIconNameWithLastTime(meshDatas);
                var newIconPath = Path.Combine(FolderPath, newIconName);
                Progress.Report(progressId, 0, $"Generating: {newIconName}");
                var bytes = SaveMeshAsPng(meshDatas, shapeKeyValues, scaleWidth, scaleHeight);
                if (bytes != null) await File.WriteAllBytesAsync($"{newIconPath}.png", bytes);
                var asset = ModularAvatarExtensionsIcon.GetOrCreate($"{newIconPath}.asset");
                iconName = newIconName;
                asset.iconNameWithLastTime = newIconNameWithLastTime;
                asset.Save();
                AssetDatabase.Refresh();
                iconTexture = AssetDatabase.LoadAssetAtPath<Texture2D>($"{newIconPath}.png");
                iconImporter = AssetImporter.GetAtPath($"{newIconPath}.png") as TextureImporter;
                if (iconImporter == null) return;
                iconImporter.alphaIsTransparency = true;
                iconImporter.alphaSource = TextureImporterAlphaSource.FromInput;
                preset?.ApplyTo(iconImporter);
                iconImporter.SaveAndReimport();
                if (this != null)
                    EditorUtility.SetDirty(this);
                Progress.Finish(progressId);
                await UniTask.DelayFrame(10);

                if (modularAvatarMenuItem == null || iconTexture == null ||
                    iconTexture == modularAvatarMenuItem.PortableControl.Icon) return;
                Undo.RecordObject(modularAvatarMenuItem, "Change Icon");
                modularAvatarMenuItem.PortableControl.Icon = iconTexture;
                EditorUtility.SetDirty(modularAvatarMenuItem);
            });

            result.OnFailure(ex =>
            {
                Progress.Finish(progressId, Progress.Status.Failed);
                Utils.LogError(nameof(ModularAvatarExtensionsIconGeneratorBase),
                    $"Generate Icon Error: {ex.Message}\n{ex.StackTrace}");
            });
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

        protected abstract List<GameObject> GetAllObjects();

        protected abstract List<ShapeKeyData> GetAllShapeKeyDatas();

        protected IEnumerable<GameObject> GetAllObjectsFromAllGenerator() =>
            GetComponents<ModularAvatarExtensionsIconGeneratorBase>().SelectMany(x => x.GetAllObjects());

        protected IEnumerable<ShapeKeyData> GetAllShapeKeyDatasFromAllGenerator() =>
            GetComponents<ModularAvatarExtensionsIconGeneratorBase>().SelectMany(x => x.GetAllShapeKeyDatas());

#if UNITY_EDITOR
        private CancellationTokenSource? _checkTokenSource;

        private void OnEnable()
        {
            _checkTokenSource = new();
            CheckLoop().Forget();
        }

        private void OnDisable() => _checkTokenSource?.Cancel();

        protected override void OnDestroy() => _checkTokenSource?.Cancel();
#endif

#if UNITY_EDITOR
        public static async UniTask ApplyPresetToAllIconsAsync(CancellationToken token)
        {
            var guid = PlayerPrefs.GetString("ModularAvatarExtensionsIconGeneratorPresetGUID", "");
            var preset = AssetDatabase.LoadAssetAtPath<Preset>(AssetDatabase.GUIDToAssetPath(guid));
            if (preset == null) return;
            if (!AssetDatabase.IsValidFolder(FolderPath)) Directory.CreateDirectory(FolderPath);
            var paths = Directory.GetFiles(FolderPath, "*.png");
            var total = paths.Length;
            var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
            var progressId = Progress.Start("Apply Preset To All Icon", "Applying preset to icons...",
                Progress.Options.Managed);

            Progress.RegisterCancelCallback(progressId, () =>
            {
                if (cts.IsCancellationRequested || EditorApplication.isCompiling || EditorApplication.isUpdating)
                    return false;
                Utils.Log(nameof(ModularAvatarExtensionsIconGeneratorBase), "Cancel requested by the user.");
                cts.Cancel();
                return true;
            });

            var result = await Try.Run(async () =>
            {
                for (var i = 0; i < total; i++)
                {
                    if (cts.IsCancellationRequested)
                        throw new OperationCanceledException(cts.Token);

                    var path = paths[i];
                    var fullPath = Path.GetFullPath(path);
                    var cutPath = fullPath.LastPath("Assets\\") ?? fullPath.LastPath("Assets/") ?? "";

                    Progress.Report(progressId, (float)i / total, $"Applying: {cutPath}");

                    var iconImporter = AssetImporter.GetAtPath(path) as TextureImporter;
                    if (iconImporter == null)
                        continue;

                    iconImporter.alphaIsTransparency = true;
                    iconImporter.alphaSource = TextureImporterAlphaSource.FromInput;

                    preset.ApplyTo(iconImporter);
                    iconImporter.SaveAndReimport();

                    await UniTask.DelayFrame(10, cancellationToken: token);
                }

                Progress.Finish(progressId);
            });

            result.OnFailure(ex =>
            {
                if (ex is OperationCanceledException)
                {
                    Progress.Finish(progressId, Progress.Status.Canceled);
                    Utils.LogWarning(nameof(ModularAvatarExtensionsIconGeneratorBase), "Apply was canceled.");
                }
                else
                {
                    Progress.Finish(progressId, Progress.Status.Failed);
                    Utils.LogError(nameof(ModularAvatarExtensionsIconGeneratorBase),
                        $"Apply Error: {ex.Message}\n{ex.StackTrace}");
                }
            });
        }

        public static async UniTask ApplyPresetToAllIconsAsync() =>
            await ApplyPresetToAllIconsAsync(CancellationToken.None);

        public static void ApplyPresetToAllIcons() => ApplyPresetToAllIconsAsync().Forget();
#endif

#if UNITY_EDITOR
        protected static string GetMaterialsSha256WithLastTime(MeshData meshData)
        {
            var guidWithTimes = meshData.Materials.Select(m =>
            {
                var path = AssetDatabase.GetAssetPath(m);
                var guid = AssetDatabase.AssetPathToGUID(path);
                var lastWriteTime = !string.IsNullOrEmpty(path) ? File.GetLastWriteTime(path) : DateTime.MinValue;
                return $"{guid}.{lastWriteTime:yyyyMMddHHmmss}";
            });
            return ListHash(guidWithTimes);
        }

        protected static string GetMaterialsSha256(MeshData meshData)
        {
            var guidWithTimes = meshData.Materials.Select(m =>
            {
                var path = AssetDatabase.GetAssetPath(m);
                var guid = AssetDatabase.AssetPathToGUID(path);
                return guid;
            });
            return ListHash(guidWithTimes);
        }

        protected static string GetAssetPathFromMesh(MeshData meshData)
        {
            var path = AssetDatabase.GetAssetPath(meshData.Mesh);
            var fbxPath = AssetDatabase.GetAssetPath(AssetDatabase.LoadMainAssetAtPath(path));
            return fbxPath;
        }

        protected static string? GetFBXAssetSha256WithLastTime(MeshData meshData)
        {
            if (meshData.Mesh == null) return null;
            var assetPath = GetAssetPathFromMesh(meshData);
            if (string.IsNullOrEmpty(assetPath)) return null;
            var assetGuid = AssetDatabase.AssetPathToGUID(assetPath);
            var lastWriteTime = File.GetLastWriteTime(assetPath);
            return $"{assetGuid}.{lastWriteTime:yyyyMMddHHmmss}";
        }

        protected static string? GetFBXAssetSha256(MeshData meshData)
        {
            if (meshData.Mesh == null) return null;
            var assetPath = GetAssetPathFromMesh(meshData);
            if (string.IsNullOrEmpty(assetPath)) return null;
            var assetGuid = AssetDatabase.AssetPathToGUID(assetPath);
            return assetGuid;
        }

        protected string GetIconNameWithLastTime(MeshData[] meshData2)
        {
            var iconNames = meshData2.Select(meshData =>
            {
                if (meshData.Mesh == null) return "";
                var fbxAssetGuid = GetFBXAssetSha256WithLastTime(meshData);
                var matsSha256 = GetMaterialsSha256WithLastTime(meshData);
                return $"{fbxAssetGuid}.{meshData.Mesh?.name}.{matsSha256}";
            });
            return StringHash(iconNames.ListString(),
                shapeKeyDatas.Select(x => $"{RuntimeUtil.AvatarRootPath(x.gameObject)}/{x.shapeKeyName}/{x.value}")
                    .ListString());
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
            return StringHash(iconNames.ListString(),
                shapeKeyDatas.Select(x => $"{RuntimeUtil.AvatarRootPath(x.gameObject)}/{x.shapeKeyName}/{x.value}")
                    .ListString());
        }

        [SerializeField] protected TextureImporter? iconImporter;
        [SerializeField] protected Preset? preset;
#endif
    }
}