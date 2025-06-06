using Ardalis.Result;
using MediatR;

namespace PortfolioHub.Users.Usecases.User.Login;

internal sealed record LoginCommand(
    string UserName,
    string Password
    ):IRequest<Result<string>>;
