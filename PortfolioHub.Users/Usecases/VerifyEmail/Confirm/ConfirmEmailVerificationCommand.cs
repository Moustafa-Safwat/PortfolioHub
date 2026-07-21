using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Users.Usecases.VerifyEmail.Confirm;

internal sealed record ConfirmEmailVerificationCommand
(
    string UserId,
    string EncodedToken
) : ICommand;
