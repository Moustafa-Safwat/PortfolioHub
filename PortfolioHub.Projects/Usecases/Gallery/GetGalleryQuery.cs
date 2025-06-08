using Ardalis.Result;
using MediatR;
using PortfolioHub.Projects.Domain.Entities;
using PortfolioHub.Projects.Endpoints.Gallery;

namespace PortfolioHub.Projects.Usecases.Gallery;

internal sealed record GetGalleryQuery(
    int PageNumber,
    int PageSize
    ) : IRequest<Result<IReadOnlyList<GalleryDto>>>;
