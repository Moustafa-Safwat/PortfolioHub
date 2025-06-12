using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Interfaces;

namespace PortfolioHub.Projects.Usecases.LinkProvider;

internal sealed class DeleteLinkProviderCommandHandler(
    IEntityRepo<Domain.Entities.LinkProvider> linkProviderRepo
    ) : IRequestHandler<DeleteLinkProviderCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(DeleteLinkProviderCommand request, CancellationToken cancellationToken)
    {
        var deleteResult = await linkProviderRepo.DeleteAsync(request.Id, cancellationToken);
        if (!deleteResult.IsSuccess)
        {
            return Result<Guid>.Error(new ErrorList(deleteResult.Errors));
        }
        var saveResult = await linkProviderRepo.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
        {
            return Result<Guid>.Error(new ErrorList(saveResult.Errors));
        }
        return Result<Guid>.Success(request.Id);
    }
}