using Ardalis.Result;
using MediatR;
using static PortfolioHub.Users.Usecases.User.Login.LoginCommandHandler;

namespace PortfolioHub.Users.Usecases.User.Login;

internal sealed record LoginCommand(
    string UserName,
    string Password,
    string Device,
    string IpAddress
    ) :IRequest<Result<LoginDtoResult>>;
