using Ardalis.Result;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using PortfolioHub.SharedKernal.Config;
using PortfolioHub.Users.Domain.Entities.Users;
using PortfolioHub.Users.Domain.Interfaces;
using PortfolioHub.Users.Usecases.User.Create;
using PortfolioHub.Users.Usecases.User.Login;
using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Users.Usecases.User.GoogleAuth;

internal sealed class GoogleAuthCommandHandler(
    UserManager<ApplicationUser> userManager,
    JwtService jwtService,
    TokenHasher tokenHasher,
    IRefreshTokenRepo refreshTokenRepo,
    IUserSecurityRepo userSecurityRepo,
    IGooglePayloadValidator googlePayloadValidator,
    IConfiguration configuration,
    ISender sender
) : ICommandHandler<GoogleAuthCommand, LoginDtoResult>
{
    private const string _provider = "Google";

    public async Task<Result<LoginDtoResult>> Handle(GoogleAuthCommand command, CancellationToken cancellationToken)
    {
        var payloadResult = await googlePayloadValidator.ValidateAsync(command.Credential, cancellationToken);
        if (!payloadResult.IsSuccess)
            return payloadResult.PropagateFailure<GoogleJsonWebSignature.Payload, LoginDtoResult>();

        var payload = payloadResult.Value;

        var user = await userManager.FindByLoginAsync(_provider, payload.Subject);

        if (user is null)
        {
            // NOTE: user may login with google account before with old way
            // and then login with google account using google way
            // so we need to check if user exists by email first

            // try to find user by email
            user = await userManager.FindByEmailAsync(payload.Email);

            if (user is null) // if user is still null, create a new user
            {
                var authUserRole = ApplicationUserRoles.User.ToString().ToLower();

                // Some times the the name comes from google as empty
                Func<string, string> getValue = (string name) =>
                {
                    return string.IsNullOrEmpty(name) ? "Unknown" : name;
                };

                var createUserCommand = new CreateUserCommand
                (
                    payload.Email,
                    $"A1@{Guid.NewGuid().ToString()}#9Z",
                    getValue(payload.GivenName),
                    getValue(payload.FamilyName),
                    authUserRole,
                    "UnKnown",
                    false,
                    payload.Picture
                );
                var createUserResult = await sender.Send(createUserCommand, cancellationToken);
                if (!createUserResult.IsSuccess)
                    return createUserResult.PropagateFailure<Guid, LoginDtoResult>();

                var userId = createUserResult.Value;
                var userSecurityResult = await userSecurityRepo.GetUserWithSecurityByIdAsync(userId, cancellationToken);
                if (!userSecurityResult.IsSuccess)
                    return userSecurityResult.PropagateFailure<ApplicationUser, LoginDtoResult>();

                user = userSecurityResult.Value;
            }

            var loginInfo = new UserLoginInfo(_provider, payload.Subject, _provider);
            var addLoginResult = await userManager.AddLoginAsync(user!, loginInfo);
            if (!addLoginResult.Succeeded)
            {
                var errors = new ErrorList
                (
                    addLoginResult.Errors
                    .Select(e => e.Description)
                    .ToArray()
                );
                return Result.Error(errors);
            }
        }

        // Generate tokens
        var accessTokenResult = await jwtService.GenerateAccessTokenAsync(user!, cancellationToken);
        if (!accessTokenResult.IsSuccess)
            return accessTokenResult.PropagateFailure<string, LoginDtoResult>();

        var refreshToken = (await jwtService.GenerateRefreshTokenAsync(user!, cancellationToken)).Value;

        if (!double.TryParse(configuration["Auth:RefreshTokenExpirationDays"], out double refreshTokenExpirationDays))
            throw new InvalidOperationException("Refresh token expiration time is not configured or is invalid.");

        var hashedRefreshToken = tokenHasher.HashToken(refreshToken);

        var refreshEntity = new Domain.Entities.Users.RefreshToken(
            id: Guid.NewGuid(),
            userId: user!.Id,
            hasedToken: hashedRefreshToken,
            device: "google",
            ipAddress: command.RemoteAddress,
            expiresAt: DateTime.UtcNow.AddDays(refreshTokenExpirationDays),
            createdAt: DateTime.UtcNow
        );

        var addResult = await refreshTokenRepo.AddAsync(refreshEntity, cancellationToken);
        var saveResult = await refreshTokenRepo.SaveChangesAsync(cancellationToken);

        if (!addResult.IsSuccess || !saveResult.IsSuccess)
            return Result.Error("Failed to save refresh token.");

        // Update last login
        var userWithSec = await userSecurityRepo.GetUserWithSecurityByIdAsync(user.Id, cancellationToken);
        if (userWithSec.IsSuccess)
        {
            userWithSec.Value.UserSecurity?.RecordLogin();
            await userSecurityRepo.SaveChangesAsync(cancellationToken);
        }

        var loginDto = new LoginDtoResult
        (
            AccessToken: accessTokenResult.Value,
            RefreshToken: refreshToken
        );

        return Result.Success(loginDto);
    }
}