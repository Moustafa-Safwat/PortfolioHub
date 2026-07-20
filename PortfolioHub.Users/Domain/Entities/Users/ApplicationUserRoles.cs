namespace PortfolioHub.Users.Domain.Entities.Users;

// Value object representing the different roles an application user can have.
// This is used to seed the default roles into the database and can be referenced
// throughout the application to ensure consistency in role names.
internal enum ApplicationUserRoles
{
    User,
    Admin,
    System,
    Contributor,
}
