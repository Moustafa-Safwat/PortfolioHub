using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using PortfolioHub.Users.Domain.Entities.Users;
using PortfolioHub.Users.Domain.Interfaces;
using PortfolioHub.Users.Infrastructure.Context;

namespace PortfolioHub.Users.Infrastructure.EFRepository;

internal sealed class EFUserSecurityRepo(
    UsersDbContext usersDbContext
    ) : IUserSecurityRepo
{
    public async Task<Result> AddAsync(UserSecurity entity, CancellationToken cancellationToken = default)
    {
        try
        {
            await usersDbContext.UserSecurity.AddAsync(entity, cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Error(new ErrorList([ex.Message]));
        }
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var userSecurity = await usersDbContext.UserSecurity.FindAsync([id], cancellationToken);
            if (userSecurity is null)
            {
                return Result.NotFound();
            }
            usersDbContext.UserSecurity.Remove(userSecurity);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Error(new ErrorList([ex.Message]));
        }
    }

    public async Task<Result<bool>> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var exists = await usersDbContext.UserSecurity.AnyAsync(x => x.Id == id, cancellationToken);
            return Result.Success(exists);
        }
        catch (Exception ex)
        {
            return Result.Error(new ErrorList([ex.Message]));
        }
    }

    public async Task<Result<IReadOnlyList<UserSecurity>>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        if (pageNumber < 1 || pageSize < 1)
        {
            return Result.Error(new ErrorList(["Page number and size must be greater than zero."]));
        }

        try
        {
            var userSecurities = await usersDbContext.UserSecurity
                .OrderBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return Result.Success((IReadOnlyList<UserSecurity>)userSecurities);
        }
        catch (Exception ex)
        {
            return Result.Error(new ErrorList([ex.Message]));
        }
    }

    public async Task<Result<UserSecurity>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var userSecurity = await usersDbContext.UserSecurity.FindAsync([id], cancellationToken);
            if (userSecurity is null)
            {
                return Result.NotFound();
            }
            return Result.Success(userSecurity);
        }
        catch (Exception ex)
        {
            return Result.Error(new ErrorList([ex.Message]));
        }
    }

    public async Task<Result<UserSecurity>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var userSecurity = await usersDbContext.UserSecurity
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (userSecurity is null)
        {
            return Result.NotFound();
        }
        return userSecurity;
    }

    public async Task<Result<ApplicationUser>> GetUserWithSecurityByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var applicationUser = await usersDbContext.ApplicationUsers
            .Include(user => user.UserSecurity)
            .FirstOrDefaultAsync(user => user.Id == userId, cancellationToken);

        if (applicationUser is null)
        {
            return Result.NotFound("User is not found");
        }
        return applicationUser;
    }

    public async Task<Result<IReadOnlyList<UserSecurity>>> IsEntitiesIdValidAsync(IList<Guid> ids, CancellationToken cancellationToken = default)
    {
        try
        {
            var userSecurities = await usersDbContext.UserSecurity
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(cancellationToken);

            if (userSecurities.Count != ids.Count)
            {
                return Result.NotFound();
            }

            return Result.Success((IReadOnlyList<UserSecurity>)userSecurities);
        }
        catch (Exception ex)
        {
            return Result.Error(new ErrorList([ex.Message]));
        }
    }

    public async Task<Result<UserSecurity>> IsEntitiyIdValidAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var userSecurity = await usersDbContext.UserSecurity.FindAsync([id], cancellationToken);
            if (userSecurity is null)
            {
                return Result.NotFound();
            }
            return Result.Success(userSecurity);
        }
        catch (Exception ex)
        {
            return Result.Error(new ErrorList([ex.Message]));
        }
    }

    public async Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await usersDbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Error(new ErrorList([ex.Message]));
        }
    }

    public async Task<Result> UpdateAsync(UserSecurity entity, CancellationToken cancellationToken = default)
    {
        try
        {
            var exists = await usersDbContext.UserSecurity.AnyAsync(x => x.Id == entity.Id, cancellationToken);
            if (!exists)
            {
                return Result.NotFound();
            }

            usersDbContext.UserSecurity.Update(entity);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Error(new ErrorList([ex.Message]));
        }
    }
}

