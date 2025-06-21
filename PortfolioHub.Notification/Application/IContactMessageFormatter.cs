namespace PortfolioHub.Notification.Application;

internal interface IContactMessageFormatter
{
    Task<string> FormatContactMessageAsync(string name, string email, string subject, string message,
        CancellationToken cancellationToken);
}
