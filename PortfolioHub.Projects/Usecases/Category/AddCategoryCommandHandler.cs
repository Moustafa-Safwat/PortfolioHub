using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Interfaces;
using Serilog;

namespace PortfolioHub.Projects.Usecases.Category;

internal sealed class AddCategoryCommandHandler(
    IEntityRepo<Domain.Entities.Category> categoryRepo,
    ILogger logger
    ) : IRequestHandler<AddCategoryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {
        logger.Information("Attempting to add a new category with name: {CategoryName}", request.Name);

        var newCategory = new Domain.Entities.Category(Guid.NewGuid(), request.Name);
        var addResult = await categoryRepo.AddAsync(newCategory, cancellationToken);
        if (!addResult.IsSuccess)
        {
            logger.Error("Failed to add category: {CategoryName}. Errors: {Errors}", request.Name, addResult.Errors);
            return Result<Guid>.Error(new ErrorList(addResult.Errors)); 
        }

        var saveResult = await categoryRepo.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
        {
            logger.Error("Failed to save new category: {CategoryName}. Errors: {Errors}", request.Name, saveResult.Errors);
            return Result<Guid>.Error(new ErrorList(addResult.Errors));
        }

        logger.Information("Successfully added new category with Id: {CategoryId}", newCategory.Id);
        return Result<Guid>.Success(newCategory.Id);
    }
}