using Ardalis.Result;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using PortfolioHub.Users.Domain.Interfaces;

namespace PortfolioHub.Users.Infrastructure.GoogleAuth;

internal sealed class GooglePayloadValidator
(
    IConfiguration configuration
) : IGooglePayloadValidator
{
    public async Task<Result<GoogleJsonWebSignature.Payload>> ValidateAsync(string credential, CancellationToken cancellationToken)
    {
        try
        {
            var clientId = configuration["GoogleAuth:ClientId"];
            if (clientId is null)
                throw new ArgumentNullException(nameof(clientId));

            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = [clientId],
                IssuedAtClockTolerance = TimeSpan.FromMinutes(1),
                ExpirationTimeClockTolerance = TimeSpan.FromMinutes(1)
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(credential, settings);

            if (IsNotValid(payload))
            {
                var errors = new ErrorList(["Invalid Google payload"]);
                return Result.Error(errors);
            }

            if (!payload.EmailVerified)
            {
                var errors = new ErrorList(["Google email is not verified"]);
                return Result.Error(errors);
            }

            return Result.Success(payload);
        }
        catch
        {
            return Result.Error(new ErrorList(new[] { "Invalid Google credential" }));
        }
    }

    private bool IsNotValid(GoogleJsonWebSignature.Payload? payload)
        => payload is null ||
            string.IsNullOrWhiteSpace(payload.Subject) ||
            string.IsNullOrWhiteSpace(payload.Email);
}
