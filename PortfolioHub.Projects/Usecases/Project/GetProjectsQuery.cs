using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Endpoints.Project;

namespace PortfolioHub.Projects.Usecases.Project;

internal sealed record GetProjectsQuery(
    int PageNumber,
    int PageSize
    ) : IRequest<Result<IReadOnlyList<ProjectDto>>>;
