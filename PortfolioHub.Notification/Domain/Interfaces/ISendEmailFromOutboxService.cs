using Ardalis.Result;

namespace PortfolioHub.Notification.Domain.Interfaces;

interface ISendEmailFromOutboxService
{
    Task CheckForAndSentEmails(CancellationToken cancellationToken);
}
