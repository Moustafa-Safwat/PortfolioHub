using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Interfaces;
using PortfolioHub.Projects.Endpoints.Project;

namespace PortfolioHub.Projects.Usecases.Project;

internal sealed class GetProjectsQueryHandler(
    IProjectsRepo projectsRepo
    ) : IRequestHandler<GetProjectsQuery, Result<GetProjectsQueryResponse>>
{
    public async Task<Result<GetProjectsQueryResponse>> Handle(GetProjectsQuery request,
        CancellationToken cancellationToken)
    {
        var projectsRes = await projectsRepo.GetAllAsync(request.PageNumber, request.PageSize,
            request.CategoryId, request.Search, request.isFeatured, cancellationToken);

        var countRes = await projectsRepo.GetTotalCount(cancellationToken);

        if (!(projectsRes.IsSuccess && countRes.IsSuccess))
            return Result.Error(new ErrorList(projectsRes.Errors));

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
                p.Links?.FirstOrDefault(l => l.Url.Contains("github.com", StringComparison.OrdinalIgnoreCase) ||
                l.LinkProvider.BaseUrl.Contains("github", StringComparison.OrdinalIgnoreCase))?.Url ?? string.Empty)
            )
            .ToList()
            .AsReadOnly();


        var response = new GetProjectsQueryResponse
        (
            projectDtos,
            countRes.Value
        );
        return Result.Success<GetProjectsQueryResponse>(response);
    }
}
