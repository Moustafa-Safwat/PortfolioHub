using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using PortfolioHub.Projects.Domain.Interfaces;
using PortfolioHub.Projects.Endpoints.Project;
using PortfolioHub.Projects.Endpoints.TechanicalSkills;

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
            project.Title,
            project.Description,
            project.LongDescription,
            project.CreatedDate,
            new Endpoints.Category.CategoryDto(
                project.Category!.Id,
                project.Category.Name
            ),
            project.VideoUrl,
            project.CoverImageUrl,
            project.Images!.OrderBy(i => i.Order).Select(i => i.ImageUrl).ToArray(),
            project.Skills!.Select(s => new TechSkillDto(s.Id, s.Name)).ToArray(),
            project.Links!.Select(l => new LinkDto(
                l.LinkProvider!.Id.ToString(),
                l.Url,
                l.LinkProvider.Name,
                l.LinkProvider.BaseUrl
            )).ToArray()
        );

        logger.LogInformation("Successfully retrieved project with Id: {ProjectId}", project.Id);
        return Result.Success(projectDto);
    }
}
