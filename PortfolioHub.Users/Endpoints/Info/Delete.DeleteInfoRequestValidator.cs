using FastEndpoints;
using FluentValidation;

namespace PortfolioHub.Users.Endpoints.Info;

internal sealed class DeleteInfoRequestValidator : Validator<DeleteInfoRequest>
{
    public DeleteInfoRequestValidator()
    {
        RuleFor(x => x.key)
            .NotEmpty().WithMessage("Key cannot be empty.")
            .Must(key => Guid.TryParse(key, out _)).WithMessage("Key must be a valid GUID.");
    }
}
