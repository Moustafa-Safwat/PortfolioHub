using System.Security.Claims;
using System.Security.Cryptography;
using Ardalis.Result;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace PortfolioHub.Users.Usecases.User.Login;

internal sealed class JwtService(
    UserManager<IdentityUser> userManager,
    IConfiguration config,
    ILogger logger)
{
    public async Task<Result<string>> GenerateAccessTokenAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        logger.Information("Attempting to create token for user: {UserName}, Id: {UserId}", user.UserName, user.Id);

        var userRoles = await userManager.GetRolesAsync(user);

        string jwtSecret = config["Auth:JwtSecret"] ?? throw new InvalidOperationException("JWT secret is not configured.");
        if (!double.TryParse(config["Auth:AccessTokenExpirationMinutes"], out double accessTokenExpTime))
            throw new InvalidOperationException("Access token expiration time is not configured or is invalid.");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.NameIdentifier, user.Id!),
        };

        var token = JwtBearer.CreateToken(options =>
        {
            options.SigningKey = jwtSecret!;
            options.ExpireAt = DateTime.Now.AddMinutes(accessTokenExpTime);
            options.User.Claims.AddRange(claims);
            options.User.Roles.AddRange(userRoles);
        });

        logger.Information("Token successfully created for user: {UserName}, Id: {UserId}", user.UserName, user.Id);

        return Result.Success(token);
    }

    public Task<Result<string>> GenerateRefreshTokenAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        logger.Information("Generating refresh token for user: {UserName}, Id: {UserId}", user.UserName, user.Id);

        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        logger.Information("Refresh token generated for user: {UserName}, Id: {UserId}", user.UserName, user.Id);
        return Task.FromResult(Result.Success(refreshToken));
    }
}
