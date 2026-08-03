using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Blogs.Usecases.BlogTags.Add;

namespace PortfolioHub.Blogs.Endpoints.BlogTags;

internal sealed class Add
(
    ISender sender
) : Endpoint<AddTagRequest, Result<Guid>>
{
    public override void Configure()
    {
        Post("/blog-tags");
        Roles(["admin"]);
    }
    public override async Task HandleAsync(AddTagRequest req, CancellationToken ct)
    {
        var addTagCommand = new AddTagCommand(req.Name);
        var result = await sender.Send(addTagCommand, ct);
        if (!result.IsSuccess)
        {
            await SendAsync(result, StatusCodes.Status400BadRequest, ct);
            return;
        }
        await SendAsync(result, StatusCodes.Status200OK, ct);
    }
}
