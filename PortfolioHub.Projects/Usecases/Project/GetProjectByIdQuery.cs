using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Endpoints.Project;

namespace PortfolioHub.Projects.Usecases.Project;

internal sealed record GetProjectByIdQuery(
    Guid Id
    ) : IRequest<Result<ProjectDto>>;
