using Ardalis.Result;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using PortfolioHub.Users.Domain.Entities.Users;

namespace PortfolioHub.Users.Domain.Interfaces;

internal interface IUserSecurityRepo
: IEntityRepo<UserSecurity>
{
    Task<Result<UserSecurity>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Result<ApplicationUser>> GetUserWithSecurityByIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
