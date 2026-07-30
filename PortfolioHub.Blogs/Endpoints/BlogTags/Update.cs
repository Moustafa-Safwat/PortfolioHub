using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Blogs.Usecases.BlogTags.Update;

namespace PortfolioHub.Blogs.Endpoints.BlogTags;

internal sealed class Update
(
    ISender sender
) : Endpoint<UpdateBlogTagRequest, Result>
{
    public override void Configure()
    {
        Put("/blog-tags");
        Roles(["admin"]);
    }

    public override async Task HandleAsync(UpdateBlogTagRequest req, CancellationToken ct)
    {
        var updateBlogTagCommand = new UpdateBlogTagCommand(req.Id, req.Name);
        var updateResult = await sender.Send(updateBlogTagCommand, ct);
        if (!updateResult.IsSuccess)
        {
            await SendAsync(updateResult, StatusCodes.Status400BadRequest, ct);
            return;
        }
        await SendAsync(updateResult, StatusCodes.Status200OK, ct);
    }
}
