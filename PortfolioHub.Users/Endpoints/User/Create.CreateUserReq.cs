namespace PortfolioHub.Users.Endpoints.User;

internal record CreateUserReq(
    string UserName,
    string Email,
    string Password
    );
