using Ardalis.Result;
using FastEndpoints;
using MediatR;
using PortfolioHub.Projects.Usecases.Project;

namespace PortfolioHub.Projects.Endpoints.Project;

internal sealed class Get(
    ISender sender
    ) : Endpoint<GetProjectsReq, Result<GetProjectsResp>>
{
    public override void Configure()
    {
        Get("/project");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetProjectsReq req, CancellationToken ct)
    {
        var query = new GetProjectsQuery(req.PageNumber, req.PageSize);
        var result = await sender.Send(query, ct);
        if (!result.IsSuccess)
        {
            await SendErrorsAsync(cancellation: ct);
            return;
        }
        var projectDtos = result.Value
            .Select(p => new ProjectsDto(
                p.Id,
                p.Title,
                p.Description,
                p.CreatedAt,
                p.CoverImageUrl,
                p.Tech,
                p.Category,
                p.gitHubLink
            ))
            .ToList()
            .AsReadOnly();

        var getProjectResponse = new GetProjectsResp(
            Projects: projectDtos,
            TotalCount: result.Value.Count,
            PageNumber: req.PageNumber,
            PageSize: req.PageSize
        );
        await SendAsync(getProjectResponse, cancellation: ct);
    }
}
