using PortfolioHub.Notification.Domain.Interfaces;
using Serilog;

namespace PortfolioHub.Notification.Infrastructure.Services;

class SendEmailFromOutboxService(
    ISendEmail sendEmail,
    IGetUnsentEmailMessages outboxService,
    ILogger logger
    )
    : ISendEmailFromOutboxService
{
    public async Task CheckForAndSentEmails(CancellationToken cancellationToken)
    {
        var unsentEmails = await outboxService.GetUnsentEmailMessagesAsync(cancellationToken);

        foreach (var unsentEmail in unsentEmails)
        {
            try
            {
                var sendResult = await sendEmail.SendEmailAsync(
                    unsentEmail.From,
                    unsentEmail.To,
                    unsentEmail.Subject,
                    unsentEmail.Body,
                    cancellationToken);
                if (sendResult.IsSuccess)
                {
                    logger.Information("Email with Id: {id} sent successfully to {To} with subject '{Subject}'", unsentEmail.Id, unsentEmail.To, unsentEmail.Subject);
                    unsentEmail.MarkAsSent();
                    await outboxService.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    logger.Information("Failed to send email with Id: {id} to {To} with subject '{Subject}'. Errors: {Errors}",
                        unsentEmail.Id,
                        unsentEmail.To,
                        unsentEmail.Subject,
                        string.Join(", ", sendResult.Errors ?? []));
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error sending email");
            }
        }
    }
}
