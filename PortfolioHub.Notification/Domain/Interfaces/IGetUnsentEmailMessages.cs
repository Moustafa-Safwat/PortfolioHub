using Ardalis.Result;
using PortfolioHub.Notification.Domain.Entities;

namespace PortfolioHub.Notification.Domain.Interfaces;

interface IGetUnsentEmailMessages : IEmailSaveChanges
{
    Task<IEnumerable<EmailOutBox>> GetUnsentEmailMessagesAsync(CancellationToken cancellationToken);
}
