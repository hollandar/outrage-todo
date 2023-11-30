using GlobExpressions;

namespace Common;

public class Globs
{
    private readonly List<string> globs = new();

    public Globs(params string[] globs)
    {
        this.Add(globs);
    }

    public void Add(params string[] globs)
    {
        this.globs.AddRange(globs);
    }
    
    public bool IsMatch(string text)
    {
        foreach (var glob in this.globs)
        {
            if (Glob.IsMatch(text, glob))
            {
                return true;
            }
        }

        return false;
    }
}
