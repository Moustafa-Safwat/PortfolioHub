using FastEndpoints;
using FluentValidation;

namespace PortfolioHub.Users.Endpoints.Role;

internal sealed class AddRoleReqValidator : Validator<AddRoleReq>
{
    public AddRoleReqValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Role name is required.")
            .MinimumLength(3).WithMessage("Role name must be at least 3 characters long.")
            .MaximumLength(50).WithMessage("Role name must not exceed 50 characters.");
    }
}
