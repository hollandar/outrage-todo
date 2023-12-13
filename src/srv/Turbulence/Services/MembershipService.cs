using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Security.Claims;
using Turbulence.Data;

namespace Turbulence.Services
{
    public class MembershipService
    {
        private readonly TurbulenceDbContext dbContext;

        public MembershipService(TurbulenceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task AccessDenied(AccessDeniedContext context)
        {
            context.HandleResponse();
            context.Response.Redirect("/");
            return Task.CompletedTask;
        }

        public Task EstablishMember(TokenValidatedContext context)
        {
            var subject = context.Principal.Claims.First(r => r.Type == ClaimTypes.NameIdentifier).Value;
            var member = dbContext.Members.FirstOrDefault(r => r.Subject == subject);
            if (member == null)
            {
                member = new Member
                {
                    Subject = subject,
                };
                dbContext.Members.Add(member);
                dbContext.SaveChanges();
            }

            return Task.CompletedTask;
        }
    }
}
