using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Projects.Usecases.LinkProvider;

namespace PortfolioHub.Projects.Endpoints.LinkProvider;

internal sealed class Add(
    ISender sender
    ) : Endpoint<AddLinkProviderReq, Result<Guid>>
{
    public override void Configure()
    {
        Post("/link-provider");
        Roles("admin");
    }

    public async override Task HandleAsync(AddLinkProviderReq req, CancellationToken ct)
    {
        var addLinkProviderCommand = new AddLinkProviderCommand(req.Name, req.BaseUrl);
        var result = await sender.Send(addLinkProviderCommand, ct);
        if (!result.IsSuccess)
        {
            await SendAsync(result, StatusCodes.Status400BadRequest, ct);
            return;
        }
        await SendAsync(result, StatusCodes.Status201Created, ct);

    }
}
