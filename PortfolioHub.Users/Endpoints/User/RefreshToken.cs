using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Users.Usecases.User.RefreshToken;

namespace PortfolioHub.Users.Endpoints.User;

internal sealed class RefreshToken(
    ISender sender
    ) : Endpoint<RefreshTokenRequest, Result<RefreshTokenResponse>>
{
    public override void Configure()
    {
        Post("/user/refresh-token");
        AllowAnonymous();
    }

    public async override Task HandleAsync(RefreshTokenRequest req, CancellationToken ct)
    {
        var createRefreshTokenCommand = new CreateRefreshTokenCommand(req.RefreshToken);
        var result = await sender.Send(createRefreshTokenCommand, ct);

        if (result.IsUnauthorized())
        {
            var unauthorizedResponse = Result.Unauthorized(result.Errors.ToArray());
            await SendAsync(unauthorizedResponse, StatusCodes.Status401Unauthorized, ct);
            return;
        }

        if (!result.IsSuccess)
        {
            var errorResponse = Result.Error(new ErrorList(result.Errors));
            await SendAsync(errorResponse, StatusCodes.Status400BadRequest, ct);
            return;
        }

        var refreshTokenObj = result.Value;
        RefreshTokenResponse refreshTokenResponse = new RefreshTokenResponse
            (
                refreshTokenObj.AccessToken,
                refreshTokenObj.RefreshToken
            );

        var responseObj = Result.Success(refreshTokenResponse);
        await SendOkAsync(responseObj, ct);
    }
}
