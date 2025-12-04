using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using io.github.ykysnk.Localization.Editor;
using nadena.dev.ndmf.localization;
using UnityEditor;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[InitializeOnLoad]
internal static class Localization
{
    private static readonly ImmutableList<string>
        SupportedLanguages = new[]
        {
            "en-US", "ja-JP"
        }.ToImmutableList();

    static Localization()
    {
        GlobalLocalization.OnLocalizationReload -= OnLocalizationReload;
        GlobalLocalization.OnLocalizationReload += OnLocalizationReload;
    }

    public static Localizer? L { get; private set; }

    private static void OnLocalizationReload()
    {
        L = new(SupportedLanguages[0], () => SupportedLanguages.Select(lang => (lang, LanguageLookup(lang))).ToList());
    }

    private static Func<string, string> LanguageLookup(string lang)
    {
        try
        {
            return InternalLocalizationExtensions.Helper.GetLanguageLocalization(lang).GetValueOrDefault;
        }
        catch (Exception e)
        {
            return _ => $"Language Lookup Error: {e.Message}";
        }
    }
}