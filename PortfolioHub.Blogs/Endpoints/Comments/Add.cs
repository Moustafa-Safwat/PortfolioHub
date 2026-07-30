using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Blogs.Usecases.Comments.Add;
using System.Security.Claims;

namespace PortfolioHub.Blogs.Endpoints.Comments;

internal sealed class Add
(
    ISender sender
) : Endpoint<CommentRequest, Result<Guid>>
{
    public override void Configure()
    {
        Post("/api/blogs/{blogId:guid}/comments");
        Claims(ClaimTypes.NameIdentifier);
    }

    public async override Task HandleAsync(CommentRequest req, CancellationToken ct)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out Guid userGuid))
        {
            var error = Result.Error("Invalid token.");
            await SendAsync(error, StatusCodes.Status400BadRequest, ct);
            return;
        }

        var addBlogComment = new AddBlogCommentCommand(
            req.BlogId,
            userGuid,
            req.Comment,
            req?.ParentCommentId);

        var result = await sender.Send(addBlogComment, ct);
        if (!result.IsSuccess)
        {
            await SendAsync(result, StatusCodes.Status400BadRequest, ct);
            return;
        }

        await SendAsync(result, StatusCodes.Status201Created, ct);
    }
}
