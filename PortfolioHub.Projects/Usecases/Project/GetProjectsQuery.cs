using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Entities;
using PortfolioHub.Projects.Endpoints.Project;

namespace PortfolioHub.Projects.Usecases.Project;

internal sealed record GetProjectsQuery(
    int PageNumber,
    int PageSize,
    Guid? CategoryId,
    string? Search) : IRequest<Result<GetProjectsQueryResponse>>;


internal sealed record GetProjectsQueryResponse(
    IReadOnlyList<ProjectsDto> ProjectsDtos,
    int TotalCount);