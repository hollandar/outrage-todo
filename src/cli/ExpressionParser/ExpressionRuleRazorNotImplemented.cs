using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExpressionParser
{
    public class ExpressionRuleRazorNotImplemented : ExpressionRule
    {
        public ExpressionRuleRazorNotImplemented() : base(
            new string[] { "**/*.cs" },
            new Regex(@"(?<content>throw\s+new\s+NotImplementedException[(].*[)];)", RegexOptions.Compiled)
        )
        {
        }
    }
}
