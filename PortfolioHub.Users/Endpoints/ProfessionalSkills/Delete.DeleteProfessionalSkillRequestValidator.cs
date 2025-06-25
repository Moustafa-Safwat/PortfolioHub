using FastEndpoints;
using FluentValidation;

namespace PortfolioHub.Users.Endpoints.ProfessionalSkills;

internal sealed class DeleteProfessionalSkillRequestValidator : Validator<DeleteProfessionalSkillRequest>
{
    public DeleteProfessionalSkillRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.")
            .Must(x => Guid.TryParse(x, out _)).WithMessage("Id must be a valid GUID.");
    }
}
