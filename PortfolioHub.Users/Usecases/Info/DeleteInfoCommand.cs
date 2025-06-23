using Ardalis.Result;
using MediatR;

namespace PortfolioHub.Users.Usecases.Info;

internal sealed record DeleteInfoCommand(
    Guid key
    ) : IRequest<Result>;
