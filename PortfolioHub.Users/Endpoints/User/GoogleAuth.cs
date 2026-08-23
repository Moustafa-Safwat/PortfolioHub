using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Users.Usecases.User.GoogleAuth;
using PortfolioHub.Users.Usecases.User.Login;

namespace PortfolioHub.Users.Endpoints.User;

internal sealed class GoogleAuth
(
    ISender sender
) : Endpoint<GoogleAuthRequest, Result<LoginDtoResult>>
{

    public override void Configure()
    {
        Post("/auth/google");
        AllowAnonymous();
    }

    public async override Task HandleAsync(GoogleAuthRequest req, CancellationToken ct)
    {
        var remoteAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;

        var googleAuthCommand = new GoogleAuthCommand(req.Credential, remoteAddress);

        var result = await sender.Send(googleAuthCommand);

        if (!result.IsSuccess)
        {
            await SendAsync(result, StatusCodes.Status400BadRequest, ct);
            return;
        }

        await SendAsync(result, StatusCodes.Status200OK, ct);
    }
}
