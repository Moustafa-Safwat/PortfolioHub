using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Users.Usecases.VerifyEmail.Confirm;

namespace PortfolioHub.Users.Endpoints.VerifyEmail;

internal sealed class Confirm
(
    ISender sender
) : EndpointWithoutRequest<Result>
{
    public const string RoutePath = "auth/verify-email/confirm";
    public const string UserIdQueryParams = "userId";
    public const string TokenQueryParams = "token";
    public override void Configure()
    {
        Get(RoutePath);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        string? userId = Query<string>(UserIdQueryParams);
        string? token = Query<string>(TokenQueryParams);

        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
        {
            var validationError = new ValidationError("UserId and token query parameters are required.");
            await SendAsync(Result.Invalid(validationError), StatusCodes.Status400BadRequest, ct);
            return;
        }

        var confirmEmailCommand = new ConfirmEmailVerificationCommand(userId, token);
        var confirmEmailResult = await sender.Send(confirmEmailCommand, ct);

        if (!confirmEmailResult.IsSuccess)
        {
            await SendAsync(confirmEmailResult, StatusCodes.Status400BadRequest, ct);
            return;
        }

        var response = Result.SuccessWithMessage($"Your email has been successfully verified.");
        await SendOkAsync(response, ct);
    }
}

