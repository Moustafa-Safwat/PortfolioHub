using FastEndpoints;
using FluentValidation;

namespace PortfolioHub.Projects.Endpoints.Gallery;

internal sealed class GetGalleryReqValidator : Validator<GetGalleryReq>
{
    public GetGalleryReqValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("PageNumber must be greater than 0.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PageSize must be between 1 and 100.");
    }
}
