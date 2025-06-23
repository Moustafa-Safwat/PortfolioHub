using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Users.Usecases.Info;

namespace PortfolioHub.Users.Endpoints.Info;

internal sealed class Add(
    ISender sender
    ) : Endpoint<AddInfoReq, Result<Guid[]>>
{
    public override void Configure()
    {
        Post("/info");
        Roles("admin");
    }

    public async override Task HandleAsync(AddInfoReq req, CancellationToken ct)
    {
        var addInfosCommand = new AddInfosCommand(req.Infos);
        var result = await sender.Send(addInfosCommand, ct);

        if (!result.IsSuccess)
        {
            await SendAsync(result, StatusCodes.Status400BadRequest, ct);
            return;
        }

        var successObj = result.Value;
        await SendAsync(successObj, StatusCodes.Status201Created, ct);
    }
}
