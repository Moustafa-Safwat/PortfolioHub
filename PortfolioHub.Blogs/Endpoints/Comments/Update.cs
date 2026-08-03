using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Blogs.Usecases.Comments.Update;
using System.Security.Claims;

namespace PortfolioHub.Blogs.Endpoints.Comments;

internal sealed class Update
(
    ISender sender
) : Endpoint<CommentUpdateRequest, Result>
{
    public override void Configure()
    {
        Put("/blogs/{blogId:guid}/comments/{commentId:guid}");
        Claims(ClaimTypes.NameIdentifier);
    }

    public override async Task HandleAsync(CommentUpdateRequest req, CancellationToken ct)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out Guid userGuid))
        {
            var error = Result.Error("Invalid token.");
            await SendAsync(error, StatusCodes.Status400BadRequest, ct);
            return;
        }

        var updateCommentCommand = new UpdateCommentCommand(req.BlogId, userGuid, req.CommentId, req.Comment);
        var result = await sender.Send(updateCommentCommand, ct);

        if (!result.IsSuccess)
        {
            await SendAsync(result, StatusCodes.Status400BadRequest, ct);
            return;
        }

        await SendAsync(result, StatusCodes.Status200OK, ct);
    }
}
