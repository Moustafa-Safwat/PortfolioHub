using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Achievements.Usecases.Education;

namespace PortfolioHub.Achievements.Endpoints.Education;

internal sealed class Delete(
    ISender sender
    ) : Endpoint<DeleteRequest, Result>
{
    public override void Configure()
    {
        Delete("/education/{Id}");
        Roles("admin");
    }

    public override async Task HandleAsync(DeleteRequest req, CancellationToken ct)
    {

        var deleteEducationCommand = new DeleteEducationCommand(Guid.Parse(req.Id));
        var result = await sender.Send(deleteEducationCommand, ct);
        if (!result.IsSuccess)
        {
            await SendAsync(result, StatusCodes.Status400BadRequest, ct);
            return;
        }
        var successObj = Result.SuccessWithMessage("The education entry was deleted successfully.");
        await SendOkAsync(successObj, ct);

    }
}
