using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Projects.Usecases.TechnicalSkill;

namespace PortfolioHub.Projects.Endpoints.TechanicalSkills;

internal sealed class Delete(
    ISender sender
    ) : EndpointWithoutRequest<Result<Guid>>
{
    public override void Configure()
    {
        Delete("/tech-skill/{id}");
        Roles("admin");
    }
    public async override Task HandleAsync(CancellationToken ct)
    {
        var id = Route<string>("id");
        if (!Guid.TryParse(id, out var guidId) || guidId == Guid.Empty)
        {
            await SendAsync(Result.Error("Invalid ID format."), StatusCodes.Status400BadRequest, ct);
            return;
        }

        var deleteTechSkillCommand = new DeleteTechnicalSkillCommand(guidId);
        var result = await sender.Send(deleteTechSkillCommand, ct);

        if (!result.IsSuccess)
        {
            await SendAsync(result, StatusCodes.Status400BadRequest, ct);
            return;
        }

        await SendAsync(result, StatusCodes.Status204NoContent, ct);
    }
}
