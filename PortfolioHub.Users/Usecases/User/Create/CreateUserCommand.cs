using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Users.Usecases.User.Create;

internal sealed record CreateUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string Role,
    string CompanyName,
    bool SendEmailVerification = true,
    string ImageUrl = ""
    ) : ICommand<Guid>;
