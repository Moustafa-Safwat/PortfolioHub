using Ardalis.Result;
using PortfolioHub.Users.Domain.Entities;

namespace PortfolioHub.Users.Domain.Interfaces;

internal interface IInfoRepo
{
    Task<Result<IReadOnlyList<Info>>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<Info>>> GetByKeysAsync(IEnumerable<string> infoKeys, CancellationToken cancellationToken = default);
    Task<Result<Guid[]>> AddRangeAsync(IEnumerable<Info> infos, CancellationToken cancellationToken = default);
    Task<Result> UpdateRangeAsync(IEnumerable<Info> infos, CancellationToken cancellationToken = default);
    Task<Result> DeleteRangeAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
    Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default);
}
