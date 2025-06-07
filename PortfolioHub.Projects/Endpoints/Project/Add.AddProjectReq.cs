namespace PortfolioHub.Projects.Endpoints.Project;

internal sealed record AddProjectReq(
    string Title,
    string Description,
    string? VideoUrl,
    DateTime CreatedAt
    );
