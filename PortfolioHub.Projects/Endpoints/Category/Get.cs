using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Projects.Usecases.Category;

namespace PortfolioHub.Projects.Endpoints.Category;

internal sealed class Get(
    ISender sender
    ) : EndpointWithoutRequest<Result<IEnumerable<CategoryDto>>>
{
    public override void Configure()
    {
        Get("/category");
        AllowAnonymous();
    }
    public async override Task HandleAsync(CancellationToken ct)
    {
        var getCategoriesQuery = new GetCategoriesQuery();
        var getCatResult = await sender.Send(getCategoriesQuery, ct);
        if (!getCatResult.IsSuccess)
        {
            await SendAsync(getCatResult, StatusCodes.Status400BadRequest, ct);
            return;
        }

        await SendOkAsync(getCatResult, ct);
    }
}
