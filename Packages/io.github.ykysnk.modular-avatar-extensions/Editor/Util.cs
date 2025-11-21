using System;
using System.Linq;
using HarmonyLib;
using UnityEditor;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[InitializeOnLoad]
internal static class Util
{
    static Util() => EditorApplication.update += TryDisableMaexGizmoIcons;

    private static void TryDisableMaexGizmoIcons()
    {
        try
        {
            DisableMaexGizmoIcons();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        finally
        {
            EditorApplication.update -= TryDisableMaexGizmoIcons;
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

        Debug.Log("Disable MAEX gizmo icons");
        SessionState.SetString("MAEXIconsDisabled", dateTime);
    }
}