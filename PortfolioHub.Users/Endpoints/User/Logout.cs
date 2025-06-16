using System.Security.Claims;
using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Users.Usecases.User.Logout;

namespace PortfolioHub.Users.Endpoints.User;

internal sealed class Logout(
    ISender sender
    ) : EndpointWithoutRequest<Result>
{
    public override void Configure()
    {
        Post("/user/logout");
        Claims(ClaimTypes.NameIdentifier);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
        {
            var error = Result.Error("Invalid user identifier in claim.");
            await SendAsync(error, StatusCodes.Status400BadRequest, ct);
            return;
        }
        var logoutCommand = new LogoutCommand(userId);
        var result = await sender.Send(logoutCommand, ct);

        if (!result.IsSuccess)
        {
            await SendAsync(result, StatusCodes.Status400BadRequest, ct);
            return;
        }

        var response = Result.SuccessWithMessage($"User with Id [{userId}] logged out successfully.");
        await SendOkAsync(response, ct);
    }
}
