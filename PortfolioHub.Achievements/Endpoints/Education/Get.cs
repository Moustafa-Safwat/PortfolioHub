using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Achievements.Usecases.Education;

namespace PortfolioHub.Achievements.Endpoints.Education;
internal sealed class Get(
    ISender sender
    ) : Endpoint<GetEducationReq, Result<IEnumerable<EducationGetDto>>>
{
    public override void Configure()
    {
        Get("/education");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetEducationReq req, CancellationToken ct)
    {
        var query = new GetEducationQuery(req.Page, req.PageSize);
        var result = await sender.Send(query, ct);

        if (!result.IsSuccess)
        {
            await SendAsync(result, StatusCodes.Status400BadRequest, cancellation: ct);
            return;
        }

        await SendAsync(result, cancellation: ct);
    }
}
