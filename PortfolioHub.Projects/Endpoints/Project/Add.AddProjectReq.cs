namespace PortfolioHub.Projects.Endpoints.Project;

internal sealed record AddProjectReq(
    string Title,
    string Description,
    string LongDescription,
    DateTime CreatedDate,
    string CategoryId, // GUID
    string? VideoUrl,
    string? CoverImageUrl,
    string[] ImagesUrls,
    string[] SkillsId, // GUID
    AddLinksDto[] Links
    );

internal sealed record AddLinksDto(
    string ProviderId, // GUID
    string Link
    );
