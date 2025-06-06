using Ardalis.Result;
using FastEndpoints;
using MediatR;
using PortfolioHub.Users.Usecases.User.Login;

namespace PortfolioHub.Users.Endpoints.User;

internal class Login(
    ISender sender
    ) : Endpoint<UserCred,Result<string>>
{
    public override void Configure()
    {
        Post("/user/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UserCred req, CancellationToken ct)
    {
        var loginCommand = new LoginCommand(req.UserName, req.Password);
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
