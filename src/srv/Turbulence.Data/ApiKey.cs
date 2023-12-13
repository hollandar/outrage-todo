using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbulence.Data
{
    public class ApiKey
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ProjectId { get; set; }
        public string? Key { get; set; }
        public DateTimeOffset Created { get; set; }
        public bool IsRevoked { get; set; }
        public virtual Project? Project { get; set; }
    }
}
