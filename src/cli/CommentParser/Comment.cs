namespace CommentParser;

public class Comment
{
    private readonly string content;
    private readonly int line;

    public Comment(string content, int line)
    {
        this.content = content;
        this.line = line;
    }

    public string Content => content;
    public int Line => line;

    public override string ToString() => content;
}
