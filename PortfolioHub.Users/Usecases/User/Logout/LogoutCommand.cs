using Ardalis.Result;
using MediatR;

namespace PortfolioHub.Users.Usecases.User.Logout;

internal sealed record LogoutCommand(
    string UserId
    ) : IRequest<Result>;
