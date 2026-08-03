using Ardalis.GuardClauses;
using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace PortfolioHub.Users.Usecases.Role;

internal sealed class AddRoleCommandHandler(
    RoleManager<IdentityRole<Guid>> roleManager
    ) : IRequestHandler<AddRoleCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.NullOrEmpty(request.Name);


        // Check if role already exists
        var existingRole = await roleManager.FindByNameAsync(request.Name);
        if (existingRole != null)
            return Result.Error($"Role '{request.Name}' already exists.");

        // Create new role
        var role = new IdentityRole<Guid>(request.Name);
        var result = await roleManager.CreateAsync(role);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToArray();
            return Result.Invalid(errors.Select(e => new ValidationError { ErrorMessage = e }).ToArray());
        }

        return Result.Success(role.Id);
    }
}
