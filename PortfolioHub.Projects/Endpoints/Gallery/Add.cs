using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Projects.Usecases.Gallery;

namespace PortfolioHub.Projects.Endpoints.Gallery;

internal class Add(
    ISender sender
    ) : Endpoint<AddGalleryReq, Result<Guid>>
{
    public override void Configure()
    {
        Post("/Gallery");
        Roles("admin");
    }

    public override async Task HandleAsync(AddGalleryReq req, CancellationToken ct)
    {
        var createGalleryCommand = new CreateGallaryCommand(
            req.ImageUrl,
            req.Order
        );

        var result = await sender.Send(createGalleryCommand, ct);

        if (result.IsSuccess)
            await SendOkAsync(result, ct);
        else
            await SendAsync(result, StatusCodes.Status400BadRequest, cancellation: ct);
    }
}
