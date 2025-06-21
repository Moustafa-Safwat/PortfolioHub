using Ardalis.Result;
using PortfolioHub.Notification.Domain.Entities;

namespace PortfolioHub.Notification.Domain.Interfaces;

interface IQueueEmailMessage : IEmailSaveChanges
{
    Task<Result> QueueEmailMessageAsync(EmailOutBox email, CancellationToken cancellationToken);
}
