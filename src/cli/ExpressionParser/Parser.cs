using System.Text;
using System.Text.RegularExpressions;

namespace ExpressionParser;
public class Parser
{
    private readonly ExpressionRule[] rules;

    public Parser(params ExpressionRule[] rules)
    {
        this.rules = rules;
    }

    public ICollection<Expression> Parse(string filename, ReadOnlySpan<char> content)
    {
        List<string> lines = new();

        {
            var position = 0;
            StringBuilder lineBuilder = new StringBuilder();
            while (position < content.Length)
            {
                if (content[position] == '\r')
                {
                    position++;
                    continue;
                }

                if (content[position] == '\n')
                {
                    position++;
                    lines.Add(lineBuilder.ToString());
                    lineBuilder.Clear();
                    continue;
                }

                lineBuilder.Append(content[position++]);
            }
        }

        List<Expression> results = new();
        {
            for (int l = 0; l < lines.Count; l++)
            {
                var line = lines[l];
                foreach (var expression in rules.Where(e => e.Globs.IsMatch(filename)))
                {
                    var matches = expression.Regex.Matches(line);
                    foreach (Match match in matches)
                    {
                        if (match.Success)
                        {
                            Expression? result = null;
                            if (match.Groups.ContainsKey("content"))
                            {
                                result = new Expression(match.Groups["content"].Value, l + 1);
                            }
                            else
                            {
                                result = new Expression(match.Value, l + 1);
                            }
                            results.Add(result);
                        }
                    }
                }
            }
        }

        return results;
    }
}