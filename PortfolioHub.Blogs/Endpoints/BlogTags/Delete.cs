using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Blogs.Usecases.BlogTags.Delete;

namespace PortfolioHub.Blogs.Endpoints.BlogTags;

internal sealed class Delete
(
    ISender sender
) : EndpointWithoutRequest<Result>
{
    public override void Configure()
    {
        Delete("/blog-tags/{id}");
        Roles(["admin"]);
    }

    public async override Task HandleAsync(CancellationToken ct)
    {
        var deleteBlogTagId = Route<Guid>("id");

        var deleteBlogTagCommand = new DeleteBlogTagCommand(deleteBlogTagId);
        var result = await sender.Send(deleteBlogTagCommand, ct);
        if (!result.IsSuccess)
        {
            await SendAsync(result, StatusCodes.Status400BadRequest, cancellation: ct);
            return;
        }

        await SendAsync(result, StatusCodes.Status200OK, cancellation: ct);
    }
}
