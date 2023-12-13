using Microsoft.EntityFrameworkCore;
using Turbulence.Data;
using Turbulence.Shared;

namespace Turbulence.Services
{
    public class ProjectService(TurbulenceDbContext dbContext)
    {

        public ValueResult<bool> SlugUsed(string slug)
        {
            return ValueResult<bool>.Ok(dbContext.Projects.Where(r => r.Slug == slug).Any());
        }

        public Result CreateProject(string userId, string name, string slug)
        {
            if (SlugUsed(slug).Value)
            {
                return $"A project with that {slug} already exists.";
            }

            var member = dbContext.Members.FirstOrDefault(r => r.Subject == userId);
            if (member is null)
            {
                return "You have no membership record";
            }

            var project = new Project()
            {
                Id = Guid.NewGuid(),
                Created = DateTimeOffset.UtcNow,
                Name = name,
                OwnerId = member.Id,
                Slug = slug,
            };

            dbContext.Projects.Add(project);
            dbContext.SaveChanges();

            return true;
        }
    }
}
