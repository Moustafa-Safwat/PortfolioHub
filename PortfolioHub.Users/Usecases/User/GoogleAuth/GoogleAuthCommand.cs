using PortfolioHub.Users.Usecases.User.Login;
using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Users.Usecases.User.GoogleAuth;

internal sealed record GoogleAuthCommand
(
    string Credential,
    string RemoteAddress
) : ICommand<LoginDtoResult>;
