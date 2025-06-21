using FastEndpoints;
using FluentValidation;

namespace PortfolioHub.Projects.Endpoints.Project;

// TODO: this class should be removed
internal sealed class GetProjectsValidator : Validator<GetProjectsReq>
{
    public GetProjectsValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("PageNumber must be greater than 0.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PageSize must be between 1 and 100.");
    }
}
