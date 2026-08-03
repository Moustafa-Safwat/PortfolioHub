using FluentValidation;

namespace PortfolioHub.Users.Endpoints.VerifyEmail;

internal sealed class ResendEmailValidator : AbstractValidator<ResendEmail>
{
    public ResendEmailValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User Id is required")
            .MaximumLength(100).WithMessage("User Id must not exceed 100 characters");

        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token is required");
    }
}
