using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Interfaces;
using Serilog;

namespace PortfolioHub.Projects.Usecases.Gallery;

internal sealed class DeleteGalleryCommandHandler(
    SharedKernal.Domain.Interfaces.IEntityRepo<Domain.Entities.Gallery> repo,
    ILogger logger
    ) : IRequestHandler<DeleteGalleryCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteGalleryCommand request, CancellationToken cancellationToken)
    {
        logger.Information("Attempting to delete Gallery with ID {GalleryId}", request.Id);

        var deleteResult = await repo.DeleteAsync(request.Id, cancellationToken);
        if (!deleteResult.IsSuccess)
        {
            logger.Warning("Failed to delete Gallery with ID {GalleryId}. Errors: {Errors}", request.Id, string.Join(", ", deleteResult.Errors));
        }

        var saveResult = await repo.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
        {
            logger.Warning("Failed to save changes after deleting Gallery with ID {GalleryId}. Errors: {Errors}", request.Id, string.Join(", ", saveResult.Errors));
        }

        if (!(deleteResult.IsSuccess && saveResult.IsSuccess))
        {
            logger.Error("Delete operation for Gallery with ID {GalleryId} failed.", request.Id);
            return Result.Error(new ErrorList(deleteResult.Errors.Concat(saveResult.Errors)));
        }

        logger.Information("Gallery with ID {GalleryId} deleted successfully.", request.Id);
        return Result.Success($"Gallery with ID {request.Id} deleted successfully.");
    }
}
