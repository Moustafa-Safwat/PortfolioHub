using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Blogs.Usecases.Blogs.Update;
using System.Security.Claims;

namespace PortfolioHub.Blogs.Endpoints.Blogs;

internal sealed class Update(
    ISender sender
) : Endpoint<UpdateBlogRequest, Result>
{
    public override void Configure()
    {
        Put("/blogs/{id}");
        Roles(["admin", "contributor"]);
        Claims(ClaimTypes.NameIdentifier);
    }

    public async override Task HandleAsync(UpdateBlogRequest req, CancellationToken ct)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out Guid userGuid))
        {
            var error = Result.Error("Invalid token.");
            await SendAsync(error, StatusCodes.Status400BadRequest, ct);
            return;
        }

        var blogId = Route<Guid>("id");

        var updateCommand = new UpdateBlogPostCommand(req, userGuid, blogId);
        var updateResult = await sender.Send(updateCommand, ct);

        if (!updateResult.IsSuccess)
        {
            await SendAsync(updateResult, StatusCodes.Status400BadRequest, ct);
            return;
        }

        await SendAsync(updateResult, StatusCodes.Status200OK, ct);
    }
}
