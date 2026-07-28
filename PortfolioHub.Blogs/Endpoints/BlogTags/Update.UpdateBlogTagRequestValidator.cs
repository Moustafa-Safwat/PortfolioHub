using FastEndpoints;
using FluentValidation;

namespace PortfolioHub.Blogs.Endpoints.BlogTags;

internal sealed class UpdateBlogTagRequestValidator : Validator<UpdateBlogTagRequest>
{
    public UpdateBlogTagRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty).WithMessage("Id should be a valid GUID.")
            .NotEmpty().WithMessage("Id is requried.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters.");
    }
}
