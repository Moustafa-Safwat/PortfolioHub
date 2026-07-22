using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using PortfolioHub.Users.Domain.Entities.Users;
using PortfolioHub.Users.Usecases.User.Create;

namespace PortfolioHub.Users.Endpoints.User;

internal class Create
(
    ISender sender,
    IConfiguration configuration,
    ICaptchaValidator captchaValidator
) : Endpoint<CreateUserReq>
{
    public override void Configure()
    {
        Post("/user/register");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserReq req, CancellationToken ct)
    {
        // Captcha validation
        string captchaAction = configuration["GoogleRecaptcha:ActionForRegister"] ??
            throw new InvalidOperationException("ActionForRegister configuration value is missing.");

        var isHuman = await captchaValidator.IsValidAsync(req.Token, captchaAction, ct);

        if (!isHuman)
        {
            var errorObj = Result.Error(new ErrorList(["Captcha validation failed. Please try again."]));
            await SendAsync(errorObj, StatusCodes.Status400BadRequest, cancellation: ct);
            return;
        }

        // Create the command with validated role
        var createUserCommand = new CreateUserCommand(
            Email: req.Email,
            Password: req.Password,
            FirstName: req.FirstName,
            LastName: req.LastName,
            Role: nameof(ApplicationUserRoles.User).ToLower(),
            CompanyName: req.CompanyName
        );

        var createUserResult = await sender.Send(createUserCommand, ct);

        if (createUserResult.IsSuccess)
        {
            var userId = createUserResult.Value;
            await SendCreatedAtAsync<Create>(
                $"/users/{userId}",
                createUserResult,
                cancellation: ct);
        }
        else
        {
            await SendAsync(createUserResult, StatusCodes.Status400BadRequest, ct);
        }
    }
}
