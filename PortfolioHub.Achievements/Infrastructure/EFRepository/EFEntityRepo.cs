using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using PortfolioHub.Achievements.Infrastructure.Context;
using PortfolioHub.SharedKernal.Domain.Entities;
using PortfolioHub.SharedKernal.Domain.Interfaces;

namespace PortfolioHub.Projects.Infrastructure.EFRepository;

internal class EFEntityRepo<TEntity>(
    AchievementsDbContext dbContext
    ) : IEntityRepo<TEntity> where TEntity : BaseEntity
{
    protected readonly AchievementsDbContext dbContext = dbContext;

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
        try
        {
            var result = await dbContext.SaveChangesAsync(cancellationToken);

            if (result > 0)
                return Result.Success();
            else
                return Result.Error("No changes were made to the database.");
        }
        catch (DbUpdateException dbEx)
        {
            // Log dbEx if logging is available
            return Result.Error($"A database update error occurred: {dbEx.Message}");
        }
        catch (OperationCanceledException)
        {
            return Result.Error("The operation was canceled.");
        }
        catch (Exception ex)
        {
            // Log ex if logging is available
            return Result.Error($"An unexpected error occurred: {ex.Message}");
        }
    }

    public async Task<Result<IReadOnlyList<TEntity>>> IsEntitiesIdValidAsync(IList<Guid> ids,
        CancellationToken cancellationToken = default)
    {
        if (ids == null || ids.Count == 0)
        {
            return Result.Invalid(new ValidationError
            {
                ErrorMessage = "Ids list cannot be null or empty."
            });
        }

        var entities = await dbContext.Set<TEntity>()
            .Where(e => ids.Contains(e.Id))
            .ToListAsync(cancellationToken);

        if (entities.Count == 0)
            return Result.NotFound();

        return Result.Success((IReadOnlyList<TEntity>)entities);
    }

    public async Task<Result<TEntity>> IsEntitiyIdValidAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.Set<TEntity>()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        if (entity is null)
            return Result.NotFound();

        return Result.Success(entity);
    }
}
