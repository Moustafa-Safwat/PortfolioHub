using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Projects.Usecases.Project;

namespace PortfolioHub.Projects.Endpoints.Project;

internal sealed record DeleteProjResp(
    Guid Id,
    string Message
);

internal class Delete(
    ISender sender
    ) : EndpointWithoutRequest<Result<DeleteProjResp>>
{
    public override void Configure()
    {
        Delete("/project/{id}");
        Roles("admin");
    }

    public async override Task HandleAsync(CancellationToken ct)
    {
        var deleteProjectId = Route<Guid>("id");
        var deleteProjectCommand = new DeleteProjectCommand(deleteProjectId);
        var result = await sender.Send(deleteProjectCommand, ct);
        if (!result.IsSuccess)
        {
            await SendAsync(result, StatusCodes.Status400BadRequest, cancellation: ct);
            return;
        }
        var response = new DeleteProjResp(
            Id: deleteProjectId,
            Message: "Project deleted successfully."
        );
        await SendAsync(response, cancellation: ct);

    }
}
