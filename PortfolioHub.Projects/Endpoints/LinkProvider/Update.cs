using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Projects.Usecases.LinkProvider;

namespace PortfolioHub.Projects.Endpoints.LinkProvider;

internal sealed record UpdateLinkProviderReq(
    Guid Id,
    string Name,
    string BaseUrl
    );

internal sealed class Update(
    ISender sender
    ) : Endpoint<UpdateLinkProviderReq, Result>
{
    public override void Configure()
    {
        Put("/link-provider");
        Roles("admin");
    }

    public override async Task HandleAsync(UpdateLinkProviderReq req, CancellationToken ct)
    {
        var updateLinkProviderCommand = new UpdateLinkProviderCommand(
            req.Id,
            req.Name,
            req.BaseUrl);

        var result = await sender.Send(updateLinkProviderCommand, ct);
        if (!result.IsSuccess)
        {
            await SendAsync(result, StatusCodes.Status400BadRequest, ct);
            return;
        }
        await SendAsync(result, StatusCodes.Status200OK, ct);
    }
}
