using Ardalis.Result;
using MediatR;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using Serilog;

namespace PortfolioHub.Projects.Usecases.Category;

internal sealed class DeleteCategoryCommandHandler(
    IEntityRepo<Domain.Entities.Category> categoryRepo
    ) : IRequestHandler<DeleteCategoryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {

        if (request.Id == Guid.Empty)
            return Result.Invalid(new ValidationError
            {
                ErrorMessage = "Category ID cannot be empty."
            });

        var isCategoryIdValidResult = await categoryRepo.IsEntitiyIdValidAsync(request.Id, cancellationToken);
        if (!isCategoryIdValidResult.IsSuccess)
            return Result.NotFound();

        var deleteCategoryResult = await categoryRepo.DeleteAsync(request.Id, cancellationToken);
        var saveResult = await categoryRepo.SaveChangesAsync(cancellationToken);
        if (!(deleteCategoryResult.IsSuccess && saveResult.IsSuccess))
            return Result.Error("Failed to delete the category.");

        return Result.Success(request.Id, "Category is deleted successfully");
    }
}
