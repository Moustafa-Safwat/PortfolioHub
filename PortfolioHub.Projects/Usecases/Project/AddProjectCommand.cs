using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Entities;

namespace PortfolioHub.Projects.Usecases.Project;

internal sealed record AddProjectCommand(
    string Title,
    string Description,
    string LongDescription,
    string? VideoUrl,
    string? CoverImageUrl,
    DateTime CreatedAt)
    : IRequest<Result<Guid>>;
