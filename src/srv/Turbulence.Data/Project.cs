using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbulence.Data
{
    public class Project
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid OwnerId { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public DateTimeOffset Created { get; set; }
        public virtual Member? Owner { get; set; }
        public virtual ICollection<ProjectMember>? Members { get; set; }

    }
}
