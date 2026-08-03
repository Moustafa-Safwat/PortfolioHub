using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Blogs.Usecases.Blogs.Like;
using System.Security.Claims;

namespace PortfolioHub.Blogs.Endpoints.Blogs;

internal sealed class Like
(
    ISender sender
) : EndpointWithoutRequest<Result>
{
    public override void Configure()
    {
        Put("/blogs/{blogId}/like");
        Claims(ClaimTypes.NameIdentifier);
    }

    public async override Task HandleAsync(CancellationToken ct)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out Guid userGuid))
        {
            var error = Result.Error("Invalid token.");
            await SendAsync(error, StatusCodes.Status400BadRequest, ct);
            return;
        }

        var blogIdRouteValue = Route<string>("blogId");
        if (!Guid.TryParse(blogIdRouteValue, out Guid blogId))
        {
            var errorList = new ErrorList(["Invalid blog id"]);
            var error = Result.Error(errorList);
            await SendAsync(error, StatusCodes.Status400BadRequest, ct);
            return;
        }

        var likeBlogCommand = new LikeBlogCommand(blogId, userGuid);
        var result = await sender.Send(likeBlogCommand, ct);
        if (!result.IsSuccess)
        {
            await SendAsync(result, StatusCodes.Status400BadRequest, ct);
            return;
        }

        await SendAsync(result, StatusCodes.Status200OK, ct);
    }
}
