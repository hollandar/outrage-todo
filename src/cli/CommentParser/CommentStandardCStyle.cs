using Common;

namespace CommentParser;

public class CommentStandardCStyle : CommentStandard
{
    static Globs globs = new Globs(
        /* CSHARP */ "**/*.cs",
        /* C/C++ */  "**/*.c", "**/*.cpp", "**/*.h", "**/*.hpp",
        /* GOLang */ "**/*.go",
        /* RUST */   "**/*.rs"
    );
    static List<StringLike> singleline = new() { "//", new IsNotString("///") };
    static List<(StringLike start, StringLike end)> multiline = new() { ("/*", "*/") };

    public override Globs Globs => globs;
    public override IEnumerable<StringLike> Singleline => singleline;
    public override IEnumerable<(StringLike start, StringLike end)> Multiline => multiline;
}