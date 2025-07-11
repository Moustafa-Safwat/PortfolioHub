using Ardalis.Result;
using MediatR;
using PortfolioHub.Notification.Domain.Entities;
using PortfolioHub.Notification.Domain.Interfaces;
using Serilog;

namespace PortfolioHub.Notification.Usecases;

internal sealed class QueueEmailInOutboxSendEmailHandler(
    IQueueEmailMessage outboxService,
    ILogger logger
    )
    : IRequestHandler<SendEmailCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        EmailOutBox emailOutBox = new EmailOutBox(
            request.From,
            request.To,
            request.Subject,
            request.Body);

        try
        {
            await outboxService.QueueEmailMessageAsync(emailOutBox, cancellationToken);
            var result = await outboxService.SaveChangesAsync(cancellationToken);

            if (!result.IsSuccess)
            {
                logger.Error("Failed to save email to outbox. Errors: {Errors}", string.Join(", ", result.Errors));
                return Result.Error(new ErrorList(result.Errors.ToArray()));
            }

            return Result.Success(emailOutBox.Id);
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Exception occurred while handling SendEmailCommand for To: {To}", request.To);
            return Result.Error(new ErrorList(["An unexpected error occurred while queuing the email."]));
        }
    }
}