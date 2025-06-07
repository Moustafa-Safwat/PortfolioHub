namespace PortfolioHub.Users.Endpoints.User;

internal record CreateUserReq(
    string UserName,
    string Email,
    string Password,
    CreateUserRole Role
    );

internal enum CreateUserRole
{
    Guest,
    Admin
}