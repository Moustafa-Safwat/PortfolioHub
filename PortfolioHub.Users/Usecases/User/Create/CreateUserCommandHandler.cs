using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PortfolioHub.Users.Infrastructure.Context;

namespace PortfolioHub.Users.Usecases.User.Create;

internal sealed class CreateUserCommandHandler(
  UserManager<IdentityUser> userManager,
  RoleManager<IdentityRole> roleManager,
  UsersDbContext dbContext // Added DbContext for database transaction support  
  )
  : IRequestHandler<CreateUserCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        IdentityUser identityUser = new IdentityUser
        {
            UserName = request.UserName,
            Email = request.Email,
        };

        // Start transaction using DbContext  
        using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var identityResult = await userManager.CreateAsync(identityUser, request.Password);

            if (!identityResult.Succeeded)
                return Result.Error(new ErrorList(identityResult.Errors.Select(e => e.Description).ToArray()));

            // Check if role exists using RoleManager  
            var roleExists = await roleManager.RoleExistsAsync(request.Role);
            if (!roleExists)
            {
                // Rollback user creation  
                await userManager.DeleteAsync(identityUser);
                return Result.Error($"Role '{request.Role}' does not exist.");
            }

            var addRoleResult = await userManager.AddToRoleAsync(identityUser, request.Role);
            if (!addRoleResult.Succeeded)
            {
                // Rollback user creation  
                await userManager.DeleteAsync(identityUser);
                return Result.Error(new ErrorList(addRoleResult.Errors.Select(e => e.Description).ToArray()));
            }

            await transaction.CommitAsync(cancellationToken);

            return Result.Success(Guid.Parse(identityUser.Id));
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            return Result.Error("An error occurred while creating the user.");
        }
    }
}
