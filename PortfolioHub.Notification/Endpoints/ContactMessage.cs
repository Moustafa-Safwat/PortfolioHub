using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PortfolioHub.Notification.Application;
using PortfolioHub.Notification.Usecases;

namespace PortfolioHub.Notification.Endpoints;

internal sealed class ContactMessage(
    ISender sender,
    IContactMessageFormatter contactMessageFormatter,
    IConfiguration configuration
    ) : Endpoint<ContactMessageRequest, Result>
{
    public override void Configure()
    {
        Post("/contact-message");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ContactMessageRequest req, CancellationToken ct)
    {
        var formatedMessage = await contactMessageFormatter.FormatContactMessageAsync(
            req.Name,
            req.Email,
            req.Subject,
            req.Message,
            ct
        );

        var sendEmailCommand = new SendEmailCommand(
            configuration.GetValue<string>("EmailSettings:FromEmail")!,
            configuration.GetValue<string>("Auth:AdminEmail")!,
            req.Subject,
            formatedMessage
        );

        var result = await sender.Send(sendEmailCommand, ct);

        if (!result.IsSuccess)
        {
            var errorObj = Result.Error(new ErrorList(result.Errors.ToArray()));
            await SendAsync(errorObj, StatusCodes.Status400BadRequest, cancellation: ct);
            return;
        }

        var successObj = Result.SuccessWithMessage("Your message has been sent successfully.");
        await SendOkAsync(successObj, cancellation: ct);
    }
}
