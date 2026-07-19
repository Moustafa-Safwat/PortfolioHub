namespace PortfolioHub.Users.Endpoints.User;

internal sealed record CreateUserReq
(
    string FirstName,
    string LastName,
    string Email,
    string Password
);