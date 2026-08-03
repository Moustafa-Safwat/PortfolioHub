namespace PortfolioHub.Users.Endpoints.User;

internal sealed record UserCred(
    string Email,
    string Password,
    string Token
    );
