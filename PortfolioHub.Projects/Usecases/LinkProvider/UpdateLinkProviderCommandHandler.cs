using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Interfaces;
using Serilog;

namespace PortfolioHub.Projects.Usecases.LinkProvider;

internal sealed class UpdateLinkProviderCommandHandler(
    IEntityRepo<Domain.Entities.LinkProvider> linkProviderRepo,
    ILogger logger
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
        {
            logger.Error("Failed to update LinkProvider with Id {LinkProviderId}: {ErrorMessage}", request.Id, updateResult.Errors?.FirstOrDefault() ?? "Unknown error");
            return Result.Error(new ErrorList(updateResult.Errors?.ToArray() ?? ["Failed to update LinkProvider."]));
        }

        var saveResult = await linkProviderRepo.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
        {
            logger.Error("Failed to save changes for LinkProvider with Id {LinkProviderId}: {ErrorMessage}", request.Id, saveResult.Errors?.FirstOrDefault() ?? "Unknown error");
            return Result.Error(new ErrorList(saveResult.Errors?.ToArray() ?? ["Failed to save changes for LinkProvider."]));
        }

        logger.Information("Successfully updated LinkProvider with Id {LinkProviderId}", request.Id);
        return Result.Success();
    }
}