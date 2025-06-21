using Ardalis.Result;

namespace PortfolioHub.Notification.Domain.Interfaces;

internal interface ISendEmail
{
    Task<Result> SendEmailAsync(string from, string to, string subject, string body, CancellationToken cancellationToken);
}
