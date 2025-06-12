using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Projects.Usecases.LinkProvider;

namespace PortfolioHub.Projects.Endpoints.LinkProvider;

internal sealed record LinkProviderDto(
    Guid Id,
    string Name,
    string BaseUrl
);

internal sealed class Get(
    ISender sender
    ) : EndpointWithoutRequest<Result<IEnumerable<LinkProviderDto>>>
{
    public override void Configure()
    {
        Get("/link-provider");
        AllowAnonymous();
    }
    public async override Task HandleAsync(CancellationToken ct)
    {
        var getLinkProvidersQuery = new GetLinkProvidersQuery();
        var result = await sender.Send(getLinkProvidersQuery, ct);
        if (!result.IsSuccess)
        {
            await SendAsync(result, StatusCodes.Status400BadRequest, cancellation: ct);
            return;
        }
        await SendAsync(result, cancellation: ct);
    }
}
