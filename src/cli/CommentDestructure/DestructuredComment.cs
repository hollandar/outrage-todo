using System.Text;
using System.Text.RegularExpressions;

namespace CommentDestructure;

public class DestructuredComment
{
    private readonly Dictionary<string, string> parameters = new();
    private readonly string commentType;
    private string text;
    private readonly int line;

    public DestructuredComment(string commentType, string comment, int line)
    {
        this.commentType = commentType;
        this.line = line;
        DestructureComment(comment);
    }

    public string CommentType => commentType;
    public string Text => text;
    public IDictionary<string, string> Parameters => parameters;

    public int Line => line;

    static Regex dictionaryEntry = new Regex(@"^\s*-\s*(?<key>\w+)\s*[:]\s*(?<value>.*)$", RegexOptions.Compiled);
    void DestructureComment(string comment)
    {
        var lines = comment.Split(Environment.NewLine);
        if (lines.Length == 0)
            return;

        var firstLine = lines[0].Trim();
        if (firstLine.StartsWith(this.CommentType)) firstLine = firstLine.Substring(this.CommentType.Length).TrimStart(':');
        
        var textBuilder = new StringBuilder( firstLine);
        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            var match = dictionaryEntry.Match(line);
            if (match.Success)
            {
                parameters[match.Groups["key"].Value] = match.Groups["value"].Value;
            }
            else
            {
                textBuilder.AppendLine().Append(line);
            }
        }

        this.text = textBuilder.ToString();
    }
}
