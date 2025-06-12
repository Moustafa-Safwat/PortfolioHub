using FastEndpoints;
using FluentValidation;

namespace PortfolioHub.Projects.Endpoints.TechnicalSkill;

internal sealed class TechcalSkillRequestValidator : Validator<TechnicalSkillRequest>
{
    public TechcalSkillRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");
    }
}
