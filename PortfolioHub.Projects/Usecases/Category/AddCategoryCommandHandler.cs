using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Interfaces;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using Serilog;

namespace PortfolioHub.Projects.Usecases.Category;

internal sealed class AddCategoryCommandHandler(
    IEntityRepo<Domain.Entities.Category> categoryRepo
    ) : IRequestHandler<AddCategoryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {

        var newCategory = new Domain.Entities.Category(Guid.NewGuid(), request.Name);
        var addResult = await categoryRepo.AddAsync(newCategory, cancellationToken);
        if (!addResult.IsSuccess)
            return Result<Guid>.Error(new ErrorList(addResult.Errors)); 

        var saveResult = await categoryRepo.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
            return Result<Guid>.Error(new ErrorList(addResult.Errors));

        return Result<Guid>.Success(newCategory.Id);
    }
}