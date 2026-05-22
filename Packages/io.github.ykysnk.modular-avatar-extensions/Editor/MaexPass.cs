using io.github.ykysnk.ModularAvatarExtensions.Editor.PluginDefinition;
using io.github.ykysnk.utils;
using JetBrains.Annotations;
using nadena.dev.ndmf;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor
{
    [PublicAPI]
    internal abstract class MaexPass<T> : Pass<T> where T : Pass<T>, new()
    {
        protected MaexBuildContext? MaexContext(BuildContext context) =>
            context.Extension<ModularAvatarExtensionsContext>().BuildContext;

        protected void LogC(object message) => Utils.Log(DisplayName, message);

        protected void Log(string key, params object?[] args) =>
            ErrorReport.ReportError(Localization.L, ErrorSeverity.Information, key, args);

        protected void LogError(string key, params object?[] args) =>
            ErrorReport.ReportError(Localization.L, ErrorSeverity.Error, key, args);

        protected void LogNonFatal(string key, params object?[] args) =>
            ErrorReport.ReportError(Localization.L, ErrorSeverity.NonFatal, key, args);

        protected void LogSimple(string title, string detail = "", string hint = "",
            ErrorSeverity severity = ErrorSeverity.Information) =>
            ErrorReport.ReportError(new SimpleStringError(title, detail, hint, severity));
    }
}