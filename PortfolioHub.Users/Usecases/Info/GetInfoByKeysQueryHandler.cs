using Ardalis.Result;
using MediatR;
using PortfolioHub.Users.Domain.Interfaces;
using PortfolioHub.Users.Endpoints.Info;
using Serilog;

namespace PortfolioHub.Users.Usecases.Info;

internal sealed class GetInfoByKeysQueryHandler(
    IInfoRepo infoRepository,
    ILogger logger
    ) : IRequestHandler<GetInfoByKeysQuery, Result<IEnumerable<InfoGetDto>>>
{
    public async Task<Result<IEnumerable<InfoGetDto>>> Handle(GetInfoByKeysQuery request, CancellationToken cancellationToken)
    {
        logger.Information("Handling GetInfoByKeysQuery for keys: {Keys}", string.Join(", ", request.Keys));

        var infosResult = await infoRepository.GetByKeysAsync(request.Keys, cancellationToken);
        if (!infosResult.IsSuccess)
        {
            logger.Warning("GetByKeysAsync failed with validation errors: {Errors}", string.Join("; ", infosResult.ValidationErrors ?? []));
            return Result.Invalid(infosResult.ValidationErrors);
        }

        var infos = infosResult.Value;
        if (infos == null || !infos.Any())
        {
            logger.Information("No information found for the provided keys: {Keys}", string.Join(", ", request.Keys));
            return Result.NotFound(["No information found for the provided keys."]);
        }

        var infoDtos = infos.Select(info => new InfoGetDto(info.Id, info.InfoKey, info.InfoValue));
        logger.Information("Successfully retrieved {Count} info records for keys: {Keys}", infos.Count, string.Join(", ", request.Keys));
        return Result.Success(infoDtos);
    }
}
