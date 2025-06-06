using FluentValidation;

namespace PortfolioHub.Users.Endpoints.User;

internal sealed class UserCredValidator : AbstractValidator<UserCred>
{
    public UserCredValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("User name is required")
            .MaximumLength(100).WithMessage("User name must not exceed 100 characters");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long")
            .MaximumLength(100).WithMessage("Password must not exceed 100 characters");
    }
}