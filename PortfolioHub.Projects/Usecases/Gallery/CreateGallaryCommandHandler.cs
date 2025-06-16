using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Interfaces;
using Serilog;

namespace PortfolioHub.Projects.Usecases.Gallery;

internal sealed class CreateGallaryCommandHandler(
    SharedKernal.Domain.Interfaces.IEntityRepo<Domain.Entities.Gallery> repo,
    ILogger logger
    ) : IRequestHandler<CreateGallaryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateGallaryCommand request, CancellationToken cancellationToken)
    {
        logger.Information("Handling CreateGallaryCommand for ImageUrl: {ImageUrl}, Order: {Order}", request.ImageUrl, request.Order);

        Domain.Entities.Gallery gallery = new Domain.Entities.Gallery(
            Guid.NewGuid(),
            request.ImageUrl,
            request.Order
        );

        var addResult = await repo.AddAsync(gallery, cancellationToken);
        if (!addResult.IsSuccess)
        {
            logger.Error("Failed to add Gallery entity. Errors: {Errors}", string.Join(", ", addResult.Errors));
            return Result.Error("Failed to create gallery item.");
        }

        var saveResult = await repo.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
        {
            logger.Error("Failed to save Gallery entity. Errors: {Errors}", string.Join(", ", saveResult.Errors));
            return Result.Error("Failed to create gallery item.");
        }

        logger.Information("Successfully created Gallery entity with Id: {GalleryId}", gallery.Id);
        return Result.Success(gallery.Id);
    }
}
