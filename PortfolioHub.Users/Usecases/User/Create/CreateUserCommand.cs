using Ardalis.Result;
using MediatR;
using PortfolioHub.Users.Endpoints.User;

namespace PortfolioHub.Users.Usecases.User.Create;

internal sealed record CreateUserCommand(
    string UserName,
    string Email,
    string Password,
    string Role) : IRequest<Result<Guid>>;
