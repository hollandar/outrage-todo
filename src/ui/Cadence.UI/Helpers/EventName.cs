using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadence.UI.Helpers
{
    public static class EventName
    {
        public static string Unique(string name) => $"{name}_{Guid.NewGuid().ToString().Replace("-", "")}";
    }
}
