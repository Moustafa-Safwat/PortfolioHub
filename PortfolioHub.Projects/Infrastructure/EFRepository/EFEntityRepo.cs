using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using PortfolioHub.Projects.Domain.Entities;
using PortfolioHub.Projects.Domain.Interfaces;
using PortfolioHub.Projects.Infrastructure.Context;

namespace PortfolioHub.Projects.Infrastructure.EFRepository;

internal class EFEntityRepo<TEntity>(
    ProjectsDbContext dbContext
    ) : IEntityRepo<TEntity> where TEntity : BaseEntity
{

    public async Task<Result> AddAsync(TEntity entity,
        CancellationToken cancellationToken = default)
    {
        if (entity == null) return Result.Invalid(
            new ValidationError
            {
                ErrorMessage = "Entity cannot be null."
            });

        await dbContext.Set<TEntity>()
            .AddAsync(entity, cancellationToken);
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await dbContext
            .Set<TEntity>()
            .FindAsync([id], cancellationToken);

        if (entity == null) return Result.NotFound();

        dbContext.
            Set<TEntity>()
            .Remove(entity);

        return Result.Success();
    }

    public async Task<Result<bool>> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var exists = await dbContext
            .Set<TEntity>()
            .AnyAsync(e => e.Id == id, cancellationToken);

        return Result.Success(exists);
    }

    public async Task<Result<IReadOnlyList<TEntity>>> GetAllAsync(int pageNumber, int pageSize,
        CancellationToken cancellationToken = default)
    {
        if (pageNumber < 1 || pageSize < 1)
            return Result.Invalid(
                new ValidationError
                {
                    ErrorMessage = "Page number and size must be greater than zero."
                });

        var items = await dbContext.Set<TEntity>()
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return Result.Success((IReadOnlyList<TEntity>)items);
    }

    public async Task<Result<TEntity>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.Set<TEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        if (entity == null) return Result.NotFound();

        return Result.Success(entity);
    }

    public async Task<Result> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity == null) return Result.Invalid(
            new ValidationError
            {
                ErrorMessage = "Entity cannot be null."
            });

        var exists = await dbContext.Set<TEntity>()
            .AnyAsync(e => e.Id == entity.Id, cancellationToken);

        if (!exists) return Result.NotFound();

        dbContext.Set<TEntity>().Update(entity);
        return Result.Success();
    }

    public async Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await dbContext.SaveChangesAsync(cancellationToken);

        if (result > 0)
            return Result.Success();
        else
            return Result.Error("No changes were made to the database.");
    }
}
