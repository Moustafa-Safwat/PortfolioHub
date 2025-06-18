using Ardalis.Result;
using FastEndpoints;
using MediatR;
using PortfolioHub.Projects.Usecases.Project;

namespace PortfolioHub.Projects.Endpoints.Project;


internal class GetById(
    ISender sender
    ) : EndpointWithoutRequest<Result<ProjectDto>>
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
        var projectDto = projectByIdRes.Value;
        var response = Result.Success(projectDto);
        await SendAsync(response, cancellation: ct);

    }
}
