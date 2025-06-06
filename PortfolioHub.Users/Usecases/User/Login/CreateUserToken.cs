using System.Security.Claims;
using Ardalis.Result;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace PortfolioHub.Users.Usecases.User.Login;

internal sealed class CreateUserToken(
    UserManager<IdentityUser> userManager,
    IConfiguration config,
    ILogger logger)
{
    public async Task<Result<string>> CreateToken(IdentityUser user, CancellationToken cancellationToken)
    {
        string? userEmail = await userManager.GetEmailAsync(user);
        if (userEmail is null)
        { // it's a must the user has an email
            logger.Error("User {@UserId} doesn't have an email", user.Id);
            return Result.Unauthorized("User email is not found");
        }

        var jwtSecret = config["Auth:JwtSecret"];
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.NameIdentifier, user.Id!),
        };

        var token = JwtBearer.CreateToken(options =>
        {
            options.SigningKey = jwtSecret!;
            options.ExpireAt = DateTime.Now.AddMinutes(15);
            options.User.Claims.AddRange(claims);
        });


        return Result.Success(token);
    }
}