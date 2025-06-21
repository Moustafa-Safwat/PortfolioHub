using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using PortfolioHub.Notification.Domain.Entities;
using PortfolioHub.Notification.Domain.Interfaces;
using PortfolioHub.Notification.Infrastructure.Context;

namespace PortfolioHub.Notification.Infrastructure.EFRepository;

class EFGetUnsentEmailMessages(
     OutboxDbContext outboxDbContext
 ) : IGetUnsentEmailMessages
{
    public async Task<IEnumerable<EmailOutBox>> GetUnsentEmailMessagesAsync(CancellationToken cancellationToken)
    {
        return await outboxDbContext.EmailOutBoxes
            .Where(e => !e.IsSent)
            .ToListAsync(cancellationToken);
    }

    public async Task<Result> SaveChangesAsync(CancellationToken cancellationToken)
    {
        var effectedRoes = await outboxDbContext.SaveChangesAsync(cancellationToken);
        return effectedRoes > 0 ? Result.Success() : Result.Invalid(new ValidationError
        {
            ErrorMessage = "No changes were made to the database."
        });
    }

}
