using FastEndpoints;
using FluentValidation;

namespace PortfolioHub.Blogs.Endpoints.BlogTags;

internal sealed class AddTagRequestValidator : Validator<AddTagRequest>
{
    public AddTagRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tag name is required.");
    }
}
