using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Projects.Usecases.TechnicalSkill;

namespace PortfolioHub.Projects.Endpoints.TechanicalSkills;

internal sealed record TechSkillDto(Guid Id, string Name);
internal sealed class Get(
    ISender sender
    ) : EndpointWithoutRequest<Result<IEnumerable<TechSkillDto>>>
{
    public override void Configure()
    {
        Get("/tech-skill");
        AllowAnonymous();
    }

    public async override Task HandleAsync(CancellationToken ct)
    {
        var getTechnicalSkillQuery = new GetTechnicalSkillQuery();
        var result = await sender.Send(getTechnicalSkillQuery, ct);

        if (!result.IsSuccess)
        {
            var response = Result.Error(new ErrorList(result.Errors));
            await SendAsync(response, StatusCodes.Status400BadRequest, cancellation: ct);
            return;
        }

        await SendAsync(result, cancellation: ct);
    }
}
