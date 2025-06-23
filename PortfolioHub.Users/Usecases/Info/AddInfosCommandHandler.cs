using Ardalis.Result;
using MediatR;
using PortfolioHub.Users.Domain.Interfaces;
using Serilog;

namespace PortfolioHub.Users.Usecases.Info;

internal sealed class AddInfosCommandHandler(
    IInfoRepo infoRepo,
    ILogger logger
    ) : IRequestHandler<AddInfosCommand, Result<Guid[]>>
{
    public async Task<Result<Guid[]>> Handle(AddInfosCommand request, CancellationToken cancellationToken)
    {
        logger.Information("Handling AddInfosCommand with {Count} infos", request.Infos.Count());

        var infoEntities = request.Infos
            .Select(info => new Domain.Entities.Info
                (
                    Guid.NewGuid(),
                    info.InfoKey,
                    info.InfoValue
                )
            ).ToList();

        logger.Debug("Prepared {Count} Info entities for insertion", infoEntities.Count);

        var result = await infoRepo.AddRangeAsync(infoEntities, cancellationToken);

        if (result.IsSuccess)
        {
            logger.Information("Successfully added {Count} infos. Ids: {@Ids}", result.Value.Length, result.Value);
        }
        else
        {
            logger.Error("Failed to add infos. Errors: {@Errors}", result.Errors);
        }

        return result;
    }
}