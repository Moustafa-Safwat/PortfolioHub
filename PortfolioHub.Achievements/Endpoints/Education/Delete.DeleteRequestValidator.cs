using FastEndpoints;
using FluentValidation;

namespace PortfolioHub.Achievements.Endpoints.Education;

internal sealed class DeleteRequestValidator : Validator<DeleteRequest>
{
    public DeleteRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("Id must be a valid GUID.");
    }
}
