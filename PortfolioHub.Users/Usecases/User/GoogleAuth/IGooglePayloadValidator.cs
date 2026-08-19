using Ardalis.Result;
using Google.Apis.Auth;

namespace PortfolioHub.Users.Usecases.User.GoogleAuth;

internal interface IGooglePayloadValidator
{
    Task<Result<GoogleJsonWebSignature.Payload>> ValidateAsync(string credential, CancellationToken cancellationToken);
}
