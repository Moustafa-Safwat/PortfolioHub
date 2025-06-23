using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using PortfolioHub.Users.Domain.Entities;
using PortfolioHub.Users.Domain.Interfaces;
using PortfolioHub.Users.Infrastructure.Context;

namespace PortfolioHub.Users.Infrastructure.EFRepository;

internal class EFInfoRepository (UsersDbContext dbContext) : IInfoRepo
{

    public async Task<Result> AddRangeAsync(IEnumerable<Info> infos, CancellationToken cancellationToken = default)
    {
        if (infos == null) return Result.Invalid(new List<ValidationError> { new("Infos", "Infos collection is null") });

        var infoList = infos.ToList();
        if (!infoList.Any())
            return Result.Invalid(new List<ValidationError> { new("Infos", "Infos collection is empty") });

        await dbContext.Infos.AddRangeAsync(infoList, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> DeleteRangeAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        if (ids == null) return Result.Invalid(new List<ValidationError> { new("Ids", "Ids collection is null") });

        var idList = ids.ToList();
        if (!idList.Any())
            return Result.Invalid(new List<ValidationError> { new("Ids", "Ids collection is empty") });

        var infos = await dbContext.Infos.Where(i => idList.Contains(i.Id)).ToListAsync(cancellationToken);
        if (!infos.Any())
            return Result.NotFound();

        dbContext.Infos.RemoveRange(infos);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result<IReadOnlyList<Info>>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        if (ids == null) return Result.Invalid(new List<ValidationError> { new("Ids", "Ids collection is null") });

        var idList = ids.ToList();
        if (!idList.Any())
            return Result.Invalid(new List<ValidationError> { new("Ids", "Ids collection is empty") });

        var infos = await dbContext.Infos.Where(i => idList.Contains(i.Id)).ToListAsync(cancellationToken);
        return Result.Success((IReadOnlyList<Info>)infos);
    }

    public async Task<Result<IReadOnlyList<Info>>> GetByKeysAsync(IEnumerable<string> infoKeys, CancellationToken cancellationToken = default)
    {
        if (infoKeys == null) return Result.Invalid(new List<ValidationError> { new("InfoKeys", "InfoKeys collection is null") });

        var keyList = infoKeys.Where(k => !string.IsNullOrWhiteSpace(k)).Distinct().ToList();
        if (!keyList.Any())
            return Result.Invalid(new List<ValidationError> { new("InfoKeys", "InfoKeys collection is empty or contains only null/whitespace") });

        var infos = await dbContext.Infos.Where(i => keyList.Contains(i.InfoKey)).ToListAsync(cancellationToken);
        return Result.Success((IReadOnlyList<Info>)infos);
    }

    public async Task<Result> UpdateRangeAsync(IEnumerable<Info> infos, CancellationToken cancellationToken = default)
    {
        if (infos == null) return Result.Invalid(new List<ValidationError> { new("Infos", "Infos collection is null") });

        var infoList = infos.ToList();
        if (!infoList.Any())
            return Result.Invalid(new List<ValidationError> { new("Infos", "Infos collection is empty") });

        var ids = infoList.Select(i => i.Id).ToList();
        var existingInfos = await dbContext.Infos.Where(i => ids.Contains(i.Id)).ToListAsync(cancellationToken);

        if (existingInfos.Count != infoList.Count)
            return Result.NotFound();

        foreach (var info in infoList)
        {
            var entity = existingInfos.FirstOrDefault(e => e.Id == info.Id);
            if (entity != null)
            {
                entity.UpdateInfo(info.InfoKey, info.InfoValue);
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
