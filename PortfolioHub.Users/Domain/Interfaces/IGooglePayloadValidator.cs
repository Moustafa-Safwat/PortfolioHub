using Ardalis.Result;
using Google.Apis.Auth;

namespace PortfolioHub.Users.Domain.Interfaces;

internal interface IGooglePayloadValidator
{
    Task<Result<GoogleJsonWebSignature.Payload>> ValidateAsync(string credential, CancellationToken cancellationToken);
}
