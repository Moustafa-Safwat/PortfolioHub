using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Users.Usecases.ProfessionalSkills;

namespace PortfolioHub.Users.Endpoints.ProfessionalSkills;
internal sealed class Get(
    ISender sender
    ) : Endpoint<GetProfessionalSkillReq, Result<IEnumerable<ProfessionalSkillGetDto>>>
{
    public override void Configure()
    {
        Get("/professional-skills");
        AllowAnonymous();
    }
    public override async Task HandleAsync(GetProfessionalSkillReq req, CancellationToken ct)
    {
        var query = new GetProfessionalSkillsQuery(req.Page, req.PageSize);
        var result = await sender.Send(query, ct);
        if (!result.IsSuccess)
        {
            await SendAsync(result, StatusCodes.Status400BadRequest, cancellation: ct);
            return;
        }
        await SendAsync(result, cancellation: ct);
    }
}
