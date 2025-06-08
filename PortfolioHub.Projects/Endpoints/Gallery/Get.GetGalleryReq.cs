namespace PortfolioHub.Projects.Endpoints.Gallery;

internal sealed record GetGalleryReq(
    int PageNumber = 0,
    int PageSize = 10
    );
