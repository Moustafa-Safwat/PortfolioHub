using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Users.Usecases.VerifyEmail.Send;

internal sealed record SendEmailVerificationCommand
(
    Guid UserId
) : ICommand;
