using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Projects.Usecases.TechnicalSkill;

namespace PortfolioHub.Projects.Endpoints.TechnicalSkill;
internal sealed class Update(
    ISender sender
    ):Endpoint<UpdateTechnicalSkillRequest, Result>
{
    public override void Configure()
    {
        Put("/tech-skill");
        Roles("admin");
    }

    public async override Task HandleAsync(UpdateTechnicalSkillRequest req, CancellationToken ct)
    {
        var updateTechnicalSkillCommand = new UpdateTechnicalSkillCommand(req.Id, req.Name);

        var result = await sender.Send(updateTechnicalSkillCommand, ct);

        if (!result.IsSuccess)
        {
            await SendAsync(result, StatusCodes.Status400BadRequest, ct);
            return;
        }

        await SendOkAsync(result, ct);
    }
}
