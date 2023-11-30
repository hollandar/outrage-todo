using Common;

namespace CommentParser;

public abstract class CommentStandard
{
    public abstract Globs Globs { get; }
    public abstract IEnumerable<StringLike> Singleline { get; }
    public abstract IEnumerable<(StringLike start, StringLike end)> Multiline { get; }

    public int StartOfSingleline(ReadOnlySpan<char> content, int position)
    {
        int length = 0;
        foreach (var stringLike in Singleline)
        {
            if (stringLike is IsString && content.Slice(position, content.Length - position).StartsWith(stringLike.ToString()) && length == 0)
            {
                length = stringLike.Length;
            }
            if (stringLike is IsNotString && content.Slice(position, content.Length - position).StartsWith(stringLike.ToString()))
            {
                length = -1;
            }
        }

        return length;
    }

    public int StartOfMultiline(ReadOnlySpan<char> content, int position)
    {
        int length = 0;
        foreach (var stringLike in Multiline)
        {
            if (stringLike.start is IsString && content.Slice(position, content.Length - position).StartsWith(stringLike.start.ToString()) && length == 0)
            {
                length = stringLike.start.Length;
            }
            if (stringLike.start is IsNotString && content.Slice(position, content.Length - position).StartsWith(stringLike.start.ToString()))
            {
                length = -1;
            }
        }

        return length;
    }

    public int EndOfMultiline(ReadOnlySpan<char> content, int position)
    {
        int length = 0;
        foreach (var stringLike in Multiline)
        {
            if (stringLike.end is IsString && content.Slice(position, content.Length - position).StartsWith(stringLike.end.ToString()) && length == 0)
            {
                length = stringLike.start.Length;
            }
            if (stringLike.end is IsNotString && content.Slice(position, content.Length - position).StartsWith(stringLike.end.ToString()))
            {
                length = -1;
            }
        }

        return length;
    }
}