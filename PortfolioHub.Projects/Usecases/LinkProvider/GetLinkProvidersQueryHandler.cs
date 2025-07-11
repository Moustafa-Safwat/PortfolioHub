using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Endpoints.LinkProvider;
using PortfolioHub.SharedKernal.Domain.Interfaces;

namespace PortfolioHub.Projects.Usecases.LinkProvider;

internal sealed class GetLinkProvidersQueryHandler(
    IEntityRepo<Domain.Entities.LinkProvider> linkProviderRepo
    ) : IRequestHandler<GetLinkProvidersQuery, Result<IEnumerable<LinkProviderDto>>>
{
    public async Task<Result<IEnumerable<LinkProviderDto>>> Handle(
        GetLinkProvidersQuery request,
        CancellationToken cancellationToken
        )
    {
        var linkProviderResult = await linkProviderRepo.GetAllAsync(1, int.MaxValue, cancellationToken);
        if (!linkProviderResult.IsSuccess)
            return Result<IEnumerable<LinkProviderDto>>.Error(new ErrorList(linkProviderResult.Errors));

        var dtos = linkProviderResult.Value
            .Select(lp => new LinkProviderDto(lp.Id, lp.Name, lp.BaseUrl));

        return Result.Success(dtos);
    }
}
