using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using PortfolioHub.Users.Domain.Entities;
using PortfolioHub.Users.Infrastructure.Context;
using Serilog;

namespace PortfolioHub.Users.Infrastructure.EFRepository;

internal sealed class EFRefreshTokenRepo(
    UsersDbContext usersDbContext,
    ILogger logger
    ) : IEntityRepo<RefreshToken>
{
    public async Task<Result> AddAsync(RefreshToken project, CancellationToken cancellationToken = default)
    {
        try
        {
            await usersDbContext.RefreshTokens.AddAsync(project, cancellationToken);
            logger.Information("Added RefreshToken with Id: {RefreshTokenId}", project.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error adding RefreshToken with Id: {RefreshTokenId}", project.Id);
            return Result.Error(new ErrorList([ex.Message]));
        }
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var token = await usersDbContext.RefreshTokens.FindAsync([id], cancellationToken);
            if (token is null)
            {
                logger.Warning("Attempted to delete non-existent RefreshToken with Id: {RefreshTokenId}", id);
                return Result.NotFound();
            }
            usersDbContext.RefreshTokens.Remove(token);
            logger.Information("Deleted RefreshToken with Id: {RefreshTokenId}", id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error deleting RefreshToken with Id: {RefreshTokenId}", id);
            return Result.Error(new ErrorList([ex.Message]));
        }
    }

    public async Task<Result<bool>> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var exists = await usersDbContext.RefreshTokens.AnyAsync(x => x.Id == id, cancellationToken);
            logger.Information("Checked existence for RefreshToken with Id: {RefreshTokenId}. Exists: {Exists}", id, exists);
            return Result.Success(exists);
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error checking existence for RefreshToken with Id: {RefreshTokenId}", id);
            return Result.Error(new ErrorList([ex.Message]));
        }
    }

    public async Task<Result<IReadOnlyList<RefreshToken>>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        if (pageNumber < 1 || pageSize < 1)
        {
            logger.Warning("Invalid pagination parameters: PageNumber={PageNumber}, PageSize={PageSize}", pageNumber, pageSize);
            return Result.Error(new ErrorList(["Page number and size must be greater than zero."]));
        }

        try
        {
            var tokens = await usersDbContext.RefreshTokens
                .OrderBy(x => x.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            logger.Information("Retrieved {Count} RefreshTokens for PageNumber={PageNumber}, PageSize={PageSize}", tokens.Count, pageNumber, pageSize);
            return Result.Success((IReadOnlyList<RefreshToken>)tokens);
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error retrieving RefreshTokens for PageNumber={PageNumber}, PageSize={PageSize}", pageNumber, pageSize);
            return Result.Error(new ErrorList([ex.Message]));
        }
    }

    public async Task<Result<RefreshToken>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var token = await usersDbContext.RefreshTokens.FindAsync([id], cancellationToken);
            if (token is null)
            {
                logger.Warning("RefreshToken not found with Id: {RefreshTokenId}", id);
                return Result.NotFound();
            }
            logger.Information("Retrieved RefreshToken with Id: {RefreshTokenId}", id);
            return Result.Success(token);
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error retrieving RefreshToken with Id: {RefreshTokenId}", id);
            return Result.Error(new ErrorList([ex.Message]));
        }
    }

    public async Task<Result<IReadOnlyList<RefreshToken>>> IsEntitiesIdValidAsync(IList<Guid> ids, CancellationToken cancellationToken = default)
    {
        try
        {
            var tokens = await usersDbContext.RefreshTokens
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(cancellationToken);

            if (tokens.Count != ids.Count)
            {
                logger.Warning("Some RefreshToken Ids are invalid. Requested: {RequestedCount}, Found: {FoundCount}", ids.Count, tokens.Count);
                return Result.NotFound();
            }

            logger.Information("Validated {Count} RefreshToken Ids", tokens.Count);
            return Result.Success((IReadOnlyList<RefreshToken>)tokens);
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error validating RefreshToken Ids");
            return Result.Error(new ErrorList([ex.Message]));
        }
    }

    public async Task<Result<RefreshToken>> IsEntitiyIdValidAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var token = await usersDbContext.RefreshTokens.FindAsync([id], cancellationToken);
            if (token is null)
            {
                logger.Warning("RefreshToken not found during validation with Id: {RefreshTokenId}", id);
                return Result.NotFound();
            }
            logger.Information("Validated RefreshToken Id: {RefreshTokenId}", id);
            return Result.Success(token);
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error validating RefreshToken Id: {RefreshTokenId}", id);
            return Result.Error(new ErrorList([ex.Message]));
        }
    }

    public async Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await usersDbContext.SaveChangesAsync(cancellationToken);
            logger.Information("Saved changes to the database.");
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error saving changes to the database.");
            return Result.Error(new ErrorList([ex.Message]));
        }
    }

    public async Task<Result> UpdateAsync(RefreshToken project, CancellationToken cancellationToken = default)
    {
        try
        {
            var exists = await usersDbContext.RefreshTokens.AnyAsync(x => x.Id == project.Id, cancellationToken);
            if (!exists)
            {
                logger.Warning("Attempted to update non-existent RefreshToken with Id: {RefreshTokenId}", project.Id);
                return Result.NotFound();
            }

            usersDbContext.RefreshTokens.Update(project);
            logger.Information("Updated RefreshToken with Id: {RefreshTokenId}", project.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error updating RefreshToken with Id: {RefreshTokenId}", project.Id);
            return Result.Error(new ErrorList([ex.Message]));
        }
    }
}
