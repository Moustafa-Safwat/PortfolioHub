using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Blogs.Usecases.BlogTags.Get;
using System.Collections.ObjectModel;

namespace PortfolioHub.Blogs.Endpoints.BlogTags;

internal class Get
(
    ISender sender
) : EndpointWithoutRequest<Result<ReadOnlyCollection<TagsDto>>>
{
    public override void Configure()
    {
        Get("/blog-tags");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var getBlogTagsQuery = new GetBlogTagsQuery();
        var result = await sender.Send(getBlogTagsQuery, ct);

        if (!result.IsSuccess)
        {
            var response = Result.Error(new ErrorList(result.Errors));
            await SendAsync(response, StatusCodes.Status400BadRequest, cancellation: ct);
            return;
        }

        await SendAsync(result, cancellation: ct);
    }
}
