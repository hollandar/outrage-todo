namespace CommentParser;

public abstract class StringLike
{
    string value;

    public StringLike(string value)
    {
        this.value = value;
    }

    public static implicit operator StringLike(string value)
    {
        return new IsString(value);
    }

    public override string ToString() => value;
    public virtual int Length => value.Length;
}