namespace PortfolioHub.Users.Domain.Interfaces;

public interface IEmailVerificationMessageFormatter
{
    Task<string> FormatEmailMessageAsync(string verificationLink, CancellationToken cancellationToken);
}
