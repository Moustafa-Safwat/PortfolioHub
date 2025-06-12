using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Interfaces;
using PortfolioHub.Projects.Endpoints.LinkProvider;
using Serilog;

namespace PortfolioHub.Projects.Usecases.LinkProvider;

internal sealed class GetLinkProvidersQueryHandler(
    IEntityRepo<Domain.Entities.LinkProvider> linkProviderRepo,
    ILogger logger
    ) : IRequestHandler<GetLinkProvidersQuery, Result<IEnumerable<LinkProviderDto>>>
{
    public async Task<Result<IEnumerable<LinkProviderDto>>> Handle(
        GetLinkProvidersQuery request,
        CancellationToken cancellationToken
        )
    {
        logger.Information("Handling GetLinkProvidersQuery...");

        var linkProviderResult = await linkProviderRepo.GetAllAsync(1, int.MaxValue, cancellationToken);
        if (!linkProviderResult.IsSuccess)
        {
            logger.Warning("Failed to retrieve link providers: {@Errors}", linkProviderResult.Errors);
            return Result<IEnumerable<LinkProviderDto>>.Error(new ErrorList(linkProviderResult.Errors));
        }

        var dtos = linkProviderResult.Value
            .Select(lp => new LinkProviderDto(lp.Id, lp.Name, lp.BaseUrl));

        logger.Information("Successfully retrieved {Count} link providers.", dtos.Count());

        return Result.Success(dtos);
    }
}
