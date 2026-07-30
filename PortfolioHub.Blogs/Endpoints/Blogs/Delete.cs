using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Blogs.Usecases.Blogs.Delete;
using System.Security.Claims;

namespace PortfolioHub.Blogs.Endpoints.Blogs;

internal sealed class Delete
(
    ISender sender
) : EndpointWithoutRequest<Result>
{
    public override void Configure()
    {
        Delete("/blogs/{id}");
        Roles(["admin", "contributor"]);
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

        var deleteBlogId = Route<Guid>("id");

        var deleteProjectCommand = new DeleteBlogCommand(deleteBlogId, userGuid);
        var result = await sender.Send(deleteProjectCommand, ct);
        if (!result.IsSuccess)
        {
            await SendAsync(result, StatusCodes.Status400BadRequest, cancellation: ct);
            return;
        }

        await SendAsync(result, StatusCodes.Status200OK, cancellation: ct);
    }
}
