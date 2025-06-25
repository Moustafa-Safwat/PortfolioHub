using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Users.Usecases.ProfessionalSkills;

namespace PortfolioHub.Users.Endpoints.ProfessionalSkills;
internal sealed class Add(
    ISender sender
    ):Endpoint<AddProfessionalSkillReq, Result<Guid>>
{
    public override void Configure()
    {
        Post("/professional-skills");
        Roles("admin");
    }
    public override async Task HandleAsync(AddProfessionalSkillReq req, CancellationToken ct)
    {
        var addProfessionalSkillCommand = new AddProfessionalSkillCommand(req.Name);
        var result = await sender.Send(addProfessionalSkillCommand, ct);
        if (!result.IsSuccess)
        {
            await SendAsync(result, StatusCodes.Status400BadRequest, ct);
            return;
        }
        await SendAsync(result, StatusCodes.Status201Created, ct);
    }
}
