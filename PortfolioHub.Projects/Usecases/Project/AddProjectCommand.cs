using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Entities;

namespace PortfolioHub.Projects.Usecases.Project;

internal sealed record AddProjectCommand(
    string Title,
    string Description,
    string? VideoUrl,
    DateTime CreatedAt,
    string UserRole
    )
    : IRequest<Result<Guid>>;
