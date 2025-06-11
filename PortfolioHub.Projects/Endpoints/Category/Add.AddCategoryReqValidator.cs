using FastEndpoints;
using FluentValidation;

namespace PortfolioHub.Projects.Endpoints.Category;

internal sealed class AddCategoryReqValidator : Validator<AddCategoryReq>
{
    public AddCategoryReqValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required.")
            .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.");
    }
}
