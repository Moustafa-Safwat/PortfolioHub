using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Blogs.Usecases.Blogs.GetById;
using System.Security.Claims;

namespace PortfolioHub.Blogs.Endpoints.Blogs;

internal sealed class GetById
(
    ISender sender
) : EndpointWithoutRequest<Result<BlogDetailsDto>>
{
    public override void Configure()
    {
        Get("/blogs/{id}");
        Claims(ClaimTypes.NameIdentifier);
    }

    public async override Task HandleAsync(CancellationToken ct)
    {
        var blogId = Route<Guid>("id");

        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out Guid userGuid))
        {
            var error = Result.Error("Invalid token.");
            await SendAsync(error, StatusCodes.Status400BadRequest, ct);
            return;
        }

        var query = new GetBlogByIdQuery(blogId, userGuid);
        var result = await sender.Send(query, ct);

        if (!result.IsSuccess)
        {
            // propagate the error status and return
            await SendAsync(result, StatusCodes.Status404NotFound, ct);
            return;
        }

        await SendAsync(result, StatusCodes.Status200OK, ct);
    }
}

