using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PortfolioHub.Notification.Domain.Interfaces;

namespace PortfolioHub.Notification.Infrastructure.Backgroundworker;

internal sealed class EmailSendingBackgroundService(
    ILogger<EmailSendingBackgroundService> logger,
    IServiceScopeFactory serviceScopeFactory
    )
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int delayInMelliseconds = 10_000;// delay for 10 seconds

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                logger.LogInformation("Checking for emails to send...");

                using var scope = serviceScopeFactory.CreateScope();
                var sendEmailFromOutboxService = scope.ServiceProvider.GetRequiredService<ISendEmailFromOutboxService>();
                await sendEmailFromOutboxService.CheckForAndSentEmails(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error sending email");
            }
            finally
            {
                // Wait for a period before checking again
                await Task.Delay(TimeSpan.FromMilliseconds(delayInMelliseconds), stoppingToken);
                logger.LogInformation("Waiting for the next email check...");
            }
        }
    }
}

