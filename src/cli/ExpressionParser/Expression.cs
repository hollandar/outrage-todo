using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionParser
{
    public class Expression
    {
        private readonly string content;
        private readonly int line;

        public Expression(string content, int line)
        {
            this.content = content;
            this.line = line;
        }

        public string Content => content;
        public int Line => line;

        public override string ToString() => content;
    }
}
