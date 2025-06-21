using Ardalis.Result;
using PortfolioHub.Notification.Domain.Entities;
using PortfolioHub.Notification.Domain.Interfaces;
using PortfolioHub.Notification.Infrastructure.Context;

namespace PortfolioHub.Notification.Infrastructure.EFRepository;

internal sealed class EFQueueEmailMessage(
  OutboxDbContext outboxDbContext
  ) : IQueueEmailMessage
{

    public async Task<Result> QueueEmailMessageAsync(EmailOutBox email, CancellationToken cancellationToken)
    {
        await outboxDbContext.EmailOutBoxes.AddAsync(email, cancellationToken);
        return Result.Success();
    }

    public async Task<Result> SaveChangesAsync(CancellationToken cancellationToken)
    {
        var effectedRows = await outboxDbContext.SaveChangesAsync(cancellationToken);
        return effectedRows > 0
            ? Result.Success()
            : Result.Error(new ErrorList(["No changes were made to the database."]));
    }
}
