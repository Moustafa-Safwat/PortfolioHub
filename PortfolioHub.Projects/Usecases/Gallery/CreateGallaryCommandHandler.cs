using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Interfaces;
using Serilog;

namespace PortfolioHub.Projects.Usecases.Gallery;

internal sealed class CreateGallaryCommandHandler(
    SharedKernal.Domain.Interfaces.IEntityRepo<Domain.Entities.Gallery> repo
    ) : IRequestHandler<CreateGallaryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateGallaryCommand request, CancellationToken cancellationToken)
    {

        Domain.Entities.Gallery gallery = new Domain.Entities.Gallery(
            Guid.NewGuid(),
            request.ImageUrl,
            request.Order
        );

        var addResult = await repo.AddAsync(gallery, cancellationToken);
        if (!addResult.IsSuccess)
            return Result.Error("Failed to create gallery item.");

        var saveResult = await repo.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
            return Result.Error("Failed to create gallery item.");

        return Result.Success(gallery.Id);
    }
}
