using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Entities;
using PortfolioHub.Projects.Endpoints.LinkProvider;

namespace PortfolioHub.Projects.Usecases.LinkProvider;

internal sealed record GetLinkProvidersQuery()
    : IRequest<Result<IEnumerable<LinkProviderDto>>>;
