using Common;

namespace CommentParser;

public class CommentStandardJS : CommentStandard
{
    static Globs globs = new Globs("**/*.js", "**/*.ts");
    static List<StringLike> singleline = new() { "//" };
    static List<(StringLike start, StringLike end)> multiline = new() { ("/*", "*/") };

    public override Globs Globs => globs;
    public override IEnumerable<StringLike> Singleline => singleline;
    public override IEnumerable<(StringLike start, StringLike end)> Multiline => multiline;
}