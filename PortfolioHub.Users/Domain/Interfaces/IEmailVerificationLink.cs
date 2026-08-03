using Ardalis.Result;

namespace PortfolioHub.Users.Domain.Interfaces;

internal interface IEmailVerificationLink
{
    Result<string> Build(string userId, string encodedToken);
}
