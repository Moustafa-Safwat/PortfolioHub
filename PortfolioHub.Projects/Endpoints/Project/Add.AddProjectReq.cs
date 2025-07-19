using Microsoft.AspNetCore.Components.Web;

namespace PortfolioHub.Projects.Endpoints.Project;

internal sealed record AddProjectReq(
    string Title,
    string Description,
    string LongDescription,
    DateTime CreatedDate,
    bool IsFeatured,
    string CategoryId, // GUID
    string? VideoId,
    string? CoverImageUrl,
    string[] ImagesUrls,
    string[] SkillsId, // GUID
    AddLinksDto[] Links
    );

internal sealed record AddLinksDto(
    string ProviderId, // GUID
    string Link
    );
