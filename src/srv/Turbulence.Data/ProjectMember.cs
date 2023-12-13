using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbulence.Data
{
    public class ProjectMember
    {
        public Guid MemberId { get; set; }
        public Guid ProjectId { get; set; }
        public virtual Member? Member { get; set; }
        public virtual Project? Project { get; set; }
    }
}
