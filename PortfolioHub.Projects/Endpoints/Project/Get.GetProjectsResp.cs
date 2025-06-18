namespace PortfolioHub.Projects.Endpoints.Project;

internal sealed record GetProjectsResp(
    IReadOnlyList<ProjectsDto> Projects,
    int TotalCount,
    int PageNumber,
    int PageSize
);
internal sealed record ProjectsDto(
    Guid Id,
    string Title,
    string Description,
    DateTime CreatedAt,
    string CoverImageUrl,
    string[] Tech,
    string Category,
    string gitHubLink
);
