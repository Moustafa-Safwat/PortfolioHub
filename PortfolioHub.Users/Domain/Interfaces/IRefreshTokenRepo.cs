using Ardalis.Result;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using PortfolioHub.Users.Domain.Entities;

namespace PortfolioHub.Users.Domain.Interfaces;

interface IRefreshTokenRepo : IEntityRepo<RefreshToken>
{
    Task<Result<RefreshToken>> GetActiveRefreshTokenByHashedTokenAsync(string hashedToken,
        CancellationToken cancellationToken = default);

    Task<Result<RefreshToken>> GetActiveRefreshTokenByUserIdAsync(string userId,
    CancellationToken cancellationToken = default);
}
