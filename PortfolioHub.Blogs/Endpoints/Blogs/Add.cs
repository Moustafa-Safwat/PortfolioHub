using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Blogs.Usecases.Blogs.Add;
using System.Security.Claims;

namespace PortfolioHub.Blogs.Endpoints.Blogs;

internal sealed class Add
(
    ISender sender
) : Endpoint<AddBlogRequest, Result<Guid>>
{
    public override void Configure()
    {
        Post("/blogs");
        Roles(["admin", "contributor"]);
        Claims(ClaimTypes.NameIdentifier);
    }

    public async override Task HandleAsync(AddBlogRequest req, CancellationToken ct)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out Guid userGuid))
        {
            var error = Result.Error("Invalid token.");
            await SendAsync(error, StatusCodes.Status400BadRequest, ct);
            return;
        }

        var addBlogPostCommand = new AddBlogPostCommand(req, userGuid);
        var addBlogPostResult = await sender.Send(addBlogPostCommand);

        if (!addBlogPostResult.IsSuccess)
        {
            await SendAsync(addBlogPostResult, StatusCodes.Status400BadRequest, ct);
            return;
        }

        await SendAsync(addBlogPostResult, StatusCodes.Status201Created, ct);
    }
}
