using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using PortfolioHub.Users.Domain.Entities.Users;
using PortfolioHub.Users.Domain.Interfaces;

namespace PortfolioHub.Users.Usecases.User.Login;

internal sealed record LoginDtoResult(string AccessToken, string RefreshToken);
internal sealed class LoginCommandHandler
(
    UserManager<ApplicationUser> userManager,
    JwtService jwtService,
    TokenHasher tokenHasher,
    IRefreshTokenRepo refreshTokenRepo,
    IUserSecurityRepo userSecurityRepo,
    IConfiguration configuration,
    IValidUser validUser
) : IRequestHandler<LoginCommand, Result<LoginDtoResult>>
{

    public async Task<Result<LoginDtoResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.UserEmail);
        if (user is null)
            return Result.NotFound($"User with email: {request.UserEmail} not found");

        var validUserResult = await validUser.IsValidUserAsync(user, cancellationToken);
        if (!validUserResult.IsSuccess)
            return Result.Unauthorized("Invalid user");

        var applicationUserWithSecurity = validUserResult.Value;

        var isPassValid = await userManager.CheckPasswordAsync(user, request.Password);
        if (!isPassValid)
            return Result.Unauthorized("Invalid password");

        var isEmailConfirmed = await userManager.IsEmailConfirmedAsync(user);
        if (!isEmailConfirmed)
            return Result.Error(new ErrorList([
            "Your email address has not been confirmed. " +
            "Please check your inbox and verify your email before signing in."]));

        // Additional logic for successful login can be added here
        var tokenResult = await jwtService.GenerateAccessTokenAsync(user, cancellationToken);
        if (!tokenResult.IsSuccess)
            return Result.Error(new ErrorList(tokenResult.Errors));

        var refreshToken = (await jwtService.GenerateRefreshTokenAsync(user, cancellationToken)).Value;

        if (!double.TryParse(configuration["Auth:RefreshTokenExpirationDays"], out double refreshTokenExpirationDays))
            throw new InvalidOperationException("Refresh token expiration time is not configured or is invalid.");

        var hashedRefreshToken = tokenHasher.HashToken(refreshToken);

        var refreseTokenEntity = new Domain.Entities.Users.RefreshToken(
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
            return Result.Error(new ErrorList(addRefreshTokenResult.Errors));

        var saveRefreshTokenResult = await refreshTokenRepo.SaveChangesAsync(cancellationToken);
        if (!saveRefreshTokenResult.IsSuccess)
            return Result.Error(new ErrorList(saveRefreshTokenResult.Errors));

        // Record the last login time for the user in the UserSecurity entity
        applicationUserWithSecurity?.UserSecurity?.RecordLogin();
        await userSecurityRepo.SaveChangesAsync(cancellationToken);

        var loginDto = new LoginDtoResult(
            AccessToken: tokenResult.Value,
            RefreshToken: refreshToken
        );

        return Result.Success(loginDto);
    }
}
