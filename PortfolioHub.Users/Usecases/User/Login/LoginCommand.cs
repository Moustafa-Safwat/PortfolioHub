using Ardalis.Result;
using MediatR;

namespace PortfolioHub.Users.Usecases.User.Login;

internal sealed record LoginCommand(
    string UserEmail,
    string Password,
    string Device,
    string IpAddress
    ) : IRequest<Result<LoginDtoResult>>;
