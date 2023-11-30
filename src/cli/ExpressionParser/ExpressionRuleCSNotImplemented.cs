using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExpressionParser
{
    public class ExpressionRuleCSNotImplemented : ExpressionRule
    {
        public ExpressionRuleCSNotImplemented() : base(
            new string[] { "**/*.cs" },
            new Regex(@"^\s*(?<content>throw\s+new\s+NotImplementedException[(].*[)];)\s*$", RegexOptions.Compiled)
        )
        {
        }
    }
}
