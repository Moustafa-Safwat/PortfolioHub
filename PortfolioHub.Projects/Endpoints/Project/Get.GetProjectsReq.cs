namespace PortfolioHub.Projects.Endpoints.Project;

// TODO: this class should be removed
internal sealed record GetProjectsReq( 
    int PageNumber = 1,
    int PageSize = 10
);
