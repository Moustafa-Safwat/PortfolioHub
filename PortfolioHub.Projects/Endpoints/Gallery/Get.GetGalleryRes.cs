namespace PortfolioHub.Projects.Endpoints.Gallery;

internal sealed record GetGalleryRes(
    int TotalCount,
    int PageNumber,
    int PageSize,
    IReadOnlyList<GalleryDto> Galleries
    );
