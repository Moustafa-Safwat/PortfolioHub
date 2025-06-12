using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Entities;

namespace PortfolioHub.Projects.Usecases.LinkProvider;

internal sealed record AddLinkProviderCommand(
    string Name,
    string Url
    ) : IRequest<Result<Guid>>;
