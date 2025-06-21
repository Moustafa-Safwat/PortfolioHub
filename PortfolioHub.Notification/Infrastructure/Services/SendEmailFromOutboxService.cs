using Ardalis.Result;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PortfolioHub.Notification.Domain.Interfaces;

namespace PortfolioHub.Notification.Infrastructure.Services;

class SendEmailFromOutboxService(
    ISendEmail sendEmail,
    IGetUnsentEmailMessages outboxService,
    ILogger<SendEmailFromOutboxService> logger
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
                    logger.LogInformation("Email with Id: {id} sent successfully to {To} with subject '{Subject}'", unsentEmail.Id, unsentEmail.To, unsentEmail.Subject);
                    unsentEmail.MarkAsSent();
                    await outboxService.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    logger.LogWarning("Failed to send email with Id: {id} to {To} with subject '{Subject}'. Errors: {Errors}",
                        unsentEmail.Id,
                        unsentEmail.To,
                        unsentEmail.Subject,
                        string.Join(", ", sendResult.Errors ?? []));
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error sending email");
            }
        }
    }
}
