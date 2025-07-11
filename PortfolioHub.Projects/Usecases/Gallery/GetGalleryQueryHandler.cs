using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Endpoints.Gallery;
using PortfolioHub.SharedKernal.Domain.Interfaces;

namespace PortfolioHub.Projects.Usecases.Gallery;

internal sealed class GetGalleryQueryHandler(
    IEntityRepo<Domain.Entities.Gallery> repo
    ) : IRequestHandler<GetGalleryQuery, Result<IReadOnlyList<GalleryDto>>>
{
    public async Task<Result<IReadOnlyList<GalleryDto>>> Handle(GetGalleryQuery request,
        CancellationToken cancellationToken)
    {
        var getGalleries = await repo.GetAllAsync(request.PageNumber, request.PageSize, cancellationToken);
        if (!getGalleries.IsSuccess)
            return Result.Error(new ErrorList(getGalleries.Errors));

        var galleryDtos = getGalleries.Value
            .Select(g => new GalleryDto(
                g.Id,
                g.ImageUrl,
                g.Order
            ))
            .ToList()
            .AsReadOnly() as IReadOnlyList<GalleryDto>;

        return Result.Success(galleryDtos);
    }
}
