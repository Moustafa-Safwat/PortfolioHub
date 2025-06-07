using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PortfolioHub.Users.Infrastructure.Context;
using Serilog;

namespace PortfolioHub.Users.Usecases.User.Create;

internal sealed class CreateUserCommandHandler(
  UserManager<IdentityUser> userManager,
  RoleManager<IdentityRole> roleManager, 
  ILogger logger,
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
            {
                logger.Information("Failed to create user {@UserName} with email {@Email}. Errors: {@Errors}",
                    request.UserName, request.Email, identityResult.Errors.Select(e => e.Description));
                return Result.Error(new ErrorList(identityResult.Errors.Select(e => e.Description).ToArray()));
            }

            // Check if role exists using RoleManager  
            var roleExists = await roleManager.RoleExistsAsync(nameof(request.Role));
            if (!roleExists)
            {
                // Rollback user creation  
                await userManager.DeleteAsync(identityUser);
                logger.Information("Role {@Role} does not exist. Rolling back user creation for {@UserName} with email {@Email}.",
                    request.Role, request.UserName, request.Email);
                return Result.Error($"Role '{request.Role}' does not exist.");
            }

            var addRoleResult = await userManager.AddToRoleAsync(identityUser, nameof(request.Role));
            if (!addRoleResult.Succeeded)
            {
                // Rollback user creation  
                await userManager.DeleteAsync(identityUser);
                logger.Information("Failed to add user {@UserName} to role {@Role}. Rolling back user creation. Errors: {@Errors}",
                    request.UserName, request.Role, addRoleResult.Errors.Select(e => e.Description));
                return Result.Error(new ErrorList(addRoleResult.Errors.Select(e => e.Description).ToArray()));
            }

            await transaction.CommitAsync(cancellationToken);

            logger.Information("User {@UserName} with email {@Email} has been created successfully",
                request.UserName, request.Email);
            return Result.Success(Guid.Parse(identityUser.Id));
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            logger.Error(ex, "Exception occurred while creating user {@UserName} with email {@Email}", request.UserName, request.Email);
            return Result.Error("An error occurred while creating the user.");
        }
    }
}
