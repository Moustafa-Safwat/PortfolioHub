using Ardalis.Result;
using MediatR;
using PortfolioHub.SharedKernal.Domain.Interfaces;

namespace PortfolioHub.Projects.Usecases.LinkProvider;

internal sealed class AddLinkProviderCommandHandler(
    IEntityRepo<Domain.Entities.LinkProvider> linkProviderRepo
    ) : IRequestHandler<AddLinkProviderCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddLinkProviderCommand request, CancellationToken cancellationToken)
    {
        var linkProviderId = Guid.NewGuid();
        var linkProvider = new Domain.Entities.LinkProvider(
            linkProviderId,
            request.Name,
            request.Url
        );

        var addLinkProviderResult = await linkProviderRepo.AddAsync(linkProvider, cancellationToken);
        if (!addLinkProviderResult.IsSuccess)
            return Result<Guid>.Error(new ErrorList(addLinkProviderResult.Errors));

        var saveResult = await linkProviderRepo.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
            return Result<Guid>.Error(new ErrorList(saveResult.Errors));

        return Result<Guid>.Success(linkProviderId);
    }
}