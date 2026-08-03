using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Blogs.Usecases.Blogs.GetBySlug;
using System.Security.Claims;

namespace PortfolioHub.Blogs.Endpoints.Blogs;

internal sealed class GetBySlug
(
    ISender sender
) : Endpoint<GetBySlugRequest, Result<BlogDetailsDto>>

{
    public override void Configure()
    {
        Get("/blogs/{slug}");
        Claims(ClaimTypes.NameIdentifier);
    }

    public async override Task HandleAsync(GetBySlugRequest req, CancellationToken ct)
    {

        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out Guid userGuid))
        {
            var error = Result.Error("Invalid token.");
            await SendAsync(error, StatusCodes.Status401Unauthorized, ct);
            return;
        }

        var query = new GetBlogBySlugQuery(req.Slug, userGuid);
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
