using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Projects.Usecases.Gallery;

namespace PortfolioHub.Projects.Endpoints.Gallery;

internal sealed class Get(
    ISender sender
    ) : Endpoint<GetGalleryReq, Result<GetGalleryRes>>
{
    public override void Configure()
    {
        Get("/gallery");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetGalleryReq req, CancellationToken ct)
    {
        var query = new GetGalleryQuery(req.PageNumber, req.PageSize);
        var result = await sender.Send(query, ct);

        if (result.IsSuccess)
        {
            await SendAsync(Result.Success(new GetGalleryRes(
                result.Value.Count,
                query.PageNumber,
                query.PageSize,
                result.Value
                    .Select(g => new GalleryDto(g.Id, g.ImageUrl, g.Order))
                    .ToList()
            )), cancellation: ct);
        }
        else
        {
            await SendAsync(Result.CriticalError(result.Errors.ToArray()),
                StatusCodes.Status400BadRequest, cancellation: ct);
        }
    }
}
