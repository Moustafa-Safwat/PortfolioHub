namespace PortfolioHub.Projects.Endpoints.Gallery;

internal sealed record AddGalleryReq(
    string ImageUrl,
    int Order
);
