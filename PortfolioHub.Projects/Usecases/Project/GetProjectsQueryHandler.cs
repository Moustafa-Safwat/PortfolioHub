using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Interfaces;
using PortfolioHub.Projects.Endpoints.Project;
using Serilog;

namespace PortfolioHub.Projects.Usecases.Project;

internal sealed class GetProjectsQueryHandler(
    IProjectsRepo projectsRepo,
    ILogger logger
    ) : IRequestHandler<GetProjectsQuery, Result<IReadOnlyList<ProjectsDto>>>
{
    public async Task<Result<IReadOnlyList<ProjectsDto>>> Handle(GetProjectsQuery request,
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
            new ProjectsDto
            (
                p.Id,
                p.Title,
                p.Description,
                p.CreatedDate,
                p.CoverImageUrl,
                p.Skills?.Take(4).Select(s => s.Name).ToArray() ?? [],
                p.Category?.Name ?? "",
                p.Links?.FirstOrDefault(l => l.LinkProvider.Name == "Github")?.Url ?? ""
            ))
            .ToList()
            .AsReadOnly();

        logger.Information("Successfully retrieved {Count} projects.", projectDtos.Count);

        return Result.Success<IReadOnlyList<ProjectsDto>>(projectDtos);
    }
}
