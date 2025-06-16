using Ardalis.Result;
using MediatR;
using PortfolioHub.Users.Domain.Interfaces;
using Serilog;

namespace PortfolioHub.Users.Usecases.User.Logout;

internal sealed class LogoutCommandHandler(
    IRefreshTokenRepo refreshTokenRepo,
    ILogger logger
    ) : IRequestHandler<LogoutCommand, Result>
{
    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        logger.Information("Logout attempt for UserId: {UserId}", request.UserId);

        var existingRefreshTokenResult = await refreshTokenRepo.GetActiveRefreshTokenByUserIdAsync(
            request.UserId, cancellationToken);

        if (!existingRefreshTokenResult.IsSuccess)
        {
            logger.Warning("No active refresh token found for UserId: {UserId}. Errors: {Errors}", request.UserId, existingRefreshTokenResult.Errors);
            return Result.Error(new ErrorList(existingRefreshTokenResult.Errors));
        }

        var existingRefreshToken = existingRefreshTokenResult.Value;
        existingRefreshToken.Revoke();
        logger.Information("Refresh token revoked for UserId: {UserId}", request.UserId);

        var saveResult = await refreshTokenRepo.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
        {
            logger.Error("Failed to save changes after revoking refresh token for UserId: {UserId}. Errors: {Errors}", request.UserId, saveResult.Errors);
            return Result.Error(new ErrorList(saveResult.Errors));
        }

        logger.Information("User with Id [{UserId}] logged out successfully.", request.UserId);
        return Result.SuccessWithMessage($"User with Id [{request.UserId}] logged out successfully.");
    }
}