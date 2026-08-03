using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using PortfolioHub.Users.Domain.Interfaces;

namespace PortfolioHub.Users.Usecases.VerifyEmail.Send;

internal sealed class EmailVerificationLinkService
(
    IHttpContextAccessor httpContextAccessor
) : IEmailVerificationLink
{
    public Result<string> Build(string userId, string encodedToken)
    {
        var request = httpContextAccessor.HttpContext?.Request;
        if (request is null)
        {
            return Result.Error("HttpContext is not available.");
        }

        var uriBuilder = new UriBuilder
        (
            request.Scheme,
            request.Host.Host,
            request.Host.Port ?? -1,
            $"api/{Endpoints.VerifyEmail.Confirm.RoutePath}"
        );

        var verificationUri = uriBuilder.Uri.AbsoluteUri.TrimEnd('/');

        Dictionary<string, string> queryParameters = new Dictionary<string, string>
        {
            { Endpoints.VerifyEmail.Confirm.UserIdQueryParams, userId },
            { Endpoints.VerifyEmail.Confirm.TokenQueryParams, encodedToken }
        };

        var verificationLink = QueryHelpers.AddQueryString(verificationUri, queryParameters!);

        return Result.Success(verificationLink);
    }
}

