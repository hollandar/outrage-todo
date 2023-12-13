using FluentValidation;

namespace Turbulence.Shared
{
    public class CreateProjectRequest
    {
        public string Name { get; set; }
        public string Slug { get; set; }
    }

    public class CreateProjectRequestValidator : AbstractValidator<CreateProjectRequest>
    {
        public CreateProjectRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Slug).NotEmpty();
        }
    }

    public record SlugUsedRequest { 
        public string Slug { get; set; }
    }

}
