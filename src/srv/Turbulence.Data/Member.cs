using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbulence.Data
{
    public class Member
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Subject { get; set; }
        public virtual ICollection<ProjectMember>? Projects { get; set; }
    }
}
