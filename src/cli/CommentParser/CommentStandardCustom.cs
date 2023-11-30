using Common;

namespace CommentParser;

public class CommentStandardCustom : CommentStandard
{
    private Globs globs;
    private List<StringLike> singleline = new();
    private List<(StringLike start, StringLike end)> multiline = new();

    public CommentStandardCustom(Globs globs, IEnumerable<StringLike> singleline, IEnumerable<(StringLike start, StringLike end)> multiline)
    {
        this.globs = globs;
        this.singleline.AddRange(singleline);
        this.multiline.AddRange(multiline);
    }

    public override Globs Globs => globs;

    public override IEnumerable<StringLike> Singleline => singleline;

    public override IEnumerable<(StringLike start, StringLike end)> Multiline => multiline;
}