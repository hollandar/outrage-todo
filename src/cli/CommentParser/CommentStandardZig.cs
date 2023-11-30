using Common;

namespace CommentParser;

public class CommentStandardZig : CommentStandard
{
    static Globs globs = new Globs("**/*.zig");
    static List<StringLike> singleline = new() { "//", new IsNotString("///"), new IsNotString("//!") };
    static List<(StringLike start, StringLike end)> multiline = new();

    public override Globs Globs => globs;
    public override IEnumerable<StringLike> Singleline => singleline;
    public override IEnumerable<(StringLike start, StringLike end)> Multiline => multiline;
}