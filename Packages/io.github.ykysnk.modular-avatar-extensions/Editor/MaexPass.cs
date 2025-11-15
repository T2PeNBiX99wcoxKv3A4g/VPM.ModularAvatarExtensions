using JetBrains.Annotations;
using nadena.dev.ndmf;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[PublicAPI]
internal abstract class MaexPass<T> : Pass<T> where T : Pass<T>, new()
{
    protected void Log(object message) => Debug.Log($"[{DisplayName}] {message}");

    protected void Log(string detail, string hint)
    {
        var error = new SimpleStringError($"{DisplayName} Warning", detail, hint, ErrorSeverity.Information);
        ErrorReport.ReportError(error);
    }

    protected void LogError(string detail, string hint)
    {
        var error = new SimpleStringError($"{DisplayName} Failed", detail, hint, ErrorSeverity.Error);
        ErrorReport.ReportError(error);
    }

    protected void LogNonFatal(string detail, string hint)
    {
        var error = new SimpleStringError($"{DisplayName} Failed", detail, hint, ErrorSeverity.NonFatal);
        ErrorReport.ReportError(error);
    }
}