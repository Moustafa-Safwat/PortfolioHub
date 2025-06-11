using Ardalis.Result;
using MediatR;

namespace PortfolioHub.Projects.Usecases.Project;

internal sealed record AddProjectCommand(
    string Title,
    string Description,
    string LongDescription,
    DateTime CreatedDate,
    Guid CategoryId,
    string? VideoUrl,
    string? CoverImageUrl,
    string[] ImagesUrls,
    Guid[] SkillsId,
    (Guid ProviderId, string Link)[] Links
    )
    : IRequest<Result<Guid>>;
