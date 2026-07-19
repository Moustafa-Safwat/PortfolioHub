using Ardalis.Result;
using Microsoft.AspNetCore.Identity;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using PortfolioHub.Users.Domain.Entities.Users;
using PortfolioHub.Users.Domain.Interfaces;
using PortfolioHub.Users.Infrastructure.Context;
using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Users.Usecases.User.Create;

internal sealed class CreateUserCommandHandler(
    UserManager<ApplicationUser> userManager,
    IUsernameGenerator usernameGenerator,
    IEntityRepo<UserProfile> userProfileRepo,
    IEntityRepo<UserSecurity> userSecurityRepo,
    IUnitOfWork<UsersDbContext> unitOfWork
    ) : ICommandHandler<CreateUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Start transaction for atomicity
        var beginTransactionResult = await unitOfWork.BeginTransactionAsync(cancellationToken);
        if (!beginTransactionResult.IsSuccess)
            return Result.Error(new ErrorList(beginTransactionResult.Errors.ToArray()));

        try
        {
            // Step 1: Check if email already exists
            var existingUser = await userManager.FindByEmailAsync(request.Email);
            if (existingUser is not null)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return Result.Error($"A user with email '{request.Email}' already exists.");
            }

            // Step 2: Generate unique username
            var usernameResult = await usernameGenerator.GenerateUniqueUsernameAsync(
                email: request.Email,
                firstName: request.FirstName,
                lastName: request.LastName,
                cancellationToken: cancellationToken
            );

            if (!usernameResult.IsSuccess)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return Result.Error(new ErrorList(usernameResult.Errors.ToArray()));
            }

            // Step 3: Create ApplicationUser with auto-generated username
            var user = new ApplicationUser(
                email: request.Email,
                firstName: request.FirstName,
                lastName: request.LastName,
                profileImageUrl: string.Empty // Default empty, can be updated later
            );

            // Set the auto-generated username
            user.SetUsername(usernameResult.Value);

            // Step 4: Use UserManager to create user with password (handles password hashing)
            var createUserResult = await userManager.CreateAsync(user, request.Password);
            if (!createUserResult.Succeeded)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                var errors = createUserResult.Errors.Select(e => e.Description).ToArray();
                return Result.Error(new ErrorList(errors));
            }

            // Step 5: Assign default role 
            var assignRoleResult = await userManager.AddToRoleAsync(user, request.Role);
            if (!assignRoleResult.Succeeded)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                var errors = assignRoleResult.Errors.Select(e => e.Description).ToArray();
                return Result.Error(new ErrorList(errors));
            }

            // Step 6: Create UserSecurity with default settings
            var userSecurity = new UserSecurity(
                id: Guid.NewGuid(),
                userId: user.Id
            );

            var addSecurityResult = await userSecurityRepo.AddAsync(userSecurity, cancellationToken);
            if (!addSecurityResult.IsSuccess)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return Result.Error(new ErrorList(addSecurityResult.Errors.ToArray()));
            }

            // Step 7: Create UserProfile with default/empty values (can be updated later)
            var userProfile = new UserProfile(
                id: Guid.NewGuid(),
                userId: user.Id
            );

            var addProfileResult = await userProfileRepo.AddAsync(userProfile, cancellationToken);
            if (!addProfileResult.IsSuccess)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return Result.Error(new ErrorList(addProfileResult.Errors.ToArray()));
            }

            // Step 8: Save all changes
            var saveResult = await unitOfWork.SaveChangesAsync(cancellationToken);
            if (!saveResult.IsSuccess)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return Result.Error(new ErrorList(saveResult.Errors.ToArray()));
            }

            // Step 9: Commit transaction
            var commitResult = await unitOfWork.CommitTransactionAsync(cancellationToken);
            if (!commitResult.IsSuccess)
            {
                return Result.Error(new ErrorList(commitResult.Errors.ToArray()));
            }

            return Result.Success(user.Id);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Error($"An unexpected error occurred while creating the user: {ex.Message}");
        }
    }
}
