namespace PortfolioHub.Projects.Endpoints.Project;

internal sealed record AddProjectReq(
    string Title,
    string Description,
    string LongDescription,
    string? VideoUrl,
    string? CoverImageUrl,
    DateTime CreatedAt
    );
