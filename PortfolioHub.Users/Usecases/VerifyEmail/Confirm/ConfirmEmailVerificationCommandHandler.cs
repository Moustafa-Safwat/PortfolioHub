using Ardalis.Result;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using PortfolioHub.Users.Domain.Entities.Users;
using System.Text;
using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Users.Usecases.VerifyEmail.Confirm;

internal sealed class ConfirmEmailVerificationCommandHandler
(
    UserManager<ApplicationUser> userManager
) : ICommandHandler<ConfirmEmailVerificationCommand>
{
    public async Task<Result> Handle(ConfirmEmailVerificationCommand request, CancellationToken cancellationToken)
    {
        var applicationUser = await userManager.FindByIdAsync(request.UserId);
        if (applicationUser is null) return Result.NotFound("User not found");

        var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.EncodedToken));

        var result = await userManager.ConfirmEmailAsync(applicationUser, token);

        if (!result.Succeeded)
        {
            return Result.Error("Invalid or expired verification token.");
        }

        return Result.Success();
    }
}