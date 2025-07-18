using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PortfolioHub.Notification.Application;
using PortfolioHub.Notification.Usecases;
using PortfolioHub.SharedKernal.Domain.Interfaces;

namespace PortfolioHub.Notification.Endpoints;

internal sealed class ContactMessage(
    ISender sender,
    IContactMessageFormatter contactMessageFormatter,
    IConfiguration configuration,
    ICaptchaValidator captchaValidator
    ) : Endpoint<ContactMessageRequest, Result>
{
    public override void Configure()
    {
        Post("/contact-message");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ContactMessageRequest req, CancellationToken ct)
    {
        // Captcha validation
        string captchaAction = configuration["GoogleRecaptcha:ActionForContactMesage"] ??
            throw new InvalidOperationException("ActionForContactMesage configuration value is missing.");

        var isHuman = await captchaValidator.IsValidAsync(req.Token, captchaAction, ct);

        if (!isHuman)
        {
            var errorObj = Result.Error(new ErrorList(["Captcha validation failed. Please try again."]));
            await SendAsync(errorObj, StatusCodes.Status400BadRequest, cancellation: ct);
            return;
        }

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
