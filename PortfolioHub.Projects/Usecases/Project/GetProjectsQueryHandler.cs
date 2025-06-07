using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Interfaces;
using PortfolioHub.Projects.Endpoints.Project;
using Serilog;

namespace PortfolioHub.Projects.Usecases.Project;

internal sealed class GetProjectsQueryHandler(
    IProjectsRepo projectsRepo,
    ILogger logger
    ) : IRequestHandler<GetProjectsQuery, Result<IReadOnlyList<ProjectDto>>>
{
    public async Task<Result<IReadOnlyList<ProjectDto>>> Handle(GetProjectsQuery request,
        CancellationToken cancellationToken)
    {
        logger.Information("Handling GetProjectsQuery: PageNumber={PageNumber}, PageSize={PageSize}", request.PageNumber, request.PageSize);

        var projectsRes = await projectsRepo.GetAllAsync(request.PageNumber, request.PageSize, cancellationToken);
        if (!projectsRes.IsSuccess)
        {
            logger.Warning("Failed to retrieve projects. Errors: {@Errors}", projectsRes.Errors);
            return Result.Error(new ErrorList(projectsRes.Errors));
        }

        var projectDtos = projectsRes.Value
            .Select(p =>
            new ProjectDto
            (
                p.Id,
                p.Title,
                p.Description,
                p.CreatedDate
            ))
            .ToList()
            .AsReadOnly();

        logger.Information("Successfully retrieved {Count} projects.", projectDtos.Count);

        return Result.Success<IReadOnlyList<ProjectDto>>(projectDtos);
    }
}
