using Ardalis.GuardClauses;
using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace PortfolioHub.Users.Usecases.Role;

internal sealed class AddRoleCommandHandler(
    RoleManager<IdentityRole> roleManager,
    ILogger logger
    ) : IRequestHandler<AddRoleCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.NullOrEmpty(request.Name);

        logger.Information("Attempting to add new role: {RoleName}", request.Name);

        // Check if role already exists
        var existingRole = await roleManager.FindByNameAsync(request.Name);
        if (existingRole != null)
        {
            logger.Warning("Role '{RoleName}' already exists.", request.Name);
            return Result.Error($"Role '{request.Name}' already exists.");
        }

        // Create new role
        var role = new IdentityRole(request.Name);
        var result = await roleManager.CreateAsync(role);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToArray();
            logger.Error("Failed to create role '{RoleName}': {Errors}", request.Name, string.Join("; ", errors));
            return Result.Invalid(errors.Select(e => new ValidationError { ErrorMessage = e }).ToArray());
        }

        // Return the role's Id as a Guid (parse from string)
        if (Guid.TryParse(role.Id, out var roleId))
        {
            logger.Information("Successfully created role '{RoleName}' with Id: {RoleId}", request.Name, roleId);
            return Result.Success(roleId);
        }
        else
        {
            logger.Error("Failed to parse role Id as Guid for role '{RoleName}'. Role Id: {RoleId}", request.Name, role.Id);
            return Result.Error("Failed to parse role Id as Guid.");
        }
    }
}
