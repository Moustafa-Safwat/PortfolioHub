using Ardalis.Result;
using MediatR;

namespace PortfolioHub.Users.Usecases.User.Create;

internal sealed record CreateUserCommand(
    string UserName,
    string Email,
    string Password
    ) : IRequest<Result<Guid>>;
