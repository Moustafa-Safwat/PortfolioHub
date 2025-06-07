using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Projects.Usecases.Gallery;

namespace PortfolioHub.Projects.Endpoints.Gallery;

internal class Delete (
    ISender sender
    ): EndpointWithoutRequest<Result<string>>
{
    public override void Configure()
    {
        Delete("/gallery/{id}");
        Roles("admin");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var galleryId = Route<Guid>("id");
        var deleteGalleryCommand = new DeleteGalleryCommand(galleryId);
        var result = await sender.Send(deleteGalleryCommand, ct);
        if (!result.IsSuccess)
        {
            await SendAsync(result, StatusCodes.Status400BadRequest, cancellation: ct);
            return;
        }
        var responseMessage = $"Gallery with ID {galleryId} deleted successfully.";
        await SendAsync(result, cancellation: ct);
    }
}
