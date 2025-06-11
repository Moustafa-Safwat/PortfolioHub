using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Projects.Usecases.Category;

namespace PortfolioHub.Projects.Endpoints.Category;

internal sealed class Delete(
    ISender sender
    ) : EndpointWithoutRequest<Result<Guid>>
{
    public override void Configure()
    {
        Delete("/category/{id}");
        Roles("admin");
    }
    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<string>("id");
        if (!Guid.TryParse(id, out Guid categoryId))
        {
            var invalidResult = Result.Invalid(new ValidationError
            {
                ErrorMessage = "Invalid category ID format."
            });
            await SendAsync(invalidResult, StatusCodes.Status400BadRequest, ct);
            return;
        }

        var deleteCategoryCommand = new DeleteCategoryCommand(categoryId);
        var deleteResult = await sender.Send(deleteCategoryCommand, ct);

        if (!deleteResult.IsSuccess)
        {
            await SendAsync(deleteResult, StatusCodes.Status400BadRequest, ct);
            return;
        }

        var successedResult = Result.Success(categoryId, "Category is deleted successfully");
        await SendOkAsync(successedResult, ct);
    }
}
