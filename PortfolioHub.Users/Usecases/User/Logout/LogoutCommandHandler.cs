using Ardalis.Result;
using MediatR;
using PortfolioHub.Users.Domain.Interfaces;

namespace PortfolioHub.Users.Usecases.User.Logout;

internal sealed class LogoutCommandHandler(
    IRefreshTokenRepo refreshTokenRepo
    ) : IRequestHandler<LogoutCommand, Result>
{
    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var existingRefreshTokenResult = await refreshTokenRepo.GetActiveRefreshTokenByUserIdAsync(
            request.UserId, cancellationToken);

        if (!existingRefreshTokenResult.IsSuccess)
            return Result.Error(new ErrorList(existingRefreshTokenResult.Errors));

        var existingRefreshToken = existingRefreshTokenResult.Value;
        existingRefreshToken.Revoke();

        var saveResult = await refreshTokenRepo.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
            return Result.Error(new ErrorList(saveResult.Errors));

        return Result.SuccessWithMessage($"User with Id [{request.UserId}] logged out successfully.");
    }
}