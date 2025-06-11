using Ardalis.Result;
using MediatR;

namespace PortfolioHub.Projects.Usecases.Category;

internal sealed record AddCategoryCommand(string Name) : IRequest<Result<Guid>>;
