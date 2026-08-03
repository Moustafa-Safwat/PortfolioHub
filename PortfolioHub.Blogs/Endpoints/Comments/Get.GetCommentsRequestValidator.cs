using FastEndpoints;
using FluentValidation;

namespace PortfolioHub.Blogs.Endpoints.Comments;

internal sealed class GetCommentsRequestValidator
    : Validator<GetCommentsRequest>
{
    public GetCommentsRequestValidator()
    {
        RuleFor(request => request.BlogId)
          .NotEqual(Guid.Empty)
          .WithMessage("Blog ID is required.");

        RuleFor(request => request.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be at least 1.");

        RuleFor(request => request.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100.");
    }
}
