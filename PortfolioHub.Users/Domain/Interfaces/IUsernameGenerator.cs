using Ardalis.Result;

namespace PortfolioHub.Users.Domain.Interfaces;

internal interface IUsernameGenerator
{
    Task<Result<string>> GenerateUniqueUsernameAsync(
        string email,
        string firstName,
        string lastName,
        CancellationToken cancellationToken = default);
}
