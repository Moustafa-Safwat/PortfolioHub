using Ardalis.Result;
using MediatR;

namespace PortfolioHub.Projects.Usecases.Gallery;

internal sealed record CreateGallaryCommand(
    string ImageUrl,
    int Order
    ) : IRequest<Result<Guid>>;
