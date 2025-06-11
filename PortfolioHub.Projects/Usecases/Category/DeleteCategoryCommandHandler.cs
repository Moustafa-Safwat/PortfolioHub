using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Interfaces;
using Serilog;

namespace PortfolioHub.Projects.Usecases.Category;

internal sealed class DeleteCategoryCommandHandler(
    IEntityRepo<Domain.Entities.Category> categoryRepo,
    ILogger logger
    ) : IRequestHandler<DeleteCategoryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        logger.Information("DeleteCategoryCommandHandler started for CategoryId: {CategoryId}", request.Id);

        if (request.Id == Guid.Empty)
        {
            logger.Warning("DeleteCategoryCommandHandler failed: Category ID is empty.");
            return Result.Invalid(new ValidationError
            {
                ErrorMessage = "Category ID cannot be empty."
            });
        }

        var isCategoryIdValidResult = await categoryRepo.IsEntitiyIdValidAsync(request.Id, cancellationToken);
        if (!isCategoryIdValidResult.IsSuccess)
        {
            logger.Warning("DeleteCategoryCommandHandler failed: Category with ID {CategoryId} not found.", request.Id);
            return Result.NotFound();
        }

        var deleteCategoryResult = await categoryRepo.DeleteAsync(request.Id, cancellationToken);
        var saveResult = await categoryRepo.SaveChangesAsync(cancellationToken);
        if (!(deleteCategoryResult.IsSuccess && saveResult.IsSuccess))
        {
            logger.Error("DeleteCategoryCommandHandler failed: Could not delete category with ID {CategoryId}.", request.Id);
            return Result.Error("Failed to delete the category.");
        }

        logger.Information("DeleteCategoryCommandHandler succeeded: Category with ID {CategoryId} deleted.", request.Id);
        return Result.Success(request.Id, "Category is deleted successfully");
    }
}
