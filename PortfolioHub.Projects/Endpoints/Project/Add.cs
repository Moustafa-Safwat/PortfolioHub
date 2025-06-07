using FastEndpoints;
using MediatR;
using PortfolioHub.Projects.Usecases.Project;

namespace PortfolioHub.Projects.Endpoints.Project;

internal class Add(
    ISender sender
    ) : Endpoint<AddProjectReq, AddProjectRes>
{
    public override void Configure()
    {
        Post("/project");
        Roles("admin");
    }

    public override async Task HandleAsync(AddProjectReq req, CancellationToken ct)
    {
        var addProjectCommand = new AddProjectCommand(
            req.Title,
            req.Description,
            req.VideoUrl,
            req.CreatedAt
        );

        var result = await sender.Send(addProjectCommand, ct);
        if (result.IsSuccess)
        {
            var response = new AddProjectRes(result.Value);
            await SendOkAsync(response, ct);
        }
        else
        {
            await SendErrorsAsync(cancellation: ct);
        }
    }
}
