namespace PortfolioHub.Projects.Endpoints.Gallery;

internal sealed record GalleryDto(
    Guid Id,
    string ImageUrl,
    int Order
    );
