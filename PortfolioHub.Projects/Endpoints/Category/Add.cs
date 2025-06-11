using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Projects.Usecases.Category;

namespace PortfolioHub.Projects.Endpoints.Category;

internal sealed class Add(
    ISender sender
    ) : Endpoint<AddCategoryReq, Result<Guid>>
{
    public override void Configure()
    {
        Post("/category");
        Roles("admin");
    }

    public async override Task HandleAsync(AddCategoryReq req, CancellationToken ct)
    {
        var addCategoryCommand = new AddCategoryCommand(req.Name);

        var addCatResult = await sender.Send(addCategoryCommand, ct);

        if (!addCatResult.IsSuccess)
        {
            await SendAsync(addCatResult, StatusCodes.Status400BadRequest, ct);
            return;
        }

        await SendOkAsync(addCatResult.Value, ct);
    }
}
