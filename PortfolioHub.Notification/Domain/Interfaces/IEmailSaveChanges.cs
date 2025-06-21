using Ardalis.Result;

namespace PortfolioHub.Notification.Domain.Interfaces;

interface IEmailSaveChanges
{
    Task<Result> SaveChangesAsync(CancellationToken cancellationToken);
}
