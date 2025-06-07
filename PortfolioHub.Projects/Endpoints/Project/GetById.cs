using FastEndpoints;
using MediatR;
using PortfolioHub.Projects.Usecases.Project;

namespace PortfolioHub.Projects.Endpoints.Project;

internal sealed record GetProjectByIdResp(ProjectDto ProjectDto);

internal class GetById(
    ISender sender
    ) : EndpointWithoutRequest<GetProjectByIdResp>
{
    public override void Configure()
    {
        Get("/project/{id}");
        AllowAnonymous();
    }

    public async override Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("id");
        var getProjectByIdQuery = new GetProjectByIdQuery(id);
        var projectByIdRes = await sender.Send(getProjectByIdQuery);
        if (!projectByIdRes.IsSuccess)
        {
            await SendErrorsAsync();
            return;
        }
        var projectDto = new ProjectDto(
            projectByIdRes.Value.Id,
            projectByIdRes.Value.Title,
            projectByIdRes.Value.Description,
            projectByIdRes.Value.CreatedAt
        );
        var response = new GetProjectByIdResp(projectDto);
        await SendAsync(response, cancellation: ct);

    }
}
