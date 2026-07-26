using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Blogs.Usecases.Get;

namespace PortfolioHub.Blogs.Endpoints;

internal sealed class Get
(
    ISender sender
) : EndpointWithoutRequest<Result<GetBlogsResponse>>
{
    public override void Configure()
    {
        Get("/blogs");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var tagIds = Query<List<Guid>>("TagIds", false) ?? new List<Guid>();
        var search = Query<string>("Search", false) ?? string.Empty;
        var pageNumber = Query<int>("Page", false);
        var pageSize = Query<int>("PageSize", false);
        var isFeatured = Query<bool>("IsFeatured", false);

        var query = new GetBlogPostQuery(tagIds, search, pageNumber, pageSize, isFeatured);
        var result = await sender.Send(query, ct);

        if (!result.IsSuccess)
        {
            await SendAsync(result, StatusCodes.Status400BadRequest, ct);
            return;
        }

        await SendAsync(result, StatusCodes.Status200OK, ct);
    }
}
