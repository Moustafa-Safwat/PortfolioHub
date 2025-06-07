using Ardalis.Result;
using MediatR;

namespace PortfolioHub.Projects.Usecases.Project;

internal sealed record DeleteProjectCommand(Guid Id)
    : IRequest<Result>;
