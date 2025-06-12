using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Projects.Usecases.TechnicalSkill;

namespace PortfolioHub.Projects.Endpoints.TechnicalSkill;

internal sealed class Add(
    ISender sender
    ) : Endpoint<TechnicalSkillRequest, Result<Guid>>
{
    public override void Configure()
    {
        Post("/tech-skill");
        Roles("admin");
    }

    public async override Task HandleAsync(TechnicalSkillRequest req, CancellationToken ct)
    {
        var addTechSkillCommand = new AddTechnicalSkillCommand(req.Name);
        var result = await sender.Send(addTechSkillCommand, ct);
        if (!result.IsSuccess)
        {
            await SendAsync(result, StatusCodes.Status400BadRequest, ct);
            return;
        }
        await SendAsync(result, StatusCodes.Status201Created, ct);
    }
}
