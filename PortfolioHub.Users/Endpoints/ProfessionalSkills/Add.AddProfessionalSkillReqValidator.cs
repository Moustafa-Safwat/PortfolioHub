using FastEndpoints;
using FluentValidation;

namespace PortfolioHub.Users.Endpoints.ProfessionalSkills;

internal sealed class AddProfessionalSkillReqValidator : Validator<AddProfessionalSkillReq>
{
    public AddProfessionalSkillReqValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
    }
}
