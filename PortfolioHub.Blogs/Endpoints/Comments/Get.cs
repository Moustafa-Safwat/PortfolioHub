using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Blogs.Usecases.Comments.Get;
using System.Security.Claims;

namespace PortfolioHub.Blogs.Endpoints.Comments;

internal sealed class Get
(
    ISender sender
) : Endpoint<GetCommentsRequest, Result<IReadOnlyCollection<GetCommentsResponse>>>
{
    public override void Configure()
    {
        Get("/api/blogs/{blogId:guid}/comments");
        Claims(ClaimTypes.NameIdentifier);
    }

    public async override Task HandleAsync(GetCommentsRequest req, CancellationToken ct)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out Guid userGuid))
        {
            var error = Result.Error("Invalid token.");
            await SendAsync(error, StatusCodes.Status400BadRequest, ct);
            return;
        }

        var getBlogCommentsQuery = new GetBlogCommentsQuery(
            req.BlogId,
            userGuid,
            req.Page,
            req.PageSize);

        var result = await sender.Send(getBlogCommentsQuery, ct);
        if (!result.IsSuccess)
        {
            await SendAsync(result, StatusCodes.Status400BadRequest, ct);
            return;
        }

        await SendAsync(result, StatusCodes.Status200OK, ct);
    }
}
