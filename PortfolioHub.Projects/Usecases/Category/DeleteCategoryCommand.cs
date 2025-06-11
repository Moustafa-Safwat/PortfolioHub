using Ardalis.Result;
using MediatR;

namespace PortfolioHub.Projects.Usecases.Category;

internal sealed record DeleteCategoryCommand(
    Guid Id
    ) : IRequest<Result<Guid>>;
