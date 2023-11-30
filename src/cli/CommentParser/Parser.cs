using System.Text;

namespace CommentParser;

public class Parser
{
    private readonly CommentStandard[] standards;

    public Parser(params CommentStandard[] standards)
    {
        this.standards = standards;
    }

    public ICollection<Comment> Parse(string filename, ReadOnlySpan<char> content)
    {
        var applicableStandards = standards.Where(s => s.Globs.IsMatch(filename)).ToArray();
        List<Comment> comments = new();
        int position = 0;
        int line = 0;

        StringBuilder commentBuilder = new StringBuilder();

        while (position < content.Length)
        {
            if (content[position] == '\r')
            {
                position++;
                continue;
            }

            if (content[position] == '\n')
            {
                line++;
                position++;
                continue;
            }

            ConsumeWhitespace(content, ref position);

            bool found = false;
            foreach (var standard in applicableStandards)
            {
                var singleLineWidth = standard.StartOfSingleline(content, position);
                if (singleLineWidth > 0)
                {
                    while (singleLineWidth > 0)
                    {
                        position += singleLineWidth;
                        commentBuilder.AppendLine(TakeToEol(content, ref position).ToString());
                        line++;
                        ConsumeWhitespace(content, ref position);
                        singleLineWidth = standard.StartOfSingleline(content, ++position);
                    }

                    comments.Add(new Comment(commentBuilder.ToString(), line));
                    commentBuilder.Clear();
                    found = true;
                    break;
                }

                var multiLineWidth = standard.StartOfMultiline(content, position);
                if (multiLineWidth > 0)
                {
                    position += multiLineWidth;

                    multiLineWidth = standard.EndOfMultiline(content, position);
                    while (multiLineWidth <= 0)
                    {
                        if (content[position] != '\r')
                        {
                            if (content[position] == '\n')
                            {
                                line++;
                                commentBuilder.Append(Environment.NewLine);
                            }
                            else
                            {
                                commentBuilder.Append(content[position]);
                            }
                        }

                        multiLineWidth = standard.EndOfMultiline(content, ++position);
                    }

                    position += multiLineWidth;
                    comments.Add( new Comment(commentBuilder.ToString(), line));
                    commentBuilder.Clear();
                    found = true;
                    break;
                }
            }

            if (!found) position++;
        }

        return comments;
    }

    static HashSet<char> whitespace = new HashSet<char> { ' ', '\t' };
    private void ConsumeWhitespace(ReadOnlySpan<char> content, ref int position)
    {
        while (whitespace.Contains(content[position]))
            position++;
    }

    private ReadOnlySpan<char> TakeToEol(ReadOnlySpan<char> content, ref int position)
    {
        var start = position;
        while (content[position] != '\n' && position < content.Length)
            position++;

        var slice = content.Slice(start, position - start);
        position += 1;

        return slice;
    }
}
