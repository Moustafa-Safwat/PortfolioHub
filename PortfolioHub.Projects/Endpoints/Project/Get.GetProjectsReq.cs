namespace PortfolioHub.Projects.Endpoints.Project;

internal sealed record GetProjectsReq(
    int PageNumber = 1,
    int PageSize = 10
);
