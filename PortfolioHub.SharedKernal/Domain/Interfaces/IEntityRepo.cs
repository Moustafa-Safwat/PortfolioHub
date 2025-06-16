using Ardalis.Result;
using PortfolioHub.SharedKernal.Domain.Entities;

namespace PortfolioHub.SharedKernal.Domain.Interfaces;


public interface IReadOnlyEntityRepo<TEntity>
    where TEntity : BaseEntity
{
    Task<Result<TEntity>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<TEntity>>> GetAllAsync(int pageNumber, int pageSize,
        CancellationToken cancellationToken = default);
    Task<Result<bool>> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<TEntity>>> IsEntitiesIdValidAsync(IList<Guid> ids,
    CancellationToken cancellationToken = default);
    Task<Result<TEntity>> IsEntitiyIdValidAsync(Guid id,
    CancellationToken cancellationToken = default);
}
public interface IEntityRepo<TEntity>
    : IReadOnlyEntityRepo<TEntity>
    where TEntity : BaseEntity
{
    Task<Result> AddAsync(TEntity project, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(TEntity project, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default);
}