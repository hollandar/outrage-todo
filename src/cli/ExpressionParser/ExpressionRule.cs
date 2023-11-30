using Common;
using System.Text.RegularExpressions;

namespace ExpressionParser;

public class ExpressionRule
{
    private readonly Globs globs;
    private readonly Regex regex;

    public ExpressionRule(ICollection<string> globs, Regex regex)
    {
        this.globs = new Globs(globs.ToArray());
        this.regex = regex;
    }

    public ExpressionRule(ICollection<string> globs, string regex)
    {
        this.globs = new Globs(globs.ToArray());
        this.regex = new Regex(regex, RegexOptions.Compiled);
    }

    public Globs Globs => globs;
    public Regex Regex => regex;
}
