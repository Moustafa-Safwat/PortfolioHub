using Ardalis.GuardClauses;
using PortfolioHub.SharedKernal.Domain.Entities;

namespace PortfolioHub.Notification.Domain.Entities;

internal class EmailOutBox : BaseEntity
{
    public EmailOutBox(string from, string to, string subject, string body)
    {
        Id = Guid.NewGuid();
        From = Guard.Against.NullOrEmpty(from);
        To = Guard.Against.NullOrEmpty(to);
        Subject = Guard.Against.NullOrEmpty(subject);
        Body = Guard.Against.NullOrEmpty(body);
    }

    public string From { get; private set; } = string.Empty;
    public string To { get; private set; } = string.Empty;
    public string Subject { get; private set; } = string.Empty;
    public string Body { get; private set; } = string.Empty;
    public bool IsSent { get; private set; } = false;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public void MarkAsSent()
    {
        IsSent = true;
    }
}
