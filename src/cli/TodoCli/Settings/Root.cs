using CommentParser;
using ExpressionParser;
using System.Text.Json.Serialization;

namespace ConsoleApp1.Settings;

public enum LanguageType
{
    css,
    cstyle,
    html,
    js,
    python,
    razor,
    zig
};
public static class LanguageTypes
{
    public static string? ToDisplayString(this LanguageType type)
    {
        return Enum.GetName(typeof(LanguageType), type);
    }

    public static CommentStandard ToStandard(this LanguageType type)
    {
        return type switch
        {
            LanguageType.css => new CommentStandardCSS(),
            LanguageType.cstyle=> new CommentStandardCStyle(),
            LanguageType.html => new CommentStandardHTML(),
            LanguageType.js => new CommentStandardJS(),
            LanguageType.python => new CommentStandardPython(),
            LanguageType.razor => new CommentStandardRazor(),
            LanguageType.zig => new CommentStandardZig(),
            _ => throw new UnknownStandardException($"{type.ToDisplayString()}: {type} does not have a valid standard language implementation")
        };
    }
}

public enum ExpressionType
{
    cs_notimplemented,
    razor_notimplemented
};

public static class ExpressionTypes
{
    public static string? ToDisplayString(this ExpressionType type)
    {
        return Enum.GetName(typeof(ExpressionType), type);
    }

    public static ExpressionRule ToExpressionRule(this ExpressionType type)
    {
        return type switch
        {
            ExpressionType.cs_notimplemented => new ExpressionRuleCSNotImplemented(),
            ExpressionType.razor_notimplemented => new ExpressionRuleRazorNotImplemented(),
            _ => throw new UnknownStandardException($"{type.ToDisplayString()}: {type} does not have a valid standard expression implementation")
        };
    }
}
public class StartEnd
{
    public string Start { get; set; } = "";
    public string End { get; set; } = "";
}

public class CustomLanguage
{
    public ICollection<string> Globs { get; set; } = new List<string>();
    public ICollection<string> Singleline { get; set; } = new List<string>();
    public ICollection<string> NotSingleline { get; set; } = new List<string>();
    public ICollection<StartEnd> Multiline { get; set; } = new List<StartEnd>();
}

public class CustomExpression
{
    public ICollection<string> Globs { get; set; } = new List<string>();
    public string Regex { get; set; } = "^.*$";
}

public class Root
{
    public ICollection<LanguageType> Languages { get; set; } = new List<LanguageType>();
    public ICollection<ExpressionType> Expressions{ get; set; } = new List<ExpressionType>();
    public ICollection<CustomLanguage> CustomLanguages { get; set; } = new List<CustomLanguage>();
    public ICollection<CustomExpression> CustomExpressions { get; set; } = new List<CustomExpression>();
}
