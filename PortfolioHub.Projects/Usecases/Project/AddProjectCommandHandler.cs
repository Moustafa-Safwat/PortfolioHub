using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Interfaces;
using Serilog;

namespace PortfolioHub.Projects.Usecases.Project;

internal sealed class AddProjectCommandHandler(
    IProjectsRepo projectsRepo,
    ILogger logger
    ) : IRequestHandler<AddProjectCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddProjectCommand request, CancellationToken cancellationToken)
    {
        logger.Information("Attempting to add a new project with Title: {Title}", request.Title);

        var projectEntity = new Domain.Entities.Project(
            Guid.NewGuid(),
            request.Title,
            request.Description,
            request.VideoUrl ?? "",
            request.CreatedAt
        );

        var addResult = await projectsRepo.AddAsync(projectEntity, cancellationToken);

        if (addResult.IsSuccess)
        {
            logger.Information("Project entity added to repository. Attempting to save changes. ProjectId: {ProjectId}", projectEntity.Id);
            var saveResult = await projectsRepo.SaveChangesAsync(cancellationToken);
            if (saveResult.IsSuccess)
            {
                logger.Information("Project successfully added and saved. ProjectId: {ProjectId}", projectEntity.Id);
                return Result.Success(projectEntity.Id);
            }
            else
            {
                logger.Error("Failed to save changes after adding project. ProjectId: {ProjectId}", projectEntity.Id);
            }
        }
        else
        {
            logger.Error("Failed to add project entity to repository. Title: {Title}", request.Title);
        }

        return Result.Error("Failed to add project.");
    }
}