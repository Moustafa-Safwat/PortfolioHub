using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Projects.Usecases.Project;

namespace PortfolioHub.Projects.Endpoints.Project;

internal class Add(
    ISender sender
    ) : Endpoint<AddProjectReq, Result<Guid>>
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
            req.LongDescription,
            req.CreatedDate,
            req.IsFeatured,
            Guid.Parse(req.CategoryId),
            req.VideoId,
            req.CoverImageUrl,
            req.ImagesUrls,
            req.SkillsId.Select(skillId => Guid.Parse(skillId)).ToArray(),
            req.Links.Select(link => (Guid.Parse(link.ProviderId), link.Link)).ToArray()
        );

        var result = await sender.Send(addProjectCommand, ct);
        if (result.IsSuccess)
        {
            var response = new AddProjectRes(result.Value);
            await SendOkAsync(result, ct);
        }
        else
        {
            await SendAsync(result, StatusCodes.Status400BadRequest, cancellation: ct);
        }
    }
}
