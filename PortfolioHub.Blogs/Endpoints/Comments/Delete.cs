using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Blogs.Usecases.Comments.Delete;
using System.Security.Claims;

namespace PortfolioHub.Blogs.Endpoints.Comments;

internal sealed class Delete
(
    ISender sender
) : Endpoint<DeleteCommentReq, Result>
{
    public override void Configure()
    {
        Delete("/blogs/{blogId:guid}/comments/{commentId:guid}");
        Claims(ClaimTypes.NameIdentifier);
    }

    public async override Task HandleAsync(DeleteCommentReq req, CancellationToken ct)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out Guid userGuid))
        {
            var error = Result.Error("Invalid token.");
            await SendAsync(error, StatusCodes.Status401Unauthorized, ct);
            return;
        }

        var deleteCommentCommand = new DeleteCommentCommand(req.BlogId, req.CommentId, userGuid);
        var result = await sender.Send(deleteCommentCommand, ct);
        if (!result.IsSuccess)
        {
            await SendAsync(result, StatusCodes.Status400BadRequest, ct);
            return;
        }
        await SendAsync(result, StatusCodes.Status200OK, ct);
    }
}
