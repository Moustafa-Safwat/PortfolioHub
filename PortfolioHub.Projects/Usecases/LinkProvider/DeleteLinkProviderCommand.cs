using Ardalis.Result;
using MediatR;

namespace PortfolioHub.Projects.Usecases.LinkProvider;

internal sealed record DeleteLinkProviderCommand(
    Guid Id
    ) : IRequest<Result<Guid>>;
