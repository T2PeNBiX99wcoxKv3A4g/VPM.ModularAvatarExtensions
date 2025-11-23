using System;
using System.IO;
using System.Linq;
using HarmonyLib;
using io.github.ykysnk.utils;
using UnityEditor;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[InitializeOnLoad]
internal static class Util
{
    private const string IconPath =
        "Packages/io.github.ykysnk.modular-avatar-extensions/Runtime/Icons/Icon_MAEX_Script.png";

    static Util() => EditorApplication.update += TryRun;

    private static void TryRun()
    {
        try
        {
            DisableMaexGizmoIcons();
            ChangeMaexIcons();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        finally
        {
            EditorApplication.update -= TryRun;
        }
    }

    // Refs: nadena.dev.modular_avatar.core.editor.Util:DisableMAGizmoIcons
    private static void DisableMaexGizmoIcons()
    {
        var dateTime = DateTime.Now.ToString("yyyy-MM-dd");

        // Disable icons then waiting util next day
        if (SessionState.GetString("MAEXIconsDisabled", "") == dateTime)
            return;

        var annotations = (Array)Traverse.CreateWithType("UnityEditor.AnnotationUtility").Method("GetAnnotations")
            .GetValue();
        var hasTurnOffInBuild = (from object? annotation in annotations
            let classID = Traverse.Create(annotation).Field<int>("classID").Value
            let scriptClass = Traverse.Create(annotation).Field<string>("scriptClass").Value
            where classID == 114 && scriptClass == "ModularAvatarExtensionsTurnOffInBuild"
            select classID).Any();

        if (!hasTurnOffInBuild)
            return;

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        foreach (var ty in assembly.GetTypes())
            if (typeof(AvatarMaexComponent).IsAssignableFrom(ty) && !ty.IsAbstract)
                GizmoUtility.SetIconEnabled(ty, false);

        Utils.Log(nameof(DisableMaexGizmoIcons), "Disable MAEX gizmo icons");
        SessionState.SetString("MAEXIconsDisabled", dateTime);
    }

    private static void ChangeMaexIcons()
    {
        Texture2D? maexIcon = null;

        if (File.Exists(IconPath))
            maexIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(IconPath);

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        foreach (var ty in assembly.GetTypes())
            if (typeof(AvatarMaexComponent).IsAssignableFrom(ty) && !ty.IsAbstract)
            {
                var path = $"Packages/io.github.ykysnk.modular-avatar-extensions/Runtime/{ty.Name}.cs";

                Utils.Log(nameof(ChangeMaexIcons), $"Test: Path {path}");

                if (!File.Exists(path) || AssetImporter.GetAtPath(path) is not MonoImporter monoImporter ||
                    maexIcon == null ||
                    monoImporter.GetIcon() == maexIcon)
                    continue;

                monoImporter.SetIcon(maexIcon);
                monoImporter.SaveAndReimport();
                Utils.Log(nameof(ChangeMaexIcons), $"Change to MAEX icon: {path}");
            }
    }
}