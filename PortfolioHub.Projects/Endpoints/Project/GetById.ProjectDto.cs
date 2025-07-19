using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using PortfolioHub.Projects.Endpoints.Category;
using PortfolioHub.Projects.Endpoints.TechanicalSkills;

namespace PortfolioHub.Projects.Endpoints.Project;

internal sealed record ProjectDto(
    string Title,
    string Description,
    string LongDescription,
    DateTime CreatedAt,
    CategoryDto Category,
    string VideoId,
    string CoverImageUrl,
    string[] ImagesUrls,
    TechSkillDto[] Skills,
    LinkDto[] Links
);

internal sealed record LinkDto(
    string ProviderId, // GUID
    string Link,
    string ProviderName,
    string ProviderBaseUrl
);