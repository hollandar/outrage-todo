using FastEndpoints;
using System.Security.Claims;
using Turbulence.Services;
using Turbulence.Shared;

namespace Turbulence.Endpoints
{
    public class CreateProjectEndpoint(IServiceProvider serviceProvider): Endpoint<CreateProjectRequest, Result>
    {
        public override void Configure()
        {
            Post("/api/project/create");
            Policies("Authenticated");
        }

        public override Task<Result> ExecuteAsync(CreateProjectRequest req, CancellationToken ct)
        {
            var subject = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var projectService = serviceProvider.GetRequiredService<ProjectService>();
            var result = projectService.CreateProject(subject, req.Name, req.Slug);
            return Task.FromResult(result);
        }
    }
}
