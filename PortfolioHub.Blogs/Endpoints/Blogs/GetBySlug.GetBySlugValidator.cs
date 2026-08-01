using FastEndpoints;
using FluentValidation;

namespace PortfolioHub.Blogs.Endpoints.Blogs;

internal sealed class GetBySlugValidator : Validator<GetBySlugRequest>
{
    public GetBySlugValidator()
    {
        RuleFor(request => request.Slug)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Slug is required.")
            .MaximumLength(100)
            .WithMessage($"Slug must not exceed 100 characters.")
            .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage("Slug must contain lowercase letters, numbers, and hyphens only.");
    }
}
