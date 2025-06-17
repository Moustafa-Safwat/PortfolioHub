using FastEndpoints;
using FluentValidation;

namespace PortfolioHub.Projects.Endpoints.TechnicalSkill;

internal sealed class UpdateTechnicalSkillRequestValidator : Validator<UpdateTechnicalSkillRequest>
{
    public UpdateTechnicalSkillRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.")
                            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
    }
}
