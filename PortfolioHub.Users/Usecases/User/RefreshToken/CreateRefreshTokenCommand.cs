using Ardalis.Result;
using MediatR;
using PortfolioHub.Users.Usecases.User.Login;

namespace PortfolioHub.Users.Usecases.User.RefreshToken;

internal sealed record CreateRefreshTokenCommand(
    string RefreshToken
    ) : IRequest<Result<LoginDtoResult>>;
