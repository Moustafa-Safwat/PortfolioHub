using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Configuration;
using PortfolioHub.Users.Domain.Interfaces;
using PortfolioHub.Users.Usecases.User.Login;
using Serilog;

namespace PortfolioHub.Users.Usecases.User.RefreshToken;

internal sealed class CreateRefreshTokenCommandHandler(
    ILogger logger,
    JwtService jwtService,
    TokenHasher tokenHasher,
    IRefreshTokenRepo refreshTokenRepo,
    IConfiguration configuration
    ) : IRequestHandler<CreateRefreshTokenCommand, Result<LoginDtoResult>>
{
    public async Task<Result<LoginDtoResult>> Handle(CreateRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // query from the database for the refresh token
        var hashedRefreshToken = tokenHasher.HashToken(request.RefreshToken);
        var activeRefreshToken = await refreshTokenRepo.GetActiveRefreshTokenByHashedTokenAsync(hashedRefreshToken, cancellationToken);
        if (!activeRefreshToken.IsSuccess)
        {
            logger.Error("Failed to retrieve active refresh token: {Error}", activeRefreshToken.Errors);
            return Result.Unauthorized(activeRefreshToken.Errors.ToArray());
        }
        // rotate the refresh token
        var refreshTokenEntity = activeRefreshToken.Value;
        refreshTokenEntity.Revoke();

        var accessToken = await jwtService.GenerateAccessTokenAsync(refreshTokenEntity.User!, cancellationToken);
        var refreshToken = (await jwtService.GenerateRefreshTokenAsync(refreshTokenEntity.User!, cancellationToken)).Value;

        if (!double.TryParse(configuration["Auth:RefreshTokenExpirationDays"], out double refreshTokenExpirationDays))
            throw new InvalidOperationException("Refresh token expiration time is not configured or is invalid.");

        var refreseTokenEntity = new Domain.Entities.RefreshToken(
            id: Guid.NewGuid(),
            userId: refreshTokenEntity.User!.Id,
            hasedToken: tokenHasher.HashToken(refreshToken),
            device: refreshTokenEntity.Device,
            ipAddress: refreshTokenEntity.IpAddress,
            expiresAt: DateTime.UtcNow.AddDays(refreshTokenExpirationDays),
            createdAt: DateTime.UtcNow
        );
        var addResult = await refreshTokenRepo.AddAsync(refreseTokenEntity, cancellationToken);
        var saveResult = await refreshTokenRepo.SaveChangesAsync(cancellationToken);

        if (!(addResult.IsSuccess && saveResult.IsSuccess))
        {
            logger.Error("Failed to save new refresh token for user {@UserId}", refreshTokenEntity.User!.Id);
            return Result.Error(new ErrorList(addResult.Errors));
        }

        return Result.Success(new LoginDtoResult(
            AccessToken: accessToken.Value,
            RefreshToken: refreshToken
        ));
    }
}
