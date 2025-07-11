using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Entities;
using PortfolioHub.Projects.Domain.Interfaces;
using PortfolioHub.Projects.Infrastructure;
using PortfolioHub.SharedKernal.Domain.Interfaces;

namespace PortfolioHub.Projects.Usecases.Project;

internal sealed class AddProjectCommandHandler(
    IProjectsRepo projectsRepo,
    IEntityRepo<Domain.Entities.LinkProvider> linkProviderRepo,
    IEntityRepo<TechanicalSkills> techinicalSkillsRepo,
    IEntityRepo<Domain.Entities.Category> categoryRepo,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<AddProjectCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddProjectCommand request, CancellationToken cancellationToken)
    {
        using (unitOfWork.BeginTransactionAsync(cancellationToken))
        {
            try
            {
                Guid projectId = Guid.NewGuid();

                var skillsIdList = request.SkillsId.ToList();
                var linksProviderIds = request.Links.Select(link => link.ProviderId).Distinct().ToList();

                var isSkillsIdValidResult = await techinicalSkillsRepo.IsEntitiesIdValidAsync(skillsIdList, cancellationToken);
                var isCategoryIdValidResult = await categoryRepo.IsEntitiyIdValidAsync(request.CategoryId, cancellationToken);
                var isLinksProvidersIdsValidResult = await linkProviderRepo.IsEntitiesIdValidAsync(linksProviderIds, cancellationToken);

                if (!isSkillsIdValidResult.IsSuccess)
                    return Result.Error("One or more provided skill IDs are invalid.");

                if (!isCategoryIdValidResult.IsSuccess)
                    return Result.Error("Provided category ID is invalid.");

                if (!isLinksProvidersIdsValidResult.IsSuccess)
                    return Result.Error("One or more provided link provider IDs are invalid.");

                // Build related entities
                var validLinkProviders = isLinksProvidersIdsValidResult.Value.ToDictionary(lp => lp.Id);

                var linksToAdd = request.Links
                    .Select(link =>
                    {
                        if (!validLinkProviders.TryGetValue(link.ProviderId, out var provider))
                            throw new InvalidOperationException("Link provider not found after validation.");
                        return new Links(Guid.NewGuid(), link.Link)
                            .SetLinkProvider(provider);
                    })
                    .ToList();

                var galleriesToAdd = request.ImagesUrls
                    .Select((imageUrl, index) => new Domain.Entities.Gallery(
                        Guid.NewGuid(),
                        imageUrl,
                        index + 1
                    ))
                    .ToList();

                var projectEntity = new Domain.Entities.Project(
                    projectId,
                    request.Title,
                    request.Description,
                    request.LongDescription,
                    request.VideoUrl ?? string.Empty,
                    request.CreatedDate,
                    request.CoverImageUrl ?? string.Empty,
                    request.IsFeatured
                );

                projectEntity.SetLinks(linksToAdd);
                projectEntity.SetTechanicalSkills(isSkillsIdValidResult.Value.ToList());
                projectEntity.SetCategory(isCategoryIdValidResult.Value);
                projectEntity.SetImages(galleriesToAdd);

                var addResult = await projectsRepo.AddAsync(projectEntity, cancellationToken);
                if (!addResult.IsSuccess)
                {
                    await unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return Result.Error("Failed to add project.");
                }

                await unitOfWork.CompleteAsync(cancellationToken);
                await unitOfWork.CommitTransactionAsync(cancellationToken);

                return Result.Success(projectId);
            }
            catch 
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return Result.Error("Failed to add project due to an unexpected error.");
            }
        }
    }
}