using Ardalis.Result;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using PortfolioHub.Notification.Domain.Interfaces;
using MimeKit.Text;

namespace PortfolioHub.Notification.Infrastructure;

internal sealed class MimeKitEmailSender(
    IConfiguration configuration
    ) : ISendEmail
{

    public async Task<Result> SendEmailAsync(string from, string to, string subject, string body,
        CancellationToken cancellationToken)
    {
        try
        {
            using (var client = new SmtpClient())
            {

                var host = configuration.GetValue<string>("EmailSettings:Host");
                var port = configuration.GetValue<int>("EmailSettings:PortTLS");
                client.Connect(host, port, MailKit.Security.SecureSocketOptions.StartTls, cancellationToken);

                await client.AuthenticateAsync(
                    configuration.GetValue<string>("EmailSettings:Username"),
                    configuration.GetValue<string>("EmailSettings:Password"), 
                    cancellationToken);

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(from, from));
                message.To.Add(new MailboxAddress(to, to));
                message.Subject = subject;
                message.Body = new TextPart(TextFormat.Html) { Text = body };

                await client.SendAsync(message, cancellationToken);
                return Result.Success();
            }
        }
        catch (Exception ex)
        {

            return Result.Error(new ErrorList(["Faild to send email", ex.Message]));
        }
    }
}
