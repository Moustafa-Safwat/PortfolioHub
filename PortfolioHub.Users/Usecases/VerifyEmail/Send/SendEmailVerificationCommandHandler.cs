using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using PortfolioHub.Notification.Usecases;
using PortfolioHub.Users.Domain.Entities.Users;
using PortfolioHub.Users.Domain.Interfaces;
using System.Text;
using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Users.Usecases.VerifyEmail.Send;

internal sealed class SendEmailVerificationCommandHandler
(
    ISender sender,
    IConfiguration configuration,
    UserManager<ApplicationUser> userManager,
    IEmailVerificationLink emailVerificationLink,
    IEmailVerificationMessageFormatter emailVerificationMessageFormatter
) : ICommandHandler<SendEmailVerificationCommand>
{
    public async Task<Result> Handle(SendEmailVerificationCommand request, CancellationToken cancellationToken)
    {
        string userId = request.UserId.ToString();

        var applicationUser = await userManager.FindByIdAsync(userId);
        if (applicationUser is null) return Result.NotFound("User not found");

        var token = await userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        var verificationLinkResult = emailVerificationLink.Build(userId, encodedToken);
        if (!verificationLinkResult.IsSuccess)
            return Result.Error(new ErrorList(verificationLinkResult.Errors));

        var emailBody = await emailVerificationMessageFormatter.FormatEmailMessageAsync(verificationLinkResult, cancellationToken);

        var fromEmail = configuration.GetValue<string>("EmailSettings:FromEmail")
            ?? throw new InvalidOperationException("From email is not configured");

        var sendEmailCommand = new SendEmailCommand
        (
            fromEmail,
            applicationUser.Email!,
            "Email Verification",
            emailBody
        );

        var sendEmailResult = await sender.Send(sendEmailCommand, cancellationToken);
        if (!sendEmailResult.IsSuccess)
            return Result.Error(new ErrorList(sendEmailResult.Errors));

        return Result.Success();
    }
}
