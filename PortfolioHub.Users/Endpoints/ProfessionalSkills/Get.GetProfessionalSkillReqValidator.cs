using FastEndpoints;
using FluentValidation;

namespace PortfolioHub.Users.Endpoints.ProfessionalSkills;

internal sealed class GetProfessionalSkillReqValidator : Validator<GetProfessionalSkillReq>
{
    public GetProfessionalSkillReqValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be greater than or equal to 1.");
        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageSize must be greater than or equal to 1.");
    }

}
