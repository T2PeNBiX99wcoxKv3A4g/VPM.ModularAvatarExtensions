using nadena.dev.ndmf;
using nadena.dev.ndmf.localization;

namespace io.github.ykysnk.ModularAvatarExtensions.Editor;

public class SimpleStringError : SimpleError
{
    private readonly string _detail;
    private readonly string _hint;

    private readonly string _title;

    public SimpleStringError(string title, string detail, string hint, ErrorSeverity severity)
    {
        _title = title;
        _detail = detail;
        _hint = hint;
        Severity = severity;
    }

    public override Localizer Localizer { get; } = new("en-US", () => new()
    {
        ("en-US", _ => "{0}")
    });

    public override string TitleKey => _title;

    public override string[] TitleSubst => new[]
    {
        _title
    };

    public override string DetailsKey => _detail;

    public override string[] DetailsSubst => new[]
    {
        _detail
    };

    public override string HintKey => _hint;

    public override string[] HintSubst => new[]
    {
        _hint
    };

    public override ErrorSeverity Severity { get; }
}