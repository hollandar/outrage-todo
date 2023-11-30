using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class CommentPatterns
    {
        private Dictionary<string, Regex> _patterns = new();

        public void AddPattern(string name, string pattern)
        {
            var regex = new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
            _patterns[name] = regex;
        }

        public IEnumerable<(string, Match)> FindMatches(string text)
        {
            foreach (var pattern in _patterns)
            {
                foreach (Match match in pattern.Value.Matches(text))
                {
                    yield return (pattern.Key, match);
                }
            }
        }


    }
    public class Tag
    {
        public Tag(string patternName, string tagLine, int line)
        {
            this.PatternName = patternName;
            this.Tagline = tagLine;
            this.Line = line;
        }

        public string PatternName { get; }
        public string Tagline { get; }
        public int Line;
    }

    public class CommentFinder
    {
        private CommentPatterns patterns;

        public CommentFinder()
        {
            this.patterns = new CommentPatterns();
            this.patterns.AddPattern("TODO", @"^\s*?//\s*?TODO[:]?\s*(?<tagline>.*)$");
        }

        public IEnumerable<Tag> FindComments(string text)
        {
            var matches = this.patterns.FindMatches(text);
            foreach (var match in matches)
            {
                var tagline = match.Item2.Groups["tagline"].Value?.Replace("\r", String.Empty) ?? String.Empty;
                var line = CountLines(text.Substring(0, match.Item2.Index).ReplaceLineEndings());
                yield return new Tag(match.Item1, tagline, line);
            }
        }

        private static int CountLines(string str)
        {
            if (str == null)
                throw new ArgumentNullException("str");
            if (str == string.Empty)
                return 0;
            int index = -1;
            int count = 0;
            while (-1 != (index = str.IndexOf(Environment.NewLine, index + 1)))
                count++;

            return count + 1;
        }
    }
}
