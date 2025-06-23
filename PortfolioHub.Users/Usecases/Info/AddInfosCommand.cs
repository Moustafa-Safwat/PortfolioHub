using Ardalis.Result;
using MediatR;
using PortfolioHub.Users.Endpoints.Info;

namespace PortfolioHub.Users.Usecases.Info;

internal sealed record AddInfosCommand(
    IEnumerable<InfoDto> Infos
    ) : IRequest<Result<Guid[]>>;
