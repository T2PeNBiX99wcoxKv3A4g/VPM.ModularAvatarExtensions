using JetBrains.Annotations;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

[PublicAPI]
public interface IMaexPlugin
{
    void Log(object message);
    void Log(string detail, string hint);
    void LogError(string detail, string hint);
    void LogNonFatal(string detail, string hint);
}