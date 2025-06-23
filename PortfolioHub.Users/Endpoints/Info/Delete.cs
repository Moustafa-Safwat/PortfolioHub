using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Users.Usecases.Info;

namespace PortfolioHub.Users.Endpoints.Info;

internal sealed class Delete(
    ISender sender
    ) : Endpoint<DeleteInfoRequest, Result>
{
    public override void Configure()
    {
        Delete("/info/{key}");
        Roles("admin");
    }
    public override async Task HandleAsync(DeleteInfoRequest req, CancellationToken ct)
    {
        var deleteInfoCommand = new DeleteInfoCommand(Guid.Parse(req.key));
        var result = await sender.Send(deleteInfoCommand, ct);
        if (!result.IsSuccess)
        {
            await SendAsync(result, StatusCodes.Status400BadRequest, ct);
            return;
        }
        var successObj = Result.SuccessWithMessage("The info entry was deleted successfully.");
        await SendOkAsync(successObj, ct);

    }
}
