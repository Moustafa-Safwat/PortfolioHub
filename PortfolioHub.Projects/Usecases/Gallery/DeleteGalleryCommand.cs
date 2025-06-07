using Ardalis.Result;
using MediatR;

namespace PortfolioHub.Projects.Usecases.Gallery;

internal sealed record DeleteGalleryCommand(
    Guid Id
    ) : IRequest<Result<string>>;
