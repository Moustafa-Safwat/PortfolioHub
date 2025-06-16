using Ardalis.Result;
using Azure.Core;
using FastEndpoints;
using MediatR;
using PortfolioHub.Users.Usecases.User.Login;
using static PortfolioHub.Users.Usecases.User.Login.LoginCommandHandler;

namespace PortfolioHub.Users.Endpoints.User;

internal class Login(
    ISender sender
    ) : Endpoint<UserCred, Result<LoginDtoResult>>
{
    public override void Configure()
    {
        Post("/user/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UserCred req, CancellationToken ct)
    {
        string deviceName = string.IsNullOrEmpty(HttpContext.Request.Headers["User-Agent"].ToString())
            ? "unknown"
            : HttpContext.Request.Headers["User-Agent"].ToString();
        string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        var loginCommand = new LoginCommand(req.UserName, req.Password, deviceName, ipAddress);
        var loginResult = await sender.Send(loginCommand);
        if (loginResult.IsSuccess)
        {
            await SendOkAsync(loginResult, ct);
        }
        else
        {
            if (loginResult.Status == ResultStatus.Unauthorized)
            {
                await SendUnauthorizedAsync(ct);
            }
            else if (loginResult.Status == ResultStatus.NotFound)
            {
                await SendNotFoundAsync(ct);
            }
            else
            {
                await SendErrorsAsync(cancellation: ct);
            }
        }
    }
}
