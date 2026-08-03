using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using PortfolioHub.Users.Usecases.User.Login;

namespace PortfolioHub.Users.Endpoints.User;

internal class Login(
    ISender sender,
    IConfiguration configuration,
    ICaptchaValidator captchaValidator
    ) : Endpoint<UserCred, Result<LoginDtoResult>>
{
    public override void Configure()
    {
        Post("/user/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UserCred req, CancellationToken ct)
    {
        // Captcha validation
        string captchaAction = configuration["GoogleRecaptcha:ActionForLogin"] ??
            throw new InvalidOperationException("ActionForLogin configuration value is missing.");

        var isHuman = await captchaValidator.IsValidAsync(req.Token, captchaAction, ct);

        if (!isHuman)
        {
            var errorObj = Result.Error(new ErrorList(["Captcha validation failed. Please try again."]));
            await SendAsync(errorObj, StatusCodes.Status400BadRequest, cancellation: ct);
            return;
        }

        string deviceName = string.IsNullOrEmpty(HttpContext.Request.Headers["User-Agent"].ToString())
            ? "unknown"
            : HttpContext.Request.Headers["User-Agent"].ToString();
        string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        var loginCommand = new LoginCommand(req.Email, req.Password, deviceName, ipAddress);
        var loginResult = await sender.Send(loginCommand);
        if (loginResult.IsSuccess)
        {
            await SendOkAsync(loginResult, ct);
        }
        else
        {
            if (loginResult.Status == ResultStatus.Unauthorized)
            {
                await SendAsync(loginResult, StatusCodes.Status401Unauthorized, ct);
            }
            else if (loginResult.Status == ResultStatus.NotFound)
            {
                await SendAsync(loginResult, StatusCodes.Status404NotFound, ct);
            }
            else
            {
                await SendAsync(loginResult, StatusCodes.Status400BadRequest, ct);
            }
        }
    }
}
