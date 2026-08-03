using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using PortfolioHub.Users.Usecases.VerifyEmail.Send;

namespace PortfolioHub.Users.Endpoints.VerifyEmail;

internal sealed class ReSend
(
    ISender sender,
    IConfiguration configuration,
    ICaptchaValidator captchaValidator
) : Endpoint<ResendEmail, Result>
{
    public override void Configure()
    {
        Post("/auth/verify-email/resend");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ResendEmail req, CancellationToken ct)
    {
        // Captcha validation
        string captchaAction = configuration["GoogleRecaptcha:ActionForResendVerificationEmail"] ??
            throw new InvalidOperationException("ActionForResendVerificationEmail configuration value is missing.");

        var isHuman = await captchaValidator.IsValidAsync(req.Token, captchaAction, ct);

        if (!isHuman)
        {
            var errorObj = Result.Error(new ErrorList(["Captcha validation failed. Please try again."]));
            await SendAsync(errorObj, StatusCodes.Status400BadRequest, cancellation: ct);
            return;
        }

        if (!Guid.TryParse(req.UserId, out Guid userGuid))
        {
            var errorObj = Result.Error(new ErrorList(["Invalid UserId format."]));
            await SendAsync(errorObj, StatusCodes.Status400BadRequest, cancellation: ct);
            return;
        }

        var sendVerificationEmailCommand = new SendEmailVerificationCommand(userGuid);
        var result = await sender.Send(sendVerificationEmailCommand, ct);

        if (!result.IsSuccess)
        {
            await SendAsync(result, StatusCodes.Status400BadRequest, ct);
            return;
        }

        var response = Result.SuccessWithMessage($"Verification email sent successfully to user.");
        await SendOkAsync(response, ct);
    }
}
