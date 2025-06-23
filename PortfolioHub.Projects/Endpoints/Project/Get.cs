using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Projects.Usecases.Project;

namespace PortfolioHub.Projects.Endpoints.Project;

internal sealed class Get(
    ISender sender
    ) : EndpointWithoutRequest<Result<GetProjectsResp>>
{
    public override void Configure()
    {
        Get("/project");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var category = Query<string>("CategoryId", false);
        var search = Query<string>("Search", false);
        var pageNumber = Query<int>("Page", false);
        var pageSize = Query<int>("PageSize", false);

        Guid? categoryId = null;
        if (!string.IsNullOrEmpty(category))
        {
            if (!Guid.TryParse(category, out Guid parsedCategoryId))
            {
                var errorObj = Result.Invalid(new ValidationError("Invalid category ID format."));
                await SendAsync(errorObj, StatusCodes.Status400BadRequest, cancellation: ct);
                return;
            }
            categoryId = parsedCategoryId;
        }

        var query = new GetProjectsQuery(pageNumber, pageSize, categoryId, search);
        var result = await sender.Send(query, ct);
        if (!result.IsSuccess)
        {
            await SendErrorsAsync(cancellation: ct);
            return;
        }
        var projectDtos = result.Value.ProjectsDtos
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
            TotalCount: result.Value.TotalCount,
            PageNumber: pageNumber,
            PageSize: pageSize
        );
        await SendAsync(getProjectResponse, cancellation: ct);
    }
}
