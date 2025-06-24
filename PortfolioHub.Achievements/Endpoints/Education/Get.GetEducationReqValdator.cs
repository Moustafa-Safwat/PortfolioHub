using FastEndpoints;
using FluentValidation;

namespace PortfolioHub.Achievements.Endpoints.Education;

internal sealed class GetEducationReqValdator:Validator<GetEducationReq>
{
    public GetEducationReqValdator()
    {
        RuleFor(x => x.Page).GreaterThan(0).WithMessage("Page must be greater than 0.");
        RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("PageSize must be greater than 0.");
    }
}
