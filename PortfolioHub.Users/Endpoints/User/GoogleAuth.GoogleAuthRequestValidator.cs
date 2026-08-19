using FastEndpoints;
using FluentValidation;

namespace PortfolioHub.Users.Endpoints.User;

internal sealed class GoogleAuthRequestValidator 
    : Validator<GoogleAuthRequest>
{
    public GoogleAuthRequestValidator()
    {
        RuleFor(x => x.Credential)
            .NotEmpty().WithMessage("Credential is required.");
    }
}
