using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using PortfolioHub.Projects.Domain.Interfaces;
using PortfolioHub.Projects.Endpoints.Project;

namespace PortfolioHub.Projects.Usecases.Project;

internal sealed class GetProjectByIdQueryHandler(
    IProjectsRepo projectsRepo,
    ILogger<GetProjectByIdQueryHandler> logger
    ) : IRequestHandler<GetProjectByIdQuery, Result<ProjectDto>>
{
    public async Task<Result<ProjectDto>> Handle(GetProjectByIdQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetProjectByIdQuery for ProjectId: {ProjectId}", request.Id);

        var projectRes = await projectsRepo.GetByIdAsync(request.Id, cancellationToken);
        if (!projectRes.IsSuccess)
        {
            logger.LogWarning("Failed to retrieve project with Id: {ProjectId}. Errors: {Errors}", request.Id, string.Join(", ", projectRes.Errors));
            return Result.Error(new ErrorList(projectRes.Errors));
        }
        var project = projectRes.Value;
        var projectDto = new ProjectDto(
            project.Id,
            project.Title,
            project.Description,
            project.CreatedDate
        );

        logger.LogInformation("Successfully retrieved project with Id: {ProjectId}", project.Id);
        return Result.Success(projectDto);
    }
}
