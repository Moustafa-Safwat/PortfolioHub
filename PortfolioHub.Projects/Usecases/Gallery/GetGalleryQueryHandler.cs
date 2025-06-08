using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Interfaces;
using PortfolioHub.Projects.Endpoints.Gallery;
using Serilog;

namespace PortfolioHub.Projects.Usecases.Gallery;

internal sealed class GetGalleryQueryHandler(
    IEntityRepo<Domain.Entities.Gallery> repo,
    ILogger logger
    ) : IRequestHandler<GetGalleryQuery, Result<IReadOnlyList<GalleryDto>>>
{
    public async Task<Result<IReadOnlyList<GalleryDto>>> Handle(GetGalleryQuery request,
        CancellationToken cancellationToken)
    {
        logger.Information("Handling GetGalleryQuery: PageNumber={PageNumber}, PageSize={PageSize}", request.PageNumber, request.PageSize);

        var getGalleries = await repo.GetAllAsync(request.PageNumber, request.PageSize, cancellationToken);
        if (!getGalleries.IsSuccess)
        {
            logger.Warning("Failed to retrieve galleries: {@Errors}", getGalleries.Errors);
            return Result.Error(new ErrorList(getGalleries.Errors));
        }
        var galleryDtos = getGalleries.Value
            .Select(g => new GalleryDto(
                g.Id,
                g.ImageUrl,
                g.Order
            ))
            .ToList()
            .AsReadOnly() as IReadOnlyList<GalleryDto>;

        logger.Information("Successfully retrieved {Count} galleries.", galleryDtos.Count);
        return Result.Success(galleryDtos);
    }
}
