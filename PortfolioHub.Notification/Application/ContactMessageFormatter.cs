namespace PortfolioHub.Notification.Application;

internal sealed class ContactMessageFormatter : IContactMessageFormatter
{
    public Task<string> FormatContactMessageAsync(string name, string email, string subject, string message, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var sentTime = DateTime.UtcNow.ToString("f");
        var formattedMessage = $@"
            <html>
                <body style=""font-family:Segoe UI,Arial,sans-serif;font-size:15px;color:#222;"">
                    <h2 style=""color:#2d6cdf;"">New Contact Form Submission</h2>
                    <p>Dear Recipient,</p>
                    <p>
                        You have received a new message via the contact form. Please find the details below:
                    </p>
                    <table style=""border-collapse:collapse;margin-bottom:16px;"">
                        <tr>
                            <td style=""padding:4px 8px;font-weight:bold;"">Sender Name:</td>
                            <td style=""padding:4px 8px;"">{name}</td>
                        </tr>
                        <tr>
                            <td style=""padding:4px 8px;font-weight:bold;"">Sender Email:</td>
                            <td style=""padding:4px 8px;"">{email}</td>
                        </tr>
                        <tr>
                            <td style=""padding:4px 8px;font-weight:bold;"">Subject:</td>
                            <td style=""padding:4px 8px;"">{subject}</td>
                        </tr>
                        <tr>
                            <td style=""padding:4px 8px;font-weight:bold;"">Sent At (UTC):</td>
                            <td style=""padding:4px 8px;"">{sentTime}</td>
                        </tr>
                    </table>
                    <p style=""font-weight:bold;margin-bottom:4px;"">Message:</p>
                    <div style=""background:#f5f5f5;padding:12px;border-radius:6px;border:1px solid #e0e0e0;"">
                        {message}
                    </div>
                    <p style=""margin-top:24px;color:#888;font-size:13px;"">
                        This message was sent automatically from the PortfolioHub contact form.
                    </p>
                </body>
            </html>
        ";

        return Task.FromResult(formattedMessage);
    }
}