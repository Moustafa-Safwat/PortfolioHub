using Ardalis.Result;
using Microsoft.Extensions.Configuration;
using PortfolioHub.Users.Domain.Entities.Users;
using PortfolioHub.Users.Domain.Interfaces;

namespace PortfolioHub.Users.Usecases.User.Login;

internal sealed class ValidUserService
(
    IUserSecurityRepo userSecurityRepo,
    IConfiguration configuration
) : IValidUser
{
    public async Task<Result<ApplicationUser>> IsValidUserAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        string systemEmail = configuration["SystemUser:Email"] ??
                   throw new InvalidOperationException("System user email configuration value is missing.");

        var isSystemUser = string.Equals(
            user?.Email?.Trim() ?? string.Empty,
            systemEmail.Trim(),
            StringComparison.OrdinalIgnoreCase);

        // System user should not be allowed to log in through the standard login process
        if (isSystemUser)
            return Result.Unauthorized("Invalid User");

        var userSecurityResult = await userSecurityRepo.GetUserWithSecurityByIdAsync(user!.Id, cancellationToken);
        if (!userSecurityResult.IsSuccess)
            return Result.Error(new ErrorList(userSecurityResult.Errors));

        var userWithSecurity = userSecurityResult.Value;
        if (userWithSecurity is null)
            return Result.CriticalError("Invalid user data");

        var isLockedByAdmin = userWithSecurity?.UserSecurity?.IsLockedByAdmin ?? true;
        if (isLockedByAdmin)
            return Result.Unauthorized("User is locked by admin.");

        var isDeleted = userWithSecurity?.UserSecurity?.IsDeleted ?? true;
        if (isDeleted)
            return Result.Unauthorized("User is deleted.");

        return Result.Success(userWithSecurity!);
    }
}
