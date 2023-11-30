namespace CommentDestructure;

public class CommentTypes
{
    const string TodoFlag = "TODO";
    const string IdeaFlag = "IDEA";
    const string IssueFlag = "ISSUE";

    List<string> commentTypes = new();

    public CommentTypes()
    {
        this.commentTypes.Add(TodoFlag);    
        this.commentTypes.Add(IdeaFlag);    
        this.commentTypes.Add(IssueFlag);    
    }

    public string? Match(string comment)
    {
        foreach (var commentType in commentTypes)
        {
            var startsWith = comment.TrimStart().StartsWith(commentType);
            if (startsWith) return commentType;
        }

        return null;
    }
}