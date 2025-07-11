using Ardalis.Result;
using MediatR;
using PortfolioHub.SharedKernal.Domain.Interfaces;

namespace PortfolioHub.Projects.Usecases.LinkProvider;

internal sealed class UpdateLinkProviderCommandHandler(
    IEntityRepo<Domain.Entities.LinkProvider> linkProviderRepo
    ) : IRequestHandler<UpdateLinkProviderCommand, Result>
{
    public async Task<Result> Handle(UpdateLinkProviderCommand request, CancellationToken cancellationToken)
    {
        var linkProvider = new Domain.Entities.LinkProvider(
            request.Id,
            request.Name,
            request.BaseUrl);

        var updateResult = await linkProviderRepo.UpdateAsync(linkProvider, cancellationToken);
        if (!updateResult.IsSuccess)
            return Result.Error(new ErrorList(updateResult.Errors?.ToArray() ?? ["Failed to update LinkProvider."]));

        var saveResult = await linkProviderRepo.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
            return Result.Error(new ErrorList(saveResult.Errors?.ToArray() ?? ["Failed to save changes for LinkProvider."]));

        return Result.Success();
    }
}