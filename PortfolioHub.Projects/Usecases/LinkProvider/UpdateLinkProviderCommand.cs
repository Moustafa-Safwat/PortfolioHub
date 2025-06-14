using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Entities;

namespace PortfolioHub.Projects.Usecases.LinkProvider;

internal sealed record UpdateLinkProviderCommand(
    Guid Id,
    string Name,
    string BaseUrl
    ) : IRequest<Result>;
