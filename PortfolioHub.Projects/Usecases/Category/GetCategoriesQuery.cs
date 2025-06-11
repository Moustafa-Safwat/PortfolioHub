using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Interfaces;
using PortfolioHub.Projects.Endpoints.Category;
using Serilog;

namespace PortfolioHub.Projects.Usecases.Category;

internal sealed record GetCategoriesQuery()
    : IRequest<Result<IEnumerable<CategoryDto>>>;

internal sealed class GetCategoriesQueryHandler(
    IEntityRepo<Domain.Entities.Category> categoryRepo,
    ILogger logger
    ) : IRequestHandler<GetCategoriesQuery, Result<IEnumerable<CategoryDto>>>
{
    public async Task<Result<IEnumerable<CategoryDto>>> Handle(
        GetCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        logger.Information("Handling GetCategoriesQuery...");

        var categoriesResult = await categoryRepo.GetAllAsync(1, int.MaxValue, cancellationToken);
        if (!categoriesResult.IsSuccess)
        {
            logger.Warning("Failed to retrieve categories: {@Errors}", categoriesResult.Errors);
            return Result<IEnumerable<CategoryDto>>.Error(new ErrorList(categoriesResult.Errors));
        }

        var categories = categoriesResult.Value;
        var categoryDtos = categories.Select(c => new CategoryDto
        (
           c.Id,
           c.Name
        ));

        logger.Information("Successfully retrieved {Count} categories.", categoryDtos.Count());

        return Result<IEnumerable<CategoryDto>>.Success(categoryDtos);
    }
}