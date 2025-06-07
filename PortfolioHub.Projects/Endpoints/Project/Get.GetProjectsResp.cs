namespace PortfolioHub.Projects.Endpoints.Project;

internal sealed record GetProjectsResp(
    IReadOnlyList<ProjectDto> Projects,
    int TotalCount,
    int PageNumber,
    int PageSize
);
internal sealed record ProjectDto(
    Guid Id,
    string Title,
    string Description,
    DateTime CreatedAt
);