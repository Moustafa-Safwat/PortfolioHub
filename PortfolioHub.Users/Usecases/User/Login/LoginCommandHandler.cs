using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using PortfolioHub.Users.Domain.Interfaces;
using Serilog;

namespace PortfolioHub.Users.Usecases.User.Login;

internal sealed record LoginDtoResult(string AccessToken, string RefreshToken);
internal sealed class LoginCommandHandler(
    UserManager<IdentityUser> userManager,
    ILogger logger,
    JwtService jwtService,
    TokenHasher tokenHasher,
    IRefreshTokenRepo refreshTokenRepo,
    IConfiguration configuration
    ) : IRequestHandler<LoginCommand, Result<LoginDtoResult>>
{

    public async Task<Result<LoginDtoResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(request.UserName);
        if (user is null)
        {
            logger.Error("User {@UserName} not found", request.UserName);
            return Result.NotFound($"User {request.UserName} not found");
        }
        var isPassValid = await userManager.CheckPasswordAsync(user, request.Password);
        if (!isPassValid)
        {
            logger.Error("Invalid password for user {@UserName}", request.UserName);
            return Result.Unauthorized("Invalid password");
        }
        // Additional logic for successful login can be added here
        var tokenResult = await jwtService.GenerateAccessTokenAsync(user, cancellationToken);
        if (!tokenResult.IsSuccess)
        {
            return Result.Error(new ErrorList(tokenResult.Errors));
        }

        var refreshToken = (await jwtService.GenerateRefreshTokenAsync(user, cancellationToken)).Value;

        if (!double.TryParse(configuration["Auth:RefreshTokenExpirationDays"], out double refreshTokenExpirationDays))
            throw new InvalidOperationException("Refresh token expiration time is not configured or is invalid.");

        var hashedRefreshToken = tokenHasher.HashToken(refreshToken);

        var refreseTokenEntity = new Domain.Entities.RefreshToken(
            id: Guid.NewGuid(),
            userId: user.Id,
            hasedToken: hashedRefreshToken,
            device: request.Device,
            ipAddress: request.IpAddress,
            expiresAt: DateTime.UtcNow.AddDays(refreshTokenExpirationDays),
            createdAt: DateTime.UtcNow
        );

        var addRefreshTokenResult = await refreshTokenRepo.AddAsync(refreseTokenEntity, cancellationToken);
        if (!addRefreshTokenResult.IsSuccess)
        {
            logger.Error("Failed to save refresh token for user {@UserId}", user.Id);
            return Result.Error(new ErrorList(addRefreshTokenResult.Errors));
        }
        var saveRefreshTokenResult = await refreshTokenRepo.SaveChangesAsync(cancellationToken);
        if (!saveRefreshTokenResult.IsSuccess)
        {
            logger.Error("Failed to save changes for refresh token for user {@UserId}", user.Id);
            return Result.Error(new ErrorList(saveRefreshTokenResult.Errors));
        }
        logger.Information("User {@UserId} logged in successfully", user.Id);
        var loginDto = new LoginDtoResult(
            AccessToken: tokenResult.Value,
            RefreshToken: refreshToken
        );

        return Result.Success(loginDto);
    }
}
