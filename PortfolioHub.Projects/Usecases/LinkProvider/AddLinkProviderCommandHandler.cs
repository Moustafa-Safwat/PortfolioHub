using Ardalis.Result;
using MediatR;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using Serilog;

namespace PortfolioHub.Projects.Usecases.LinkProvider;

internal sealed class AddLinkProviderCommandHandler(
    IEntityRepo<Domain.Entities.LinkProvider> linkProviderRepo,
    ILogger logger
    ) : IRequestHandler<AddLinkProviderCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddLinkProviderCommand request, CancellationToken cancellationToken)
    {
        logger.Information("Starting AddLinkProviderCommand for Name: {Name}, Url: {Url}", request.Name, request.Url);

        var linkProviderId = Guid.NewGuid();
        var linkProvider = new Domain.Entities.LinkProvider(
            linkProviderId,
            request.Name,
            request.Url
        );

        var addLinkProviderResult = await linkProviderRepo.AddAsync(linkProvider, cancellationToken);
        if (!addLinkProviderResult.IsSuccess)
        {
            logger.Error("Failed to add LinkProvider. Errors: {@Errors}", addLinkProviderResult.Errors);
            return Result<Guid>.Error(new ErrorList(addLinkProviderResult.Errors));
        }

        var saveResult = await linkProviderRepo.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
        {
            logger.Error("Failed to save LinkProvider changes. Errors: {@Errors}", saveResult.Errors);
            return Result<Guid>.Error(new ErrorList(saveResult.Errors));
        }

        logger.Information("Successfully added LinkProvider with Id: {Id}", linkProviderId);
        return Result<Guid>.Success(linkProviderId);
    }
}