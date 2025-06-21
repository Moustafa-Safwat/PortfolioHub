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
    private static readonly TimeSpan CheckInterval = TimeSpan.FromSeconds(60);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(CheckInterval);

        do
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

            logger.LogInformation("Waiting for the next email check...");
        }
        while (await timer.WaitForNextTickAsync(stoppingToken));
    }
}

