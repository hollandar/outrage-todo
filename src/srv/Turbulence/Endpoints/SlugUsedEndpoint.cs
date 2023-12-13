using FastEndpoints;
using Turbulence.Services;
using Turbulence.Shared;

namespace Turbulence.Endpoints
{
    public class SlugUsedEndpoint(IServiceProvider serviceProvider): Endpoint<SlugUsedRequest, Result>
    {
        public override void Configure()
        {
            Get("/api/project/slug-used/{Slug}");
            //Policies("Authenticated");
            AllowAnonymous();
        }

        public override Task<Result> ExecuteAsync(SlugUsedRequest req, CancellationToken ct)
        {
            var projectService = serviceProvider.GetRequiredService<ProjectService>();
            var result = projectService.SlugUsed(req.Slug);
            return Task.FromResult<Result>(result);
        }
    }
    
    }

