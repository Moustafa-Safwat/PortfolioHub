using Ardalis.Result;
using MediatR;

namespace PortfolioHub.Projects.Usecases.Gallery;

internal sealed class DeleteGalleryCommandHandler(
    SharedKernal.Domain.Interfaces.IEntityRepo<Domain.Entities.Gallery> repo
    ) : IRequestHandler<DeleteGalleryCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteGalleryCommand request, CancellationToken cancellationToken)
    {

        var deleteResult = await repo.DeleteAsync(request.Id, cancellationToken);
        var saveResult = await repo.SaveChangesAsync(cancellationToken);

        if (!(deleteResult.IsSuccess && saveResult.IsSuccess))
            return Result.Error(new ErrorList(deleteResult.Errors.Concat(saveResult.Errors)));

        return Result.Success($"Gallery with ID {request.Id} deleted successfully.");
    }
}
