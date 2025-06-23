using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Users.Usecases.Info;

namespace PortfolioHub.Users.Endpoints.Info;
internal sealed class Get(
    ISender sender
    ) : Endpoint<GetInfoByKeysRequest, Result<IEnumerable<InfoGetDto>>>
{
    public override void Configure()
    {
        Get("/info");
        AllowAnonymous();
    }
    public override async Task HandleAsync(GetInfoByKeysRequest req, CancellationToken ct)
    {
        var getInfoByKeysQuery = new GetInfoByKeysQuery(req.Keys);
        var queryResult = await sender.Send(getInfoByKeysQuery, ct);

        if (!queryResult.IsSuccess)
        {
            await SendAsync(queryResult, StatusCodes.Status400BadRequest, ct);
            return;
        }

        await SendAsync(queryResult, StatusCodes.Status200OK, ct);
    }
}


