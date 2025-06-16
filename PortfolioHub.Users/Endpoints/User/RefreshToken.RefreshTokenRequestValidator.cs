using FastEndpoints;
using FluentValidation;

namespace PortfolioHub.Users.Endpoints.User;

internal sealed class RefreshTokenRequestValidator : Validator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(r => r.RefreshToken)
            .NotNull()
            .WithMessage("Refresh token must not be null.")
            .NotEmpty()
            .WithMessage("Refresh token must not be empty.");
    }
}
